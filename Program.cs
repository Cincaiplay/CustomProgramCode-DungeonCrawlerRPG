using SplashKitSDK;
using System;

namespace CustomProgramCode
{
    public class Program
    {
        public static void Main()
        {
            Window window = new Window("Game", 1280, 960);
            GameState gameState = GameState.Menu;

            GameMenu menu = new GameMenu(window);
            Game game = new Game(window);
            game.Initialize();

            Run(window, menu, game, ref gameState);

            window.Close();
        }

        private static void Run(Window window, GameMenu menu, Game game, ref GameState gameState)
        {
            while (gameState != GameState.Exiting)
            {
                SplashKit.ProcessEvents();
                window.Clear(Color.White); // Clear the window with white color

                switch (gameState)
                {
                    case GameState.Menu:
                        menu.Show(ref gameState);
                        break;
                    case GameState.Playing:
                        game.HandleInput();
                        game.Update();
                        game.Draw();

                        if (window.CloseRequested) gameState = GameState.Exiting;
                        break;
                }

                window.Refresh();
            }
        }
    }
}
