using System;

namespace FearOfForest.Entities
{
    public class ScoreEntry : IComparable<ScoreEntry>
    {
        public int Score { get; set; }

        public int CoinsCollected { get; set; }
        public int EnemiesDefeated { get; set; }
        public TimeSpan TimeTaken { get; set; }


        public DateTime DateAchieved { get; set; }

        public ScoreEntry() { }

        public ScoreEntry(int score, int coinsCollected, int enemiesDefeated, TimeSpan timeTaken)
        {
            Score = score;
            CoinsCollected = coinsCollected;
            EnemiesDefeated = enemiesDefeated;
            TimeTaken = timeTaken;
            DateAchieved = DateTime.Now;
        }

        // For sorting in descending order
        public int CompareTo(ScoreEntry other)
        {
            return other.Score.CompareTo(this.Score);
        }
    }
}
