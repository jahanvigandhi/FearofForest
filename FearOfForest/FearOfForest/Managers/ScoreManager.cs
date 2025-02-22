using System;

namespace FearOfForest.Managers
{
    public class ScoreManager
    {

        public int CoinsCollected { get; private set; }
        public int EnemiesDefeated { get; private set; }
        public TimeSpan TimeTaken { get; private set; }
        public int LevelCompletionBonus { get; private set; }

        public int TotalScore
        {
            get
            {
                return Math.Max(0, CoinsCollected * 10 + EnemiesDefeated * 50 + LevelCompletionBonus - (int)TimeTaken.TotalSeconds);
            }
        }

        public ScoreManager()
        {
            CoinsCollected = 0;
            EnemiesDefeated = 0;
            TimeTaken = TimeSpan.Zero;
            LevelCompletionBonus = 0;
        }

        
        public void CollectCoin()
        {
            CoinsCollected++;
        }

        public void DefeatEnemy()
        {
            EnemiesDefeated++;
        }

        public void UpdateTime(TimeSpan deltaTime)
        {
            TimeTaken += deltaTime;
        }

        public void CompleteLevel()
        {
            LevelCompletionBonus += 100;
            AssetManager.LevelUpSound.Play();
        }

        public void Reset()
        {
            CoinsCollected = 0;
            EnemiesDefeated = 0;
            TimeTaken = TimeSpan.Zero;
            LevelCompletionBonus = 0;
        }
    }
}
