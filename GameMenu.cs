using SplashKitSDK;
using System;

namespace CustomProgramCode
{
    public class GameMenu
    {
        private Window _window;
        private Font _font;
        private string _title;
        private Button _startButton;
        private Button _quitButton;

        public GameMenu(Window window)
        {
            _window = window;
            _font = new Font("Arial", "arial.ttf"); // Load the font
            _title = "Dungeon Crawler RPG"; // Set the title

            // Initialize buttons
            _startButton = new Button("Start", 300, 300, 200, 50);
            _quitButton = new Button("Quit", 300, 400, 200, 50);
        }

        public void Show(ref GameState gameState)
        {
            while (!_window.CloseRequested && gameState == GameState.Menu)
            {
                SplashKit.ProcessEvents(); // Process events such as key presses

                // Draw menu
                _window.Clear(Color.White); // Clear window with white color
                DrawTitle(); // Draw the title
                _startButton.Draw(); // Draw the start button
                _quitButton.Draw(); // Draw the quit button
                _window.Refresh(); // Refresh the window display

                // Handle button clicks
                if (_startButton.IsClicked())
                {
                    gameState = GameState.Playing; // Start the game
                    break;
                }

                if (_quitButton.IsClicked())
                {
                    gameState = GameState.Exiting; // Exit the game
                    break;
                }
            }
        }

        private void DrawTitle()
        {
            _window.DrawText(_title, Color.Black, _font, 40, 100, 100); // Draw the title text
        }
    }
}
