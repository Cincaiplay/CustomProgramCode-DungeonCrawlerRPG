using SplashKitSDK;
using System;

namespace CustomProgramCode
{
    public class Button
    {
        private string _text;
        private double _x, _y, _width, _height;
        private Font _font;
        private bool _isHovered;

        public Button(string text, double x, double y, double width, double height)
        {
            _text = text;
            _x = x;
            _y = y;
            _width = width;
            _height = height;
            _font = new Font("Arial", "arial.ttf"); // Load a font (ensure arial.ttf is in your resources)
        }

        public void Draw()
        {
            Color buttonColor = _isHovered ? Color.Gray : Color.LightGray;
            SplashKit.FillRectangle(buttonColor, _x, _y, _width, _height);
            SplashKit.DrawText(_text, Color.Black, _font, 20, _x + 10, _y + 10);

            // Check if the mouse is over the button
            _isHovered = SplashKit.PointInRectangle(SplashKit.MousePosition(), SplashKit.RectangleFrom(_x, _y, _width, _height));
        }

        public bool IsClicked()
        {
            return _isHovered && SplashKit.MouseClicked(MouseButton.LeftButton);
        }
    }
}

