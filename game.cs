using System.Collections.Generic;
using System.Diagnostics;
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
#if false
            Renderer.Scene = ObjLoader.LoadScene("C:\\Users\\Jasper\\Desktop\\sphere.obj");
#else
            Renderer.Scene = new Scene();
            Renderer.Scene.Objects = new List<RenderableObject>()
            { new Sphere(new Vector3(0, 0, -5), 2, Material.TestDiffuseMaterial) };
#endif
            Renderer.Scene.PointLights = new List<PointLight>
                { new PointLight(new Vector3(0, -5, 0), 25f)};
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
                Screen.Plot(tuple.Item1, tuple.Item2, HelperFunctions.VectorColorToInt(c));
                
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
