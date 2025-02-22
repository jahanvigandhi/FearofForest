using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace FearOfForest.Entities
{
    public class Fly : Enemy
    {
        private float speed = 250f; // Pixels per second

        public Fly(Texture2D texture, Vector2 startPosition)
            : base(texture, startPosition, frameCount: 6, timePerFrame: 0.1f)
        {
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move Fly to the left
            Position -= new Vector2(speed * deltaTime, 0);

            // Optional: Add vertical movement for a flying effect
            Position = new Vector2(Position.X, Position.Y + (float)Math.Sin(Position.X / 50f) * 2f);

            // Update animation
            AnimatedTexture.Update(gameTime);
        }
    }
}
