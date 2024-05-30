using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace CustomProgramCode
{
    public class Game
    {
        private Window _window;
        private Level _level;
        private Player _player;
        private List<Enemy> _enemies;
        private int _currentLevel; // Variable to track the current level
        private const int TileSize = 64; // Size of each tile in the grid
        private bool _levelCleared; // Variable to track if the level is cleared
        private bool _gameWon; // Variable to track if the game is won
        private bool _gameLost; // Variable to track if the game is lost
        private SplashKitSDK.Timer _levelClearTimer; // Timer for cooldown before moving to the next level
        private const double LEVEL_CLEAR_COOLDOWN = 3000; // Cooldown time in milliseconds (3 seconds)
        private const int MAX_LEVEL = 3; // Maximum number of levels

        public Game(Window window)
        {
            _window = window;
            _level = new Level(window);
            _player = new Player();
            _enemies = new List<Enemy>();
            _currentLevel = 1; // Start with level 1
            _levelCleared = false; // Initially, no level is cleared
            _gameWon = false; // Initially, the game is not won
            _gameLost = false; // Initially, the game is not lost
            _levelClearTimer = new SplashKitSDK.Timer("LevelClearTimer");
        }

        public void Initialize()
        {
            LoadLevel(_currentLevel); // Load the first level
        }

        public void LoadLevel(int levelNumber)
        {
            _player.SetSpawnPoint(100, 150); // Set player spawn point
            if (levelNumber > MAX_LEVEL)
            {
                _gameWon = true; // Mark the game as won if the maximum level is exceeded
                return;
            }

            _level.InitializeLevel(levelNumber); // Initialize the level
            _enemies = _level.GetEnemies(); // Get the list of enemies in the level
            _levelCleared = false; // Reset level cleared state
            _levelClearTimer.Stop();
            _levelClearTimer.Reset();
        }

        public void DrawBackground()
        {
            // Set the background color
            Color floorColor = Color.Gray;
            _window.Clear(floorColor);

            // Draw grid lines
            Color lineColor = Color.DarkGray;
            int cellSize = TileSize; // Size of each cell in the grid

            for (int x = 0; x < _window.Width; x += cellSize)
            {
                SplashKit.DrawLine(lineColor, x, 0, x, _window.Height); // Vertical lines
            }

            for (int y = 0; y < _window.Height; y += cellSize)
            {
                SplashKit.DrawLine(lineColor, 0, y, _window.Width, y); // Horizontal lines
            }
        }

        public void HandleInput()
        {
            if (!_levelCleared && !_gameWon && !_gameLost)
            {
                _player.HandleInput(_level.GetWalls(), _enemies); // Handle player input for movement and actions
            }

            // Example of changing levels with key press (for debugging and presenting)
            if (SplashKit.KeyTyped(KeyCode.Num1Key)) LoadLevel(1);
            if (SplashKit.KeyTyped(KeyCode.Num2Key)) LoadLevel(2);
            if (SplashKit.KeyTyped(KeyCode.Num3Key)) LoadLevel(3);
        }

        public void Update()
        {
            if (_levelCleared)
            {
                if (_levelClearTimer.Ticks >= LEVEL_CLEAR_COOLDOWN)
                {
                    _currentLevel++;
                    LoadLevel(_currentLevel); // Load the next level after cooldown
                }
                return; // Skip the rest of the update if the level is cleared
            }

            if (_gameWon || _gameLost) return; // Do not update if the game is won or lost

            _player.Update(); // Update player state

            if (_player.IsDead)
            {
                _gameLost = true; // Mark the game as lost if the player is dead
                return;
            }

            for (int i = _enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemies[i];

                enemy.HandleMovement(_player, _level.GetWalls()); // Handle enemy movement
                enemy.HandleAttack(_player); // Enemy attacks the player if in range
                enemy.Update(_player); // Update enemy state

                // Check health after updating
                if (enemy.Health <= 0)
                {
                    _enemies.RemoveAt(i); // Remove dead enemy
                }
            }

            // Check if all enemies are cleared
            if (_enemies.Count == 0 && !_levelCleared && !_gameWon)
            {
                _levelCleared = true; // Mark the level as cleared
                _levelClearTimer.Start(); // Start the timer for level transition
            }
        }

        public void Draw()
        {
            _window.Clear(Color.White); // Clear the window with white color
            DrawBackground(); // Draw the background grid
            _player.Draw(); // Draw the player

            _level.DrawWalls(); // Draw the walls
            _level.DrawEnemies(); // Draw the enemies

            if (_levelCleared && !_gameWon && !_gameLost)
            {
                DrawMessage($"You have cleared Level {_currentLevel}!", Color.Green); // Display level cleared message
            }
            else if (_gameWon)
            {
                DrawMessage("You have won!", Color.Green); // Display game won message
            }
            else if (_gameLost)
            {
                DrawMessage("You have lost!", Color.Red); // Display game lost message
            }

            _window.Refresh(); // Refresh the window display
        }

        private void DrawMessage(string message, Color color)
        {
            int fontSize = 24;
            Font font = SplashKit.LoadFont("Arial", "arial.ttf"); // Load the font
            float x = (_window.Width - SplashKit.TextWidth(message, font, fontSize)) / 2; // Calculate x position
            float y = (_window.Height - SplashKit.TextHeight(message, font, fontSize)) / 2; // Calculate y position
            SplashKit.DrawText(message, color, font, fontSize, x, y); // Draw the message text
        }
    }
}
