// GameplayScene.cs
using FearOfForest.Entities;
using FearOfForest.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace FearOfForest.Scenes
{
    public class GameplayScene : Scene
    {
        private Player player;
        private Level currentLevel;
        private int currentLevelNumber = 1;
        private ScoreManager scoreManager;

        private BackgroundLayer backgroundLayer1; // Front layer
        private BackgroundLayer backgroundLayer2; // Middle layer
        private BackgroundLayer backgroundLayer3; // Back layer

        private float scrollSpeed = 200f; // Pixels per second

        private Texture2D borderTexture; // Bounding box texture

        private SpriteFont healthFont;

        private bool isDebug = false;

        private bool isGameOver = false;

        private SoundEffectInstance smashSound;

        public override void LoadContent()
        {
            // Initialize background layers with appropriate speed factors and GraphicsDevice
            backgroundLayer1 = new BackgroundLayer(AssetManager.BackgroundLayer1, speedFactor: 0.1f, graphicsDevice: Game1.Instance.GraphicsDevice);
            backgroundLayer2 = new BackgroundLayer(AssetManager.BackgroundLayer2, speedFactor: 0.3f, graphicsDevice: Game1.Instance.GraphicsDevice);
            backgroundLayer3 = new BackgroundLayer(AssetManager.BackgroundLayer3, speedFactor: 0.6f, graphicsDevice: Game1.Instance.GraphicsDevice);

            // Initialize player with all necessary animations
            player = new Player(
                runTexture: AssetManager.PlayerRunTexture,
                jumpTexture: AssetManager.PlayerJumpTexture,
                attackTexture: AssetManager.PlayerAttackTexture,
                deadTexture: AssetManager.PlayerDeadTexture,
                startPosition: new Vector2(100, 400),
                runFrameCount: 8,
                jumpFrameCount: 15,
                attackFrameCount: 8,
                deadFrameCount: 8
            );

            scoreManager = new ScoreManager();
            currentLevel = new Level();
            currentLevel.LoadLevel(currentLevelNumber);

            // Initialize borderTexture as a 1x1 white pixel
            borderTexture = new Texture2D(Game1.Instance.GraphicsDevice, 1, 1);
            borderTexture.SetData(new[] { Color.White });

            // Load health font
            healthFont = AssetManager.GameFont;

            // Load smash sound
            smashSound = AssetManager.SmashSound.CreateInstance();

            player.OnPlayerDeath += Player_OnPlayerDeath;
        }

        private void Player_OnPlayerDeath()
        {
            isGameOver = true;

            AssetManager.StopBackgroundMusic();


            var entry = new Entities.ScoreEntry
            {
                Score = scoreManager.TotalScore,
                CoinsCollected = scoreManager.CoinsCollected,
                EnemiesDefeated = scoreManager.EnemiesDefeated,
                TimeTaken = scoreManager.TimeTaken
            };
            Leaderboard.Instance.AddScore(entry);

            SceneManager.ChangeScene(new LeaderboardScene());
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            player.Update(gameTime, keyboardState, currentLevel.Platforms);


            if (player.IsPlayerDead)
            {
                HandleGameOverInput();
                return;
            }
            else
            {
                HandleGamePlayInput(keyboardState);
            }

            // Update background layers based on scroll speed and delta time
            var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            backgroundLayer1.Update(deltaTime, scrollSpeed);
            backgroundLayer2.Update(deltaTime, scrollSpeed);
            backgroundLayer3.Update(deltaTime, scrollSpeed);

            // Update platforms, enemies, and coins within the level
            currentLevel.Update(gameTime, scrollSpeed);

            // Update enemies and handle collisions
            foreach (var enemy in currentLevel.Enemies)
            {
                // Collision detection between player and enemies
                if (player.BoundingBox.Intersects(enemy.BoundingBox))
                {
                    if (player.CurrentState == PlayerState.Attack)
                    {
                        if (enemy.IsKillable)
                        {
                            // Kill the enemy
                            enemy.IsDead = true;

                            // Award double points for killing Snail or Fly
                            if (enemy is Snail || enemy is Fly)
                            {
                                scoreManager.DefeatEnemy();

                            }

                            AssetManager.KillSound.Play();
                        }

                    }
                    else
                    {
                        // Player takes damage
                        player.TakeDamage(1);

                        // Play smash sound
                        smashSound.Play();
                    }
                }
            }

            // Remove dead enemies after iteration to prevent modifying the collection during iteration
            currentLevel.Enemies.RemoveAll(e => e.IsDead);

            // Update collectibles (coins)
            foreach (var coin in currentLevel.Coins)
            {
                if (!coin.IsCollected && player.BoundingBox.Intersects(coin.BoundingBox))
                {
                    coin.IsCollected = true;
                    scoreManager.CollectCoin();
                    AssetManager.CoinSound.Play();
                }
            }

            // Level completion condition (if applicable)
            // For endless runner, you might not have a level end, but keeping it as per existing logic
            if (player.Position.X > currentLevel.LevelEndPosition)
            {
                scoreManager.CompleteLevel();
                currentLevelNumber++;
                if (currentLevelNumber > 4)
                {
                    // Handle game completion (e.g., show leaderboard or end game)
                    SceneManager.ChangeScene(new LeaderboardScene());
                }
                else
                {
                    currentLevel.LoadLevel(currentLevelNumber);
                    player.Position = new Vector2(100, 400); // Reset player position
                }
            }

            // Update score based on time
            scoreManager.UpdateTime(gameTime.ElapsedGameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw Background Layers in order from back to front
            backgroundLayer1.Draw(spriteBatch);
            backgroundLayer2.Draw(spriteBatch);
            backgroundLayer3.Draw(spriteBatch);

            // Draw level elements
            currentLevel.Draw(spriteBatch);

            if (isGameOver)
            {
                DrawGameOverScreen(spriteBatch);
                return;
            }

            player.Draw(spriteBatch);

            // Draw enemies
            foreach (var enemy in currentLevel.Enemies)
            {
                enemy.Draw(spriteBatch);
            }

            // Draw collectibles
            foreach (var coin in currentLevel.Coins)
            {
                coin.Draw(spriteBatch);
            }

            // Draw Score
            spriteBatch.DrawString(AssetManager.GameFont, $"Score: {scoreManager.TotalScore}", new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(healthFont, $"Health: {player.CurrentHealth}/{player.MaxHealth}", new Vector2(10, 40), Color.White);

            if (isDebug)
            {
                DrawBoundingBoxes(spriteBatch);
            }
        }

        private void HandleGamePlayInput(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                SceneManager.ChangeScene(new MainMenuScene());
            }
            if (keyboardState.IsKeyDown(Keys.F1))
            {
                isDebug = !isDebug;
            }
        }

        private void HandleGameOverInput()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                isGameOver = false;
                ResetLevel();
            }
            else if (keyboardState.IsKeyDown(Keys.Escape))
            {
                SceneManager.ChangeScene(new MainMenuScene());
            }
        }

        private void DrawGameOverScreen(SpriteBatch spriteBatch)
        {
            // Draw semi-transparent overlay
            spriteBatch.Draw(
                borderTexture,
                new Rectangle(0, 0, Game1.Instance.GraphicsDevice.Viewport.Width, Game1.Instance.GraphicsDevice.Viewport.Height),
                Color.Black * 0.5f
            );

            // Draw "Game Over" text
            string gameOverText = "Game Over";
            Vector2 gameOverSize = healthFont.MeasureString(gameOverText);
            Vector2 gameOverPosition = new Vector2(
                (Game1.Instance.GraphicsDevice.Viewport.Width - gameOverSize.X) / 2,
                200
            );
            spriteBatch.DrawString(healthFont, gameOverText, gameOverPosition, Color.White);

            // Draw final score
            string finalScoreText = $"Final Score: {scoreManager.TotalScore}";
            Vector2 finalScoreSize = healthFont.MeasureString(finalScoreText);
            Vector2 finalScorePosition = new Vector2(
                (Game1.Instance.GraphicsDevice.Viewport.Width - finalScoreSize.X) / 2,
                250
            );
            spriteBatch.DrawString(healthFont, finalScoreText, finalScorePosition, Color.White);

            // Draw restart instructions
            string restartText = "Press Enter to Restart";
            Vector2 restartSize = healthFont.MeasureString(restartText);
            Vector2 restartPosition = new Vector2(
                (Game1.Instance.GraphicsDevice.Viewport.Width - restartSize.X) / 2,
                300
            );
            spriteBatch.DrawString(healthFont, restartText, restartPosition, Color.White);

            // Draw main menu instructions
            string menuText = "Press Esc to Return to Main Menu";
            Vector2 menuSize = healthFont.MeasureString(menuText);
            Vector2 menuPosition = new Vector2(
                (Game1.Instance.GraphicsDevice.Viewport.Width - menuSize.X) / 2,
                350
            );
            spriteBatch.DrawString(healthFont, menuText, menuPosition, Color.White);
        }

        /// <summary>
        /// Resets the current level and player to initial state.
        /// </summary>
        private void ResetLevel()
        {
            // Reset player's health and state
            player = new Player(
                runTexture: AssetManager.PlayerRunTexture,
                jumpTexture: AssetManager.PlayerJumpTexture,
                attackTexture: AssetManager.PlayerAttackTexture,
                deadTexture: AssetManager.PlayerDeadTexture,
                startPosition: new Vector2(100, 400),
                runFrameCount: 8,
                jumpFrameCount: 15,
                attackFrameCount: 8,
                deadFrameCount: 8
            );

            // Reset score
            scoreManager.Reset();

            // Reload the current level
            currentLevel.LoadLevel(currentLevelNumber);

            AssetManager.StopBackgroundMusic();
            AssetManager.PlayBackgroundMusic();
        }

        /// <summary>
        /// Helper method to draw bounding boxes around the player and enemies.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance.</param>
        private void DrawBoundingBoxes(SpriteBatch spriteBatch)
        {
            // Draw Bounding Box Around Player
            DrawBoundingBox(spriteBatch, player.BoundingBox, Color.Red);

            // Draw Bounding Boxes Around Enemies
            foreach (var enemy in currentLevel.Enemies)
            {
                DrawBoundingBox(spriteBatch, enemy.BoundingBox, Color.Green);
            }

            // Draw Bounding Boxes Around Coins
            foreach (var coin in currentLevel.Coins)
            {
                if (!coin.IsCollected)
                {
                    DrawBoundingBox(spriteBatch, coin.BoundingBox, Color.Yellow);
                }
            }
        }

        /// <summary>
        /// Helper method to draw a bounding box around a rectangle.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance.</param>
        /// <param name="rectangle">Rectangle to draw around.</param>
        /// <param name="color">Color of the bounding box.</param>
        private void DrawBoundingBox(SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            // Top
            spriteBatch.Draw(borderTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width, 1), color);
            // Bottom
            spriteBatch.Draw(borderTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height - 1, rectangle.Width, 1), color);
            // Left
            spriteBatch.Draw(borderTexture, new Rectangle(rectangle.X, rectangle.Y, 1, rectangle.Height), color);
            // Right
            spriteBatch.Draw(borderTexture, new Rectangle(rectangle.X + rectangle.Width - 1, rectangle.Y, 1, rectangle.Height), color);
        }

    }
}
