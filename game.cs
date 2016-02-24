using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using OpenTK;
using Template.Objects;

namespace Template
{
    class Game
    {
        public Surface Screen;
        public Renderer Renderer;
        public Camera Camera;

        public void Init()
        {
            Screen.Clear(0x2222ff);
            Renderer = new WhittedRenderer();
            Renderer.Scene = ObjLoader.LoadScene("C:\\Users\\Jasper\\Desktop\\sphere.obj");
            Renderer.Scene.PointLights = new List<PointLight>
                { new PointLight(new Vector3(10, 10, 10), 2500f)};
            Camera = new Camera();
        }
        /// <summary>
        /// Called once per frame at the start of rendering
        /// </summary>
        public void Tick()
        {
            //Screen.Print("hello world!", 2, 2, 0xffffff);
            foreach (var tuple in Camera.GenerateRays(Screen.width, Screen.height))
            {
                var c = Renderer.Trace(tuple.Item3);
                Screen.Plot(tuple.Item1, tuple.Item2, HelperFunctions.ColorToInt(c));
                
            }
            Debug.WriteLine("render done");
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
