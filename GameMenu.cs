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
            _font = new Font("Arial", "arial.ttf"); // Load a font (ensure arial.ttf is in your resources)
            _title = "Dungeon Crawler RPG";

            // Initialize buttons
            _startButton = new Button("Start", 300, 300, 200, 50);
            _quitButton = new Button("Quit", 300, 400, 200, 50);
        }

        public void Show(ref GameState gameState)
        {
            while (!_window.CloseRequested && gameState == GameState.Menu)
            {
                SplashKit.ProcessEvents();

                // Draw menu
                _window.Clear(Color.White);
                DrawTitle();
                _startButton.Draw();
                _quitButton.Draw();
                _window.Refresh();

                // Handle button clicks
                if (_startButton.IsClicked())
                {
                    // Update game state to start the game
                    gameState = GameState.Playing;
                    break;
                }

                if (_quitButton.IsClicked())
                {
                    // Update game state to exit
                    gameState = GameState.Exiting;
                    break;
                }
            }
        }

        private void DrawTitle()
        {
            _window.DrawText(_title, Color.Black, _font, 40, 100, 100);
        }
    }
}
