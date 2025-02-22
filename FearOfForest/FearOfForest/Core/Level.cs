using FearOfForest.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace FearOfForest.Entities
{
    public class Level
    {
        public List<Platform> Platforms { get; private set; }
        public List<Enemy> Enemies { get; private set; }
        public List<Coin> Coins { get; private set; }
        public float LevelEndPosition { get; private set; }

        private Random random;

        public Level()
        {
            Platforms = new List<Platform>();
            Enemies = new List<Enemy>();
            Coins = new List<Coin>();
            random = new Random();
        }

        public void LoadLevel(int levelNumber)
        {
            // Clear existing objects
            Platforms.Clear();
            Enemies.Clear();
            Coins.Clear();

            // Define ground Y position (adjust based on your game's layout)
            float groundY = 460;

            // Get screen width from the GraphicsDevice via the Game1 singleton
            float screenWidth = Game1.Instance.GraphicsDevice.Viewport.Width;

            // Tile dimensions
            int tileWidth = AssetManager.Tileset1.Width;
            int tileHeight = AssetManager.Tileset1.Height;

            // Calculate the number of tiles needed to cover the screen plus a buffer
            int numberOfTiles = (int)Math.Ceiling(screenWidth / (float)tileWidth) + 2; // +2 for seamless scrolling

            for (int i = 0; i < numberOfTiles; i++)
            {
                // Select a tileset based on the specified probabilities
                Texture2D selectedTileset = SelectTileset();

                // Calculate the position for each tile
                Vector2 position = new Vector2(i * tileWidth, groundY);

                // Create and add the platform
                Platform platform = new Platform(selectedTileset, position, new Vector2(tileWidth, tileHeight));
                Platforms.Add(platform);

                // Optionally, spawn enemies and coins on the platform
                SpawnEnemiesAndCoins(platform, i);
            }

            // Set the end position based on the number of tiles
            LevelEndPosition = numberOfTiles * tileWidth;
        }

        /// <summary>
        /// Selects a tileset based on predefined probabilities:
        /// 95% Tileset1, 4% Tileset2, 1% Tileset3
        /// </summary>
        /// <returns>Selected Texture2D tileset</returns>
        private Texture2D SelectTileset()
        {
            int rand = random.Next(100); // Generates a number between 0 and 99

            if (rand < 95)
                return AssetManager.Tileset1; // 95%
            else if (rand < 99)
                return AssetManager.Tileset2; // 4%
            else
                return AssetManager.Tileset3; // 1%
        }

        /// <summary>
        /// Spawns enemies and coins on the given platform based on random chance.
        /// </summary>
        /// <param name="platform">The platform to spawn on.</param>
        /// <param name="tileIndex">Index of the tile in the level.</param>
        private void SpawnEnemiesAndCoins(Platform platform, int tileIndex)
        {
            // Define spawn probabilities
            float enemySpawnChance = 0.1f; // 10% chance to spawn an enemy on a platform
            float coinSpawnChance = 0.3f;  // 30% chance to spawn a coin on a platform

            // Spawn Enemy
            if (random.NextDouble() < enemySpawnChance)
            {
                Enemy enemy = GenerateRandomEnemy(platform.Position, platform.Size);
                if (enemy != null)
                {
                    Enemies.Add(enemy);
                }
            }

            // Spawn Coin
            if (random.NextDouble() < coinSpawnChance)
            {
                // Ensure the coin spawns within the player's reachable vertical range
                // Assuming the player can jump up to 150 pixels above the platform
                float maxJumpHeight = 150f;



                Vector2 coinPosition = new Vector2(
                    platform.Position.X + random.Next(20, (int)(platform.Size.X - 20)), // Slight padding from platform edges
                    platform.Position.Y - AssetManager.PlayerRunTexture.Height / 2 - AssetManager.CoinTexture.Height / 2 - random.Next(0, (int)(maxJumpHeight / 2))
                );

                Coin coin = new Coin(AssetManager.CoinTexture, coinPosition);
                Coins.Add(coin);
            }
        }

        /// <summary>
        /// Generates a random enemy type and returns an instance positioned on the given platform.
        /// </summary>
        /// <param name="platformPosition">Position of the platform.</param>
        /// <param name="platformSize">Size of the platform.</param>
        /// <returns>An instance of Enemy or null.</returns>
        private Enemy GenerateRandomEnemy(Vector2 platformPosition, Vector2 platformSize)
        {
            // Define enemy types and their probabilities
            // Boar: 50%, Fly: 30%, Snail: 20%
            double enemyRand = random.NextDouble();

            Enemy enemy = null;

            if (enemyRand < 0.5)
            {
                // Spawn Boar
                int maxX = (int)(platformSize.X - AssetManager.BoarTexture.Width);
                if (maxX < 0) maxX = 0;

                Vector2 enemyPosition = new Vector2(
                    platformPosition.X + random.Next(0, maxX),
                    platformPosition.Y - AssetManager.BoarTexture.Height
                );
                enemy = new Boar(AssetManager.BoarTexture, enemyPosition);
            }
            else if (enemyRand < 0.8)
            {
                int maxX = (int)(platformSize.X - AssetManager.FlyTexture.Width);
                if (maxX < 0) maxX = 0;

                // Spawn Fly
                Vector2 enemyPosition = new Vector2(
                    platformPosition.X + random.Next(0, maxX),
                    platformPosition.Y - AssetManager.FlyTexture.Height - 20 // Slightly higher than platform
                );
                enemy = new Fly(AssetManager.FlyTexture, enemyPosition);
            }
            else
            {
                // Spawn Snail
                int maxX = (int)(platformSize.X - AssetManager.SnailTexture.Width);
                if (maxX < 0) maxX = 0;
                Vector2 enemyPosition = new Vector2(
                    platformPosition.X + random.Next(0, maxX),
                    platformPosition.Y - AssetManager.SnailTexture.Height
                );
                enemy = new Snail(AssetManager.SnailTexture, enemyPosition);
            }

            return enemy;
        }

        /// <summary>
        /// Updates the platforms, enemies, and coins within the level.
        /// </summary>
        /// <param name="gameTime">GameTime instance.</param>
        /// <param name="scrollSpeed">Speed at which platforms should scroll.</param>
        public void Update(GameTime gameTime, float scrollSpeed)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move each platform to the left based on scroll speed
            foreach (var platform in Platforms)
            {
                platform.Update(deltaTime, scrollSpeed);
            }

            // Remove platforms that have moved off-screen to the left
            Platforms.RemoveAll(p => p.Position.X + p.Size.X < 0);

            // Update enemies
            foreach (var enemy in Enemies)
            {
                enemy.Update(gameTime);
            }

            // Remove enemies that have moved off-screen
            Enemies.RemoveAll(e => e.Position.X + e.AnimatedTexture.FrameWidth < 0);

            // Update coins
            foreach (var coin in Coins)
            {
                coin.Update(gameTime);
            }

            // Remove coins that have been collected or moved off-screen
            Coins.RemoveAll(c => c.IsCollected || c.Position.X + (c.Texture.Width) < 0);

            // Add new platforms, enemies, and coins as needed
            // Determine the last platform's X position
            if (Platforms.Count > 0)
            {
                float lastPlatformX = Platforms[Platforms.Count - 1].Position.X + Platforms[Platforms.Count - 1].Size.X;
                float screenWidth = Game1.Instance.GraphicsDevice.Viewport.Width;
                int tileWidth = (int)Platforms[Platforms.Count - 1].Size.X;

                while (lastPlatformX < screenWidth + tileWidth)
                {
                    // Select a tileset based on the specified probabilities
                    Texture2D selectedTileset = SelectTileset();

                    // Calculate the position for the new platform
                    Vector2 position = new Vector2(lastPlatformX, Platforms[Platforms.Count - 1].Position.Y);

                    // Create and add the new platform
                    Platform newPlatform = new Platform(selectedTileset, position, new Vector2(tileWidth, Platforms[Platforms.Count - 1].Size.Y));
                    Platforms.Add(newPlatform);

                    // Optionally, spawn enemies and coins on the new platform
                    SpawnEnemiesAndCoins(newPlatform, Platforms.Count - 1);

                    // Update the lastPlatformX for the next iteration
                    lastPlatformX += tileWidth;
                }
            }
            else
            {
                // If no platforms exist, load the initial platforms
                LoadLevel(1);
            }
        }

        /// <summary>
        /// Draws all platforms, enemies, and coins.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance for drawing textures.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw platforms
            foreach (var platform in Platforms)
            {
                platform.Draw(spriteBatch);
            }

            // Draw enemies
            foreach (var enemy in Enemies)
            {
                enemy.Draw(spriteBatch);
            }

            // Draw coins
            foreach (var coin in Coins)
            {
                coin.Draw(spriteBatch);
            }
        }
    }
}
