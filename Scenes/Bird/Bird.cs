using System.Linq;
using System.Reflection.Metadata;
using Godot;

public partial class Bird : RigidBody2D
{

	public enum BirdState { READY, DRAG, RELEASE }

	private static readonly Vector2 DRAG_LIM_MAX = new Vector2(0, 60);
	private static readonly Vector2 DRAG_LIM_MIN = new Vector2(-60, 0);

	private const float IMPULSE_MULT = 20.0F;
	private const float IMPULSE_MAX = 1200.0F;

	[Export] private AudioStreamPlayer2D _StrechSound;
	[Export] private AudioStreamPlayer2D _KickSound;
	[Export] private AudioStreamPlayer2D _LaunchSound;
	[Export] private Sprite2D _arrow;
	[Export] private VisibleOnScreenNotifier2D _visibleOnScreen;
	[Export] private Label _debugLabel;

	private BirdState _state = BirdState.READY;
	private float _arrowScaleX = 0.0f;
	private float _impulseLenght = 0.0f;
	private int _lastCollsionCount = 0;


	private Vector2 _start = Vector2.Zero;
	private Vector2 _dragStart = Vector2.Zero;
	private Vector2 _draggedVector = Vector2.Zero;
	private Vector2 _lastDraggedVector = Vector2.Zero;




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ConnectSignals();
		InitializeVariables();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		UpdateDebugLabel();
		UpdateState();
	}


	private void InitializeVariables()
	{
		_start = Position;
		_arrowScaleX = _arrow.Scale.X;
		_arrow.Hide();
	}

	private void ConnectSignals()
	{
		_visibleOnScreen.ScreenExited += OffScreen;
		InputEvent += GetInput;
		SleepingStateChanged += StateChanged;
	}

	private void UpdateDebugLabel()
	{
		_debugLabel.Text = $"St: {_state} Sl: {Sleeping}\n";
		_debugLabel.Text += $"X: {_dragStart.X:F1}, Y: {_dragStart.Y:F1}";

	}

	private void StartDragging()
	{
		_dragStart = GetGlobalMousePosition();
		_arrow.Show();
	}

	private Vector2 CalculateImpulse()
	{
		return _draggedVector * -IMPULSE_MULT;
	}

	private void StartRealease()
	{
		_arrow.Hide();
		_LaunchSound.Play();
		Freeze = false;
		ApplyCentralImpulse(CalculateImpulse());
		SignalManager.EmitOnAttempt();
	}

	private void ConstrainDragWithinLimits()
	{
		_lastDraggedVector = _draggedVector;
		_draggedVector = _draggedVector.Clamp(DRAG_LIM_MIN, DRAG_LIM_MAX);
		Position = _start + _draggedVector;
	}

	private void PlayStretchSound()
	{
		Vector2 diff = _draggedVector - _lastDraggedVector;
		if (diff.Length() > 0 && !_StrechSound.Playing)
		{
			_StrechSound.Play();
		}
	}

	private void UpdateDraggedVector()
	{
		_draggedVector = GetGlobalMousePosition() - _dragStart;
	}

	private bool DetectRelease()
	{
		// did we release "drag" and are we in drag start
		// if so, set state to RELEASE and return true
		// otherwise return false
		if (Input.IsActionJustReleased("drag") && _state == BirdState.DRAG)
		{
			ChangeState(BirdState.RELEASE);
			return true;
		}
		return false;

	}

	private void UpdateArrowScale()
	{
		_impulseLenght = CalculateImpulse().Length();
		float scalePercentage = _impulseLenght / IMPULSE_MAX;
		_arrow.Scale = new Vector2(
			(_arrowScaleX * scalePercentage) + _arrowScaleX,
			_arrow.Scale.Y
		);
		_arrow.Rotation = (_start - Position).Angle();
	}

	private void HandleDragging()
	{
		if (DetectRelease())
			return;

		UpdateDraggedVector();
		PlayStretchSound();
		ConstrainDragWithinLimits();
		UpdateArrowScale();


	}

	private void PlayKickSoundOnCollsion()
	{
		if (_lastCollsionCount == 0 && GetContactCount() > 0 && !_KickSound.Playing)
		{
			_KickSound.Play();
		}
		_lastCollsionCount = GetContactCount();


	}

	private void HandleFlying()
	{
		PlayKickSoundOnCollsion();
	}

	private void UpdateState()
	{
		switch (_state)
		{
			case BirdState.DRAG:
				HandleDragging();
				break;
			case BirdState.RELEASE:
				HandleFlying();
				break;
		}
	}

	private void ChangeState(BirdState newState)
	{
		_state = newState;
		switch (_state)
		{
			case BirdState.DRAG:
				StartDragging();
				break;
			case BirdState.RELEASE:
				StartRealease();
				break;
		}
	}

	private void StateChanged()
	{

		if (Sleeping)
		{
			foreach (Node2D body in GetCollidingBodies())
			{
				if (body is Cup cup)
				{
					cup.Die();
				}
			}
			CallDeferred("Die");
		}

	}


	private void GetInput(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (_state == BirdState.READY && @event.IsActionPressed("drag"))
		{
			ChangeState(BirdState.DRAG);
		}
	}

	private void Die()
	{
		SignalManager.EmitOnBirdDie();
		QueueFree();
	}

	private void OffScreen()
	{
		Die();
	}
}
