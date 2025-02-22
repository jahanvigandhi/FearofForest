using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FearOfForest.Entities
{
    public abstract class Enemy
    {
        public Vector2 Position { get; set; }
        public AnimatedSprite AnimatedTexture { get; private set; }
        public bool IsDead { get; set; }

        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    AnimatedTexture.FrameWidth,
                    AnimatedTexture.FrameHeight
                );
            }
        }

        // Indicates whether the enemy can be killed via player's attack
        public virtual bool IsKillable => true;

        protected Enemy(Texture2D texture, Vector2 startPosition, int frameCount, float timePerFrame)
        {
            Position = startPosition;
            AnimatedTexture = new AnimatedSprite(texture, frameCount, timePerFrame, isLooping: true);
            IsDead = false;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsDead)
                AnimatedTexture.Draw(spriteBatch, Position, Color.White);
        }
    }
}
