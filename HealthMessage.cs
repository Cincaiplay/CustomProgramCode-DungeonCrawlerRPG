using SplashKitSDK;
using System;
using System.Collections.Generic;

namespace CustomProgramCode
{
    public class HealthMessage
    {
        private double _x, _y;
        private string _text;
        private Color _color;
        private int _duration;

        public HealthMessage(double x, double y, string text, Color color)
        {
            _x = x;
            _y = y;
            _text = text;
            _color = color;
            _duration = 1000; // Duration of the message in frames
        }

        public void Update()
        {
            if (_duration > 0)
            {
                _duration--;
                _y -= 0.05; // Move the message upwards
            }
        }

        public void Render()
        {
            if (_duration > 0)
            {
                SplashKit.DrawText(_text, _color, _x, _y);
            }
        }

        public bool IsExpired()
        {
            return _duration <= 0;
        }
    }
}
