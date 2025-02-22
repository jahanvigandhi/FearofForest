using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FearOfForest.Entities
{
    public class Snail : Enemy
    {
        private float speed = 160f;

        public Snail(Texture2D texture, Vector2 startPosition)
            : base(texture, startPosition, frameCount: 8, timePerFrame: 0.1f)
        {
        }

        public override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Move snail to the left
            Position -= new Vector2(speed * deltaTime, 0);

            // Update animation (if any)
            AnimatedTexture.Update(gameTime);
        }
    }
}
