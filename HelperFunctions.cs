using System;
using System.Drawing;
using OpenTK;

namespace Template
{
    static class HelperFunctions
    {
        public static int ColorToInt(Color c)
        {
            return c.R * 0x10000 + c.G * 0x100 + c.B;
        }

        public static int VectorColorToInt(Vector3 c)
        {
            float t;
            if (c.X > 1)
            {
                t = c.X;
                c /= t;
            }
            if (c.Y > 1)
            {
                t = c.Y;
                c /= t;
            }
            if (c.Z > 1)
            {
                t = c.Z;
                c /= t;
            }
            c *= 255;
            return (int)c.X * 0x10000 + (int)c.Y * 0x100 + (int)c.Z;
        }

        public static Color MultiplyColor(Color c, float m)
        {
            return Color.FromArgb(255, (int)(c.R * m), (int)(c.B * m), (int)(c.G * m));
        }
    }
}
