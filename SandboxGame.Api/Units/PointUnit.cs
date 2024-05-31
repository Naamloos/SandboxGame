using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SandboxGame.Api.Units
{
    public class PointUnit
    {
        public float X;
        public float Y;

        public PointUnit(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static PointUnit Zero => new PointUnit(0, 0);

        // "borrowed" from MonoGame
        public static float Distance(PointUnit value1, PointUnit value2)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            return MathF.Sqrt(num * num + num2 * num2);
        }

        public static PointUnit operator -(PointUnit value)
        {
            value.X = 0f - value.X;
            value.Y = 0f - value.Y;
            return value;
        }

        public static PointUnit operator -(PointUnit value1, PointUnit value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;
            return value1;
        }

        public static PointUnit operator *(PointUnit value, float scaleFactor)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        public static PointUnit operator *(float scaleFactor, PointUnit value)
        {
            value.X *= scaleFactor;
            value.Y *= scaleFactor;
            return value;
        }

        public static PointUnit operator +(PointUnit value1, PointUnit value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;
            return value1;
        }

        public void Normalize()
        {
            float num = 1f / MathF.Sqrt(X * X + Y * Y);
            X *= num;
            Y *= num;
        }
    }
}
