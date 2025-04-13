using Godot;
using System;

public partial class SignalManager : Node
{
	[Signal] public delegate void OnBirdDieEventHandler();
	[Signal] public delegate void OnCupDestroyedEventHandler();
	[Signal] public delegate void OnLevelCompletedEventHandler();
	[Signal] public delegate void OnAttemptEventHandler();
	[Signal] public delegate void OnAttemptUpdatedEventHandler(int num);
	[Signal] public delegate void OnScoreUpdateEventHandler(int _attempts);
	public static SignalManager Instance { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
	}

	public static void EmitOnScoreUpdate(int _attempts)
	{
		Instance.EmitSignal(SignalName.OnScoreUpdate, _attempts);
	}
	public static void EmitOnBirdDie()
	{
		Instance.EmitSignal(SignalName.OnBirdDie);
	}

	public static void EmitOnCupDestroyed()
	{
		Instance.EmitSignal(SignalName.OnCupDestroyed);
	}

	public static void EmitOnLevelCompleted()
	{
		Instance.EmitSignal(SignalName.OnLevelCompleted);
	}

	public static void EmitOnAttempt()
	{
		Instance.EmitSignal(SignalName.OnAttempt);
	}

	public static void EmitOnAttemptUpdate(int num)
	{
		Instance.EmitSignal(SignalName.OnAttemptUpdated, num);
	}


}
