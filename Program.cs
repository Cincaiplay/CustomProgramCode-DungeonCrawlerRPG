using SplashKitSDK;
using System;

namespace CustomProgramCode
{
    public class Program
    {
        public static void Main()
        {
            Window window = new Window("Game", 1280, 960); // Create a window for the game
            GameState gameState = GameState.Menu; // Set initial game state to menu

            GameMenu menu = new GameMenu(window); // Initialize the game menu
            Game game = new Game(window); // Initialize the game
            game.Initialize(); // Load the first level

            Run(window, menu, game, ref gameState); // Start the game loop

            window.Close(); // Close the window when the game ends
        }

        private static void Run(Window window, GameMenu menu, Game game, ref GameState gameState)
        {
            while (gameState != GameState.Exiting)
            {
                SplashKit.ProcessEvents(); // Process events such as key presses
                window.Clear(Color.White); // Clear the window with white color

                switch (gameState)
                {
                    case GameState.Menu:
                        menu.Show(ref gameState); // Show the menu and update game state
                        break;
                    case GameState.Playing:
                        game.HandleInput(); // Handle player input
                        game.Update(); // Update game state
                        game.Draw(); // Draw game elements

                        if (window.CloseRequested) gameState = GameState.Exiting; // Exit if window close requested
                        break;
                }

                window.Refresh(); // Refresh the window display
            }
        }
    }
}
