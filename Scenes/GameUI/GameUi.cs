using Godot;
using System;

public partial class GameUi : Control
{
	[Export] private Label _LevelLabel;
	[Export] private Label _attemptsLabel;

	[Export] private VBoxContainer _vbGameOver;

	[Export] private AudioStreamPlayer _GameOverMusic;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_vbGameOver.Hide();
		_LevelLabel.Text = $"LEVEL: {ScoreManager.GetLevelSelected:00}";
		SignalManager.Instance.OnAttemptUpdated += OnAttemptUpdated;
		SignalManager.Instance.OnLevelCompleted += OnLevelCompleted;
	}

	private void OnAttemptUpdated(int num)
	{
		_attemptsLabel.Text = $"ATTEMPTS: {num:000}";
	}

	public override void _ExitTree()
	{
		SignalManager.Instance.OnAttemptUpdated -= OnAttemptUpdated;
		SignalManager.Instance.OnLevelCompleted -= OnLevelCompleted;
	}

	private void OnLevelCompleted()
	{
		_vbGameOver.Show();
		_GameOverMusic.Play();
	}

}
