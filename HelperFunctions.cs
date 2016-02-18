using System.Drawing;

namespace Template
{
    static class HelperFunctions
    {
        public static int ColorToInt(Color c)
        {
            return c.R * 0x10000 + c.G * 0x100 + c.B;
        }
    }
}
