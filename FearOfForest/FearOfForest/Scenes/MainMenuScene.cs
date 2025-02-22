using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using FearOfForest.Managers;

namespace FearOfForest.Scenes
{
    public class MainMenuScene : Scene
    {
        private List<string> menuItems = new List<string> { "Start Game", "Leaderboard", "Info", "Credit", "Quit" };
        private int selectedItem = 0;
        private Vector2 startPosition;
        private Texture2D background;
        private Texture2D buttonTexture;
        private SpriteFont font;

        private readonly int buttonWidth = 300;
        private readonly int buttonHeight = 50;
        private readonly int buttonPadding = 10;

        public override void LoadContent()
        {
            background = AssetManager.MainMenuBackground;
            buttonTexture = AssetManager.Button;
            font = AssetManager.MenuFont;

            // Calculate starting position to center buttons vertically
            float totalMenuHeight = menuItems.Count * (buttonHeight + buttonPadding) - buttonPadding;
            float screenCenterY = Game1.Instance.GraphicsDevice.Viewport.Height / 2f;
            float startY = screenCenterY - (totalMenuHeight / 2f);

            startPosition = new Vector2(
                (Game1.Instance.GraphicsDevice.Viewport.Width - buttonWidth) / 2f,
                startY
            );
        }

        public override void Update(GameTime gameTime)
        {

            // Keyboard Navigation
            if (KeyboardManager.IsKeyPressed(Keys.Down))
            {
                selectedItem = (selectedItem + 1) % menuItems.Count;
            }
            if (KeyboardManager.IsKeyPressed(Keys.Up))
            {
                selectedItem = (selectedItem - 1 + menuItems.Count) % menuItems.Count;
            }
            if (KeyboardManager.IsKeyPressed(Keys.Enter))
            {
                SelectMenuItem(selectedItem);
            }

            // Mouse Click Navigation
            foreach (var (item, index) in menuItems.Select((value, index) => (value, index)))
            {
                Vector2 position = startPosition + new Vector2(0, index * (buttonHeight + buttonPadding));
                Rectangle buttonRect = new Rectangle((int)position.X, (int)position.Y, buttonWidth, buttonHeight);
                if (MouseManager.IsLeftButtonClicked() && buttonRect.Contains(Mouse.GetState().Position))
                {
                    SelectMenuItem(index);
                }
            }
        }

        private void SelectMenuItem(int index)
        {
            // Reset input states before switching scenes
            MouseManager.Reset();
            KeyboardManager.Reset();

            switch (index)
            {
                case 0:
                    SceneManager.ChangeScene(new GameplayScene());
                    break;
                case 1:
                    SceneManager.ChangeScene(new LeaderboardScene());
                    break;
                case 2:
                    SceneManager.ChangeScene(new InfoScene());
                    break;
                case 3:
                    SceneManager.ChangeScene(new CreditScene());
                    break;
                case 4:
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw background
            spriteBatch.Draw(background, Game1.Instance.GraphicsDevice.Viewport.Bounds, Color.White);

            // Draw buttons
            for (int i = 0; i < menuItems.Count; i++)
            {
                Vector2 position = startPosition + new Vector2(0, i * (buttonHeight + buttonPadding));
                bool isSelected = i == selectedItem;

                DrawButton(spriteBatch, position, menuItems[i], isSelected);
            }
        }

        private void DrawButton(SpriteBatch spriteBatch, Vector2 position, string text, bool isSelected)
        {
            // Draw button background
            Rectangle buttonRect = new Rectangle((int)position.X, (int)position.Y, buttonWidth, buttonHeight);
            Color buttonColor = isSelected ? Color.Gold : Color.White;
            spriteBatch.Draw(buttonTexture, buttonRect, buttonColor);

            // Draw button text centered
            Vector2 textSize = font.MeasureString(text);
            Vector2 textPosition = new Vector2(
                position.X + (buttonWidth - textSize.X) / 2f,
                position.Y + (buttonHeight - textSize.Y) / 2f
            );
            spriteBatch.DrawString(font, text, textPosition, Color.Black);
        }
    }
}
