using Godot;
using System;

public partial class Scorer : Node
{

	private int _totalCups = 0;
	private int _cupHits = 0;
	private int _attempts = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_totalCups = GetTree().GetNodesInGroup(Cup.GROUP_NAME).Count;
		SignalManager.Instance.OnCupDestroyed += OnCupDestroyed;
		SignalManager.Instance.OnAttempt += OnAttempt;
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnCupDestroyed -= OnCupDestroyed;
		SignalManager.Instance.OnAttempt -= OnAttempt;
	}

	private void OnAttempt()
	{
		_attempts++;
		SignalManager.EmitOnAttemptUpdate(_attempts);
	}

	private void OnCupDestroyed()
	{
		_cupHits++;
		GD.Print($"Now the cup counter is {_cupHits}");

		if (_totalCups == _cupHits)
		{
			SignalManager.EmitOnLevelCompleted();
			GD.Print("Level Completed!");
			ScoreManager.SetScoreForLevel(ScoreManager.GetLevelSelected, _attempts);
		}
	}
}
