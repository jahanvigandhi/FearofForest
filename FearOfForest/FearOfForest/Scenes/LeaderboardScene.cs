using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using FearOfForest.Managers;

namespace FearOfForest.Scenes
{
    public class LeaderboardScene : Scene
    {
        private SpriteFont font;
        private List<Entities.ScoreEntry> topScores;
        private Vector2 titlePosition;
        private Vector2 scoreListStartPosition;

        public override void LoadContent()
        {
            font = AssetManager.MenuFont;

            // Load top scores
            Leaderboard.Instance.Load();
            topScores = Leaderboard.Instance.GetTopScores();

            // Positioning
            titlePosition = new Vector2(200, 50); 
            scoreListStartPosition = new Vector2(100, 150);
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            // Go back to the main menu on Enter or Escape
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                SceneManager.ChangeScene(new MainMenuScene());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            spriteBatch.Draw(AssetManager.LeaderboardBackground, Game1.Instance.GraphicsDevice.Viewport.Bounds, Color.White);

            // Draw title
            string title = "Leaderboard";
            Vector2 titleSize = font.MeasureString(title);
            Vector2 titleCenteredPosition = new Vector2(
                (Game1.Instance.GraphicsDevice.Viewport.Width - titleSize.X) / 2,
                titlePosition.Y
            );
            spriteBatch.DrawString(font, title, titleCenteredPosition, Color.White);

            // Draw leaderboard entries
            Vector2 position = scoreListStartPosition;
            int rank = 1;

            foreach (var scoreEntry in topScores)
            {
                string entryText = $"{rank}. Score: {scoreEntry.Score} | Coins: {scoreEntry.CoinsCollected} | Enemies: {scoreEntry.EnemiesDefeated} | Time: {scoreEntry.TimeTaken:mm\\:ss}";
                spriteBatch.DrawString(font, entryText, position, Color.White);
                position.Y += font.LineSpacing + 5; // Add spacing between entries
                rank++;
            }

            // Draw footer text
            string footer = "Press Enter or Escape to return to the main menu.";
            Vector2 footerSize = font.MeasureString(footer);
            Vector2 footerPosition = new Vector2(
                (Game1.Instance.GraphicsDevice.Viewport.Width - footerSize.X) / 2,
                Game1.Instance.GraphicsDevice.Viewport.Height - footerSize.Y - 20
            );
            spriteBatch.DrawString(font, footer, footerPosition, Color.White);
        }
    }
}
