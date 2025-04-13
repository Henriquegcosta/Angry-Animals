using System;

public class LevelScore
{
    public int LevelNumber { get; set; }
    public int BestScore { get; set; }
    public DateTime DateSet { get; set; }

    public LevelScore(int levelNumber, int bestScore)
    {
        DateSet = DateTime.Now;
        LevelNumber = levelNumber;
        BestScore = bestScore;
    }
}
