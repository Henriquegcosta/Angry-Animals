using Godot;
using System;

public partial class LevelButton : TextureButton
{
	static readonly Vector2 HOVER_SCALE = new Vector2(1.1f, 1.1f);
	static readonly Vector2 NORMAL_SCALE = new Vector2(1.0f, 1.0f);

	private PackedScene _levelsScene;

	[Export] private int _GetLevel { get; set; }
	[Export] private Label _levelNumber;
	[Export] private Label _levelScore;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LoadTexts();
		Pressed += OnButtonPressed;
		MouseEntered += ScaleUp;
		MouseExited += ScaleDown;
	}

	private void ScaleUp()
	{
		Scale = HOVER_SCALE;
	}

	private void ScaleDown()
	{
		Scale = NORMAL_SCALE;
	}

	private void OnButtonPressed()
	{
		ScoreManager.SetLevelSelected(_GetLevel);
		_levelsScene = GD.Load<PackedScene>($"res://Scenes/Level/Level{ScoreManager.GetLevelSelected}.tscn");
		GetTree().ChangeSceneToPacked(_levelsScene);
		//GetTree().ChangeSceneToFile($"res://Scenes/Level/Level{ScoreManager.GetLevelSelected}.tscn");
		GD.Print(ScoreManager.GetLevelSelected);
	}

	public void LoadTexts()
	{
		_levelNumber.Text = $"{_GetLevel}";
		_levelScore.Text = $"{ScoreManager.GetLevelBestScore(_GetLevel):D4}";
	}

}
