using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System;

namespace FearOfForest.Managers
{
    public class Leaderboard
    {

        private static Leaderboard instance;
        public static Leaderboard Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Leaderboard();
                }
                return instance;
            }
        }

        private const string LeaderboardFileName = "leaderboard.json";
        private readonly string LeaderboardFilePath;

        public List<Entities.ScoreEntry> Scores { get; private set; }

        private Leaderboard()
        {
            // Set the file path to the application's local directory
            LeaderboardFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, LeaderboardFileName);
            Scores = new List<Entities.ScoreEntry>();
        }

        /// <summary>
        /// Loads the leaderboard from the JSON file. If the file doesn't exist, initializes an empty leaderboard.
        /// </summary>
        public void Load()
        {
            if (File.Exists(LeaderboardFilePath))
            {
                try
                {
                    string json = File.ReadAllText(LeaderboardFilePath);
                    Scores = JsonSerializer.Deserialize<List<Entities.ScoreEntry>>(json) ?? new List<Entities.ScoreEntry>();
                }
                catch (Exception ex)
                {
                    // Handle JSON deserialization errors
                    Console.WriteLine($"Error loading leaderboard: {ex.Message}");
                    Scores = new List<Entities.ScoreEntry>();
                }
            }
            else
            {
                Scores = new List<Entities.ScoreEntry>();
            }
        }

        /// <summary>
        /// Saves the current leaderboard to the JSON file.
        /// </summary>
        public void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(Scores, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(LeaderboardFilePath, json);
            }
            catch (Exception ex)
            {
                // Handle file write errors
                Console.WriteLine($"Error saving leaderboard: {ex.Message}");
            }
        }

        /// <summary>
        /// Adds a new score to the leaderboard and maintains only the top 10 scores.
        /// </summary>
        /// <param name="entry">The new score entry to add.</param>
        public void AddScore(Entities.ScoreEntry entry)
        {
            Scores.Add(entry);
            Scores.Sort();

            if (Scores.Count > 10)
            {
                Scores.RemoveRange(10, Scores.Count - 10);
            }

            Save();
        }

        /// <summary>
        /// Retrieves the top 10 scores.
        /// </summary>
        /// <returns>A list of the top 10 score entries.</returns>
        public List<Entities.ScoreEntry> GetTopScores()
        {
            return new List<Entities.ScoreEntry>(Scores);
        }
    }
}
