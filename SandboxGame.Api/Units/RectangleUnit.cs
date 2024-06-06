using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Units
{
    public struct RectangleUnit
    {
        public float Left => X;
        public float Right => X + Width;
        public float Top => Y;
        public float Bottom => Y + Height;

        public float X;
        public float Y;
        public float Width;
        public float Height;

        public RectangleUnit(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public PointUnit Center
        {
            get => new PointUnit(X + Width * 0.5f, Y + Height * 0.5f);
        }

        public PointUnit Position
        {
            get => new PointUnit(X, Y);
        }

        public bool Intersects(RectangleUnit value)
        {
            if (value.Left < Right && Left < value.Right && value.Top < Bottom)
            {
                return Top < value.Bottom;
            }

            return false;
        }

        public void Intersects(ref RectangleUnit value, out bool result)
        {
            result = value.Left < Right && Left < value.Right && value.Top < Bottom && Top < value.Bottom;
        }
    }
}
