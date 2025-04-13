using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ScoreManager : Node
{
	public static ScoreManager Instance { get; private set; }
	const int DEFAULT_SCORE = 1000;
	const string SCORE_FILE = "user://animals.save";
	private int _levelSelected;
	List<LevelScore> _levelScores = new List<LevelScore>();
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		LoadScores();
	}

	public override void _ExitTree()
	{
		SaveScores();
	}



	// ====== _levelSelected =============
	public static int GetLevelSelected => Instance._levelSelected;
	public static void SetLevelSelected(int level) => Instance._levelSelected = level;

	// ======== level score ==============

	// Get best score for level
	public static LevelScore GetLevelScore(int levelNumber)
	{
		return Instance._levelScores.FirstOrDefault(ls => ls.LevelNumber == levelNumber);
	}
	public static int GetLevelBestScore(int levelNumber)
	{
		LevelScore levelScore = GetLevelScore(levelNumber);
		if (levelScore != null)
		{
			return levelScore.BestScore;
		}
		return DEFAULT_SCORE;
	}

	//Set score for level
	public static void SetScoreForLevel(int levelNumber, int score)
	{
		LevelScore levelScore = GetLevelScore(levelNumber);
		if (levelScore != null)
		{
			if (score < levelScore.BestScore)
			{
				levelScore.BestScore = score;
				levelScore.DateSet = DateTime.Now;
			}
		}
		else
		{
			Instance._levelScores.Add(new LevelScore(levelNumber, score));
		}
	}

	// Save score to file
	private void SaveScores()
	{
		using var file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Write);
		if (file != null)
		{
			string jsonStr = JsonConvert.SerializeObject(_levelScores);
			file.StoreString(jsonStr);
		}
	}

	// Load score from file
	private void LoadScores()
	{
		using var file = FileAccess.Open(SCORE_FILE, FileAccess.ModeFlags.Read);
		if (file != null)
		{
			string jsonStr = file.GetAsText();
			if (!string.IsNullOrEmpty(jsonStr))
			{
				_levelScores = JsonConvert.DeserializeObject<List<LevelScore>>(jsonStr);
			}
		}
	}
}
