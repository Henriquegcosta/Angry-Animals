using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Level : Node2D
{
	[Export] private Marker2D _startLocate;
	[Export] private PackedScene _birdScene;

	private bool isGameOver = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SignalManager.Instance.OnBirdDie += SpawnAnimal;
		SignalManager.Instance.OnLevelCompleted += LevelCompleted;
		SpawnAnimal();
	}

	private void SpawnAnimal()
	{
		Bird _bird = _birdScene.Instantiate<Bird>();

		_bird.Position = _startLocate.Position;
		AddChild(_bird);
	}
	private void LevelCompleted()
	{
		isGameOver = true;
	}
	public override void _ExitTree()
	{
		SignalManager.Instance.OnBirdDie -= SpawnAnimal;
		SignalManager.Instance.OnLevelCompleted -= LevelCompleted;
	}

	public override void _Process(double delta)
	{
		if (Input.IsKeyPressed(Key.Q))
		{
			GetTree().ChangeSceneToFile($"res://Scenes/Main/Main.tscn");
		}
		if (Input.IsKeyPressed(Key.Space) && isGameOver == true)
		{
			GetTree().ChangeSceneToFile($"res://Scenes/Main/Main.tscn");
		}
	}



}
