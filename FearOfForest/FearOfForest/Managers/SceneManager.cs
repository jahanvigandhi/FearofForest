using FearOfForest.Scenes;

namespace FearOfForest.Managers
{
    // SceneManager.cs
    public class SceneManager
    {
        private Scene _currentScene;
        public Scene CurrentScene => _currentScene;

        public SceneManager()
        {
        }

        public void ChangeScene(Scene newScene)
        {
            _currentScene = newScene;
            _currentScene.Initialize(this);
            _currentScene.LoadContent();

            if (_currentScene is GameplayScene)
            {
                AssetManager.PlayBackgroundMusic();
            }
            else
            {
                AssetManager.PauseBackgroundMusic();
            }

        }
    }
}
