using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FearOfForest.Entities
{
    public class Platform
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; private set; }

        public Rectangle BoundingBox => new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);

        public Platform(Texture2D texture, Vector2 position, Vector2 size)
        {
            Texture = texture;
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Updates the platform's position based on the scroll speed.
        /// </summary>
        /// <param name="scrollSpeed">Speed at which platforms should scroll.</param>
        /// <param name="deltaTime">Elapsed game time in seconds.</param>
        public void Update(float scrollSpeed, float deltaTime)
        {
            // Move the platform to the left based on scroll speed
            Position -= new Vector2(scrollSpeed * deltaTime, 0);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White);
        }
    }
}
