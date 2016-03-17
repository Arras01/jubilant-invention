using System;
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
#if true
            Renderer.Scene = ObjLoader.LoadScene("C:\\Users\\Jasper\\Desktop\\sphere.obj");
            Renderer.Scene.Bvh.ConstructBVH(Renderer.Scene.Triangles);
#else
            Renderer.Scene = new Scene();
            Renderer.Scene.Objects = new List<RenderableObject>()
            {
                new Sphere(new Vector3(0, -4, -10), 5, Material.TestRefractiveMaterial),
                new Sphere(new Vector3(-5, 3, -20), 2, Material.TestDiffuseMaterial),
                new Sphere(new Vector3(5, 3, -11), 2, Material.TestDiffuseMaterial),
                new Sphere(new Vector3(2, 2, -14), 2, Material.TestSpecularMaterial),
                new Sphere(new Vector3(-3, 3, -10), 1f, Material.TestRefractiveMaterial),
                new CheckboardPlane(5, new Vector3(0, 1, 0), Material.TestWhiteMaterial),
                //new Sphere(new Vector3(0,0,0), 50, Material.TestWhiteMaterial)
            };
#endif
            Renderer.Scene.PointLights = new List<PointLight>
            {
                new PointLight(new Vector3(0, -5, 0), 250f),
                new PointLight(new Vector3(0, -5, -20), 250f)
            };
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
                var c = Renderer.Trace(tuple.Item3, 1);
                Screen.Plot(tuple.Item1, tuple.Item2, HelperFunctions.VectorColorToInt(c));

            }
            Console.WriteLine("frame");
        }

        /// <summary>
        /// Called once per frame at the end of rendering?
        /// "render stuff over the backbuffer (OpenGL, sprites)"
        /// </summary>
        public void Render()
        {
        }

        public void CheckPixel(int x, int y)
        {
            Ray r = Camera.GenerateRay(x, y, Screen.width, Screen.height).Item3;
            Debugger.Break();
            Renderer.Trace(r, 1);
        }
    }
}
