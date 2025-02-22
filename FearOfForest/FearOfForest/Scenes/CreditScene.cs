using FearOfForest.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FearOfForest.Scenes
{
    // CreditScene.cs
    public class CreditScene : Scene
    {
        private Texture2D background;

        public override void Initialize(SceneManager sceneManager)
        {
            base.Initialize(sceneManager);

            LoadContent();
        }

        public override void LoadContent()
        {
            background = AssetManager.CreditBackground;
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
            // You can add additional credit text here if desired
        }
    }
}
