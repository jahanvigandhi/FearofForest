using FearOfForest.Managers;
using FearOfForest.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FearOfForest
{
    public class Game1 : Game
    {
        private static Game1 _instance;
        public static Game1 Instance => _instance;


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SceneManager sceneManager;

        public Game1()
        {
            _instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            sceneManager = new SceneManager();
            sceneManager.ChangeScene(new MainMenuScene());
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            AssetManager.LoadAssets(Content);
            sceneManager.CurrentScene.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            MouseManager.Update();
            KeyboardManager.Update();
            sceneManager.CurrentScene.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            sceneManager.CurrentScene.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
