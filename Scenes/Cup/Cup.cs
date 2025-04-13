using Godot;
using System;

public partial class Cup : StaticBody2D
{
	public const string GROUP_NAME = "cup";
	[Export] private AnimationPlayer _cupDie;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_cupDie.AnimationFinished += OnAnimationFinished;
	}

	/*     public override void _EnterTree()
		{
			AddToGroup(GROUP_NAME);
		} */

	private void OnAnimationFinished(StringName animName)
	{
		SignalManager.EmitOnCupDestroyed();
		QueueFree();
	}

	public void Die()
	{
		_cupDie.Play("vanish");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
