using FearOfForest.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FearOfForest.Scenes
{
    // InfoScene.cs
    public class InfoScene : Scene
    {
        private Texture2D background;

        public override void LoadContent()
        {
            background = AssetManager.InfoBackground;
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                SceneManager.ChangeScene(new MainMenuScene());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            // You can add additional info text here if desired
        }
    }
}
