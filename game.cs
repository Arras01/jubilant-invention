using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;

namespace Template
{
    class Game
    {
        public Surface Screen;
        public void Init()
        {
            Screen.Clear(0x2222ff);
        }
        /// <summary>
        /// Called once per frame at the start of rendering
        /// </summary>
        public void Tick()
        {
            Screen.Print("hello world!", 2, 2, 0xffffff);
        }
        /// <summary>
        /// Called once per frame at the end of rendering?
        /// "render stuff over the backbuffer (OpenGL, sprites)"
        /// </summary>
        public void Render()
        {
        }
    }
}
