using Microsoft.Xna.Framework.Input;

namespace FearOfForest.Managers
{
    public static class MouseManager
    {
        private static MouseState _previousMouseState;
        private static MouseState _currentMouseState;

        /// <summary>
        /// Updates the current and previous mouse states.
        /// Call this in Game1.Update or Scene.Update.
        /// </summary>
        public static void Update()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
        }

        /// <summary>
        /// Checks if the left mouse button was clicked.
        /// </summary>
        public static bool IsLeftButtonClicked()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed &&
                   _previousMouseState.LeftButton == ButtonState.Released;
        }

        /// <summary>
        /// Checks if the left mouse button is currently being held down.
        /// </summary>
        public static bool IsLeftButtonDown()
        {
            return _currentMouseState.LeftButton == ButtonState.Pressed;
        }

        /// <summary>
        /// Resets the mouse state. Call this when changing scenes.
        /// </summary>
        public static void Reset()
        {
            _previousMouseState = Mouse.GetState();
            _currentMouseState = Mouse.GetState();
        }
    }
}
