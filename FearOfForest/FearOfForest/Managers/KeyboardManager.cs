using Microsoft.Xna.Framework.Input;

namespace FearOfForest.Managers
{
    public static class KeyboardManager
    {
        private static KeyboardState _currentKeyboardState;
        private static KeyboardState _previousKeyboardState;

        /// <summary>
        /// Updates the current and previous keyboard states.
        /// Call this in Game1.Update or Scene.Update.
        /// </summary>
        public static void Update()
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Checks if a key was just pressed (down in the current frame but not in the previous frame).
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && !_previousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a key is currently being held down.
        /// </summary>
        public static bool IsKeyDown(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Checks if a key was just released (up in the current frame but down in the previous frame).
        /// </summary>
        public static bool IsKeyReleased(Keys key)
        {
            return !_currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Resets the keyboard state. Call this when switching scenes.
        /// </summary>
        public static void Reset()
        {
            _currentKeyboardState = Keyboard.GetState();
            _previousKeyboardState = Keyboard.GetState();
        }
    }
}
