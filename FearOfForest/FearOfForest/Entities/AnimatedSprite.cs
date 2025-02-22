// AnimatedSprite.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FearOfForest.Entities
{
    public class AnimatedSprite
    {
        private Texture2D _texture;
        private int _frameCount;
        private float _timePerFrame;
        private float _timer;
        private int _currentFrame;
        private bool _isLooping;

        public int FrameWidth => _texture.Width / _frameCount;
        public int FrameHeight => _texture.Height;

        public AnimatedSprite(Texture2D texture, int frameCount, float timePerFrame, bool isLooping = true)
        {
            _texture = texture;
            _frameCount = frameCount;
            _timePerFrame = timePerFrame;
            _isLooping = isLooping;
            _currentFrame = 0;
            _timer = 0f;
        }

        /// <summary>
        /// Updates the animation based on the elapsed game time.
        /// </summary>
        /// <param name="gameTime">GameTime instance.</param>
        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= _timePerFrame)
            {
                _currentFrame++;
                _timer = 0f;

                if (_currentFrame >= _frameCount)
                {
                    if (_isLooping)
                        _currentFrame = 0;
                    else
                        _currentFrame = _frameCount - 1; // Stay on the last frame
                }
            }
        }

        /// <summary>
        /// Resets the animation to the first frame.
        /// </summary>
        public void Reset()
        {
            _currentFrame = 0;
            _timer = 0f;
        }

        /// <summary>
        /// Draws the current frame of the animation.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance.</param>
        /// <param name="position">Position to draw the sprite.</param>
        /// <param name="color">Color tint.</param>
        public void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            Rectangle sourceRectangle = new Rectangle(_currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            spriteBatch.Draw(_texture, position, sourceRectangle, color);
        }

        /// <summary>
        /// Determines if the animation has reached its last frame.
        /// Useful for non-looping animations.
        /// </summary>
        public bool IsAnimationFinished
        {
            get
            {
                return !_isLooping && _currentFrame == _frameCount - 1;
            }
        }

        /// <summary>
        /// Sets whether the animation should loop.
        /// </summary>
        /// <param name="isLooping">True to loop, false otherwise.</param>
        public void SetLooping(bool isLooping)
        {
            _isLooping = isLooping;
        }
    }
}
