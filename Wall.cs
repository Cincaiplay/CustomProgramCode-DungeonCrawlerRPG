using SplashKitSDK;
using System;

namespace CustomProgramCode
{
    public class Wall
    {
        public Sprite _sprite;

        public Sprite Sprite => _sprite; // Property to access the sprite

        public Wall(int cellIndex, float x, float y)
        {
            Bitmap wallBitmap = SplashKit.LoadBitmap("Wall", "Wall.png"); // Load the wall bitmap
            wallBitmap.SetCellDetails(64, 64, 8, 4, 32); // Set cell details for animation

            AnimationScript wallAnimations = SplashKit.LoadAnimationScript("wall_animations", "Wall.txt"); // Load animation script
            _sprite = SplashKit.CreateSprite(wallBitmap, wallAnimations); // Create sprite with animations
            _sprite.StartAnimation($"wall_{cellIndex}"); // Start animation based on cell index
            _sprite.X = x; // Set x position
            _sprite.Y = y; // Set y position
        }

        public void Draw()
        {
            SplashKit.DrawSprite(_sprite); // Draw the sprite
        }

    }
}
