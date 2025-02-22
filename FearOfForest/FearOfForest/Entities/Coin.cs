using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FearOfForest.Entities
{
    public class Coin
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; private set; }
        public bool IsCollected { get; set; }

        float scaleX;
        float scaleY;


        public Rectangle BoundingBox
        {
            get
            {
                // Calculate the scaled size of the coin
                int width = (int)(Texture.Width * scaleX);
                int height = (int)(Texture.Height * scaleY);
                // Calculate the scaled position of the coin
                int x = (int)(Position.X - (Texture.Width / 2) * scaleX);
                int y = (int)(Position.Y - (Texture.Height / 2) * scaleY);
                return new Rectangle(x, y, width, height);
            }
        }


        // Movement parameters
        private Vector2 movementDirection;
        private float movementSpeed = 100f; // Pixels per second

        // Enum to manage scaling states
        private enum ScaleState
        {
            Normal,
            Flipped
        }

        public Coin(Texture2D texture, Vector2 startPosition)
        {
            Texture = texture;
            Position = startPosition;
            IsCollected = false;

            // Initialize movement direction towards the player (assuming player's position is accessible)
            // For simplicity, set movement direction to left (negative X)
            movementDirection = new Vector2(-1, 0);
        }

        /// <summary>
        /// Updates the coin's rotation, scaling, and movement to create a spinning and approaching effect.
        /// </summary>
        /// <param name="gameTime">GameTime instance.</param>
        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update position to move towards the player
            Position += movementDirection * movementSpeed * deltaTime;
        }

        /// <summary>
        /// Draws the coin with rotation, scaling, and flipping applied.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsCollected)
            {
                // Calculate origin for rotation
                Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

                // Calculate the scale factor to make the coin 50x50
                scaleX = 50f / Texture.Width;
                scaleY = 50f / Texture.Height;

                // Draw the coin with rotation, scaling, and flipping
                spriteBatch.Draw(Texture, Position, null, Color.White, 0f, origin, new Vector2(scaleX, scaleY), SpriteEffects.None, 0);
            }
        }

    }
}
