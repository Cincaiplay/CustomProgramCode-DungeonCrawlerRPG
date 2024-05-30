using SplashKitSDK;
using System;

namespace CustomProgramCode
{
    public class Wall
    {
        public Sprite _sprite;

        public Sprite Sprite => _sprite; // Add this property

        public Wall(int cellIndex, float x, float y)
        {
            Bitmap wallBitmap = SplashKit.LoadBitmap("Wall", "Wall.png");
            wallBitmap.SetCellDetails(64, 64, 8, 4, 32); // Set cell details

            AnimationScript wallAnimations = SplashKit.LoadAnimationScript("wall_animations", "Wall.txt");
            _sprite = SplashKit.CreateSprite(wallBitmap, wallAnimations);
            _sprite.StartAnimation($"wall_{cellIndex}");
            _sprite.X = x;
            _sprite.Y = y;
        }

        public void Draw()
        {
            SplashKit.DrawSprite(_sprite);
        }

        public bool CollidesWith(Player player)
        {
            return SplashKit.SpriteCollision(_sprite, player.Sprite);
        }
    }
}
