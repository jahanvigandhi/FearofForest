using FearOfForest.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FearOfForest
{
    public class Boar : Enemy
    {
        private float speed = 200f;

        public Boar(Texture2D texture, Vector2 startPosition)
            : base(texture, startPosition, frameCount: 6, timePerFrame: 0.1f)
        {
        }

        public override bool IsKillable => false;

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move boar to the left
            Position -= new Vector2(speed * deltaTime, 0);

            // Update animation
            AnimatedTexture.Update(gameTime);
        }
    }
}
