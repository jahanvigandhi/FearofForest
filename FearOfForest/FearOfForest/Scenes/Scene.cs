using FearOfForest.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FearOfForest.Scenes
{
    // Scene.cs
    public abstract class Scene
    {

        protected SceneManager SceneManager;

        public virtual void Initialize(SceneManager sceneManager)
        {
            SceneManager = sceneManager;
        }

        public abstract void LoadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
