using SplashKitSDK;
using System.Collections.Generic;

namespace CustomProgramCode
{
    public class Level
    {
        private List<Wall> _walls;
        private List<Enemy> _enemies;
        private const int TileSize = 64; // Size of each tile in the grid
        private Window _window;

        public Level(Window window)
        {
            _walls = new List<Wall>();
            _enemies = new List<Enemy>();
            _window = window;
        }

        public void MakeWall(int cellIndex, int tileIndex)
        {
            // Calculate the x and y position on the grid based on the tile index
            int tilesPerRow = _window.Width / TileSize;
            int x = (tileIndex % tilesPerRow) * TileSize;
            int y = (tileIndex / tilesPerRow) * TileSize;

            // Create a new wall and add it to the list
            _walls.Add(new Wall(cellIndex, x, y));
        }

        public void InitializeLevel(int levelNumber)
        {
            // Clear existing walls and enemies
            _walls.Clear();
            _enemies.Clear();

            // Define wall and enemy configurations for different levels
            switch (levelNumber)
            {
                case 1:
                    #region level 1
                    // Walls for level 1
                    #region border walls
                    //upper wall
                    for (int i = 0; i <= 19; i++)
                    {
                        MakeWall(9, i);
                    }
                    for (int i = 21; i < 39; i++)
                    {
                        MakeWall(25, i);
                    }

                    // right wall
                    for (int i = 39; i <= 279; i += 20)
                    {
                        MakeWall(8, i);
                    }
                    MakeWall(9, 299);

                    // bottom wall
                    for (int i = 281; i <= 298; i++)
                    {
                        MakeWall(1, i);
                    }
                    MakeWall(9, 280);

                    // left wall
                    MakeWall(2, 80);
                    for (int i = 20; i <= 260; i += 20)
                    {
                        MakeWall(10, i);
                    }
                    #endregion

                    #region rooms
                    //MakeWall(9, 30);
                    for (int i = 30; i <= 170; i += 20)
                    {
                        MakeWall(11, i);
                    }
                    MakeWall(27, 190);

                    #endregion

                    // Enemies for level 1
                    _enemies.Add(new Goblin(200, 200));
                    _enemies.Add(new Goblin(900, 700));
                    #endregion
                    break;
                case 2:
                    #region level 2
                    // Walls for level 2
                    #region border walls
                    //upper wall
                    for (int i = 0; i <= 19; i++)
                    {
                        MakeWall(9, i);
                    }
                    for (int i = 21; i < 39; i++)
                    {
                        MakeWall(25, i);
                    }

                    // right wall
                    for (int i = 39; i <= 279; i += 20)
                    {
                        MakeWall(8, i);
                    }
                    MakeWall(9, 299);

                    // bottom wall
                    for (int i = 281; i <= 298; i++)
                    {
                        MakeWall(1, i);
                    }
                    MakeWall(9, 280);

                    // left wall
                    MakeWall(2, 80);
                    for (int i = 20; i <= 260; i += 20)
                    {
                        MakeWall(10, i);
                    }
                    #endregion

                    #region rooms
                    MakeWall(3, 69);
                    for (int i = 143; i <= 156; i++)
                    {
                        MakeWall(1, i);
                    }
                    for (int i = 163; i <= 176; i++)
                    {
                        MakeWall(25, i);
                    }
                    for (int i = 89; i <= 229; i += 20)
                    {
                        MakeWall(11, i);
                    }
                    MakeWall(0, 142);

                    MakeWall(24, 162);
                    MakeWall(26, 177);
                    MakeWall(2, 157);
                    MakeWall(9, 149);
                    MakeWall(27, 249);


                    #endregion

                    // Enemies for level 2
                    _enemies.Add(new Goblin(300, 300));
                    _enemies.Add(new Goblin(650, 300));
                    _enemies.Add(new Goblin(750, 800));
                    #endregion

                    break;
                case 3:
                    #region Level 3
                    // Walls for level 3
                    #region border walls
                    //upper wall
                    for (int i = 0; i <= 19; i++)
                    {
                        MakeWall(9, i);
                    }
                    for (int i = 21; i < 39; i++)
                    {
                        MakeWall(25, i);
                    }

                    // right wall
                    for (int i = 39; i <= 279; i += 20)
                    {
                        MakeWall(8, i);
                    }
                    MakeWall(9, 299);

                    // bottom wall
                    for (int i = 281; i <= 298; i++)
                    {
                        MakeWall(1, i);
                    }
                    MakeWall(9, 280);

                    // left wall
                    MakeWall(2, 80);
                    for (int i = 20; i <= 260; i += 20)
                    {
                        MakeWall(10, i);
                    }
                    #endregion


                    #region rooms
                    // top right pillar
                    MakeWall(0, 74);
                    MakeWall(1, 75);
                    MakeWall(2, 76);

                    MakeWall(8, 94);
                    MakeWall(9, 95);
                    MakeWall(10, 96);

                    MakeWall(24, 114);
                    MakeWall(25, 115);
                    MakeWall(26, 116);

                    //bottom left pillar
                    MakeWall(0, 183);
                    MakeWall(1, 184);
                    MakeWall(2, 185);

                    MakeWall(8, 203);
                    MakeWall(9, 204);
                    MakeWall(10, 205);

                    MakeWall(24, 223);
                    MakeWall(25, 224);
                    MakeWall(26, 225);

                    //Middle left pillar
                    MakeWall(1, 129);
                    MakeWall(1, 130);
                    MakeWall(2, 131);

                    MakeWall(8, 149);
                    MakeWall(9, 150);
                    MakeWall(10, 151);

                    MakeWall(24, 169);
                    MakeWall(25, 170);
                    MakeWall(26, 171);

                    //connector
                    MakeWall(9, 120);
                    for (int i = 121; i <= 128; i++)
                    {
                        MakeWall(1, i);
                    }
                    for (int i = 141; i <= 148; i++)
                    {
                        MakeWall(25, i);
                    }

                    #endregion

                    // Enemies for level 3
                    _enemies.Add(new Goblin(500, 500));
                    _enemies.Add(new Goblin(600, 600));
                    _enemies.Add(new Goblin(700, 700));
                    #endregion
                    break;
                default:
                    // No walls or enemies for the default case
                    break;
            }
        }

        public void DrawWalls()
        {
            foreach (var wall in _walls)
            {
                wall.Draw(); // Draw each wall
            }
        }

        public void DrawEnemies()
        {
            foreach (var enemy in _enemies)
            {
                enemy.Draw(); // Draw each enemy
            }
        }

        public List<Wall> GetWalls()
        {
            return _walls; // Return the list of walls
        }

        public List<Enemy> GetEnemies()
        {
            return _enemies; // Return the list of enemies
        }
    }
}
