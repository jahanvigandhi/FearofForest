// BackgroundLayer.cs
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

namespace FearOfForest.Entities
{
    public class BackgroundLayer
    {
        private Texture2D _texture;
        private float _speedFactor;
        private List<Vector2> _positions;
        private float _scaledWidth;
        private float _scaledHeight;

        public BackgroundLayer(Texture2D texture, float speedFactor, GraphicsDevice graphicsDevice)
        {
            _texture = texture;
            _speedFactor = speedFactor;

            // Scale the texture to fit the screen height
            _scaledHeight = graphicsDevice.Viewport.Height;
            _scaledWidth = (_texture.Width / (float)_texture.Height) * _scaledHeight;

            // Calculate how many textures are needed to cover the screen width plus one for seamless scrolling
            int numberOfTextures = (int)Math.Ceiling(graphicsDevice.Viewport.Width / _scaledWidth) + 1;

            _positions = new List<Vector2>();

            for (int i = 0; i < numberOfTextures; i++)
            {
                _positions.Add(new Vector2(i * _scaledWidth, 0));
            }
        }

        /// <summary>
        /// Updates the positions of the background layers based on the scroll speed and delta time.
        /// </summary>
        /// <param name="deltaTime">Elapsed time since last update.</param>
        /// <param name="scrollSpeed">Base scroll speed of the game.</param>
        public void Update(float deltaTime, float scrollSpeed)
        {
            float movement = scrollSpeed * _speedFactor * deltaTime;

            for (int i = 0; i < _positions.Count; i++)
            {
                _positions[i] -= new Vector2(movement, 0);

                // If a texture has moved completely off screen to the left, reposition it to the right
                if (_positions[i].X <= -_scaledWidth)
                {
                    // Find the rightmost texture
                    float rightMostX = float.MinValue;
                    foreach (var pos in _positions)
                    {
                        if (pos.X > rightMostX)
                            rightMostX = pos.X;
                    }

                    var x = rightMostX + _scaledWidth;
                    _positions[i] = new Vector2(x, 0);
                }
            }
        }

        /// <summary>
        /// Draws all background textures to ensure seamless looping.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch instance for drawing textures.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var position in _positions)
            {
                spriteBatch.Draw(_texture, new Rectangle((int)position.X, (int)position.Y, (int)_scaledWidth, (int)_scaledHeight), Color.White);
            }
        }
    }
}
