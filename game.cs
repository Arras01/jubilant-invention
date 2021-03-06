﻿using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenTK;
using OpenTK.Input;
using Template.Objects;

namespace Template
{
    class Game
    {
        public Surface Screen;
        public Renderer Renderer;
        public Camera Camera;
        Stopwatch timer;
        public Vector3[,] screenBuffer;
        public int framecount;

        public void Init()
        {

            Screen.Clear(0x2222ff);
            screenBuffer = new Vector3[Screen.width, Screen.height];
            Renderer = new PathRenderer();

            Renderer.Scene = new Scene();
            //ObjLoader.LoadObject("../../../Models/teapot3.obj", Renderer.Scene);

            var l1 = new Triangle(new Vector3(-4, 7, -10), new Vector3(4, 7, -10), new Vector3(4, 7, -5),
                Material.TestLightMaterial);
            var l2 = new Triangle(new Vector3(-4, 7, -5), new Vector3(-4, 7, -10), new Vector3(4, 7, -5),
                Material.TestLightMaterial);

            Renderer.Scene.Objects.AddRange(new List<RenderableObject>()
            {
                //new Sphere(new Vector3(0, 0, -10), 5, Material.TestRefractiveMaterial),
                new Sphere(new Vector3(-5, 3, -20), 2, Material.TestDiffuseMaterial),
                new Sphere(new Vector3(5, -3, -11), 2, Material.TestDiffuseMaterial),
                //new Sphere(new Vector3(2, 2, -14), 2, Material.TestLightMaterial),
                //new Sphere(new Vector3(110, 2, -14), 100, Material.TestLightMaterial),
                //new Sphere(new Vector3(3, -3, -13), 2f, Material.TestSpecularMaterial),
                new Sphere(new Vector3(0, 0, -5), 1, Material.TestWhiteMaterial),
               //new Triangle(new Vector3(14, -5, -100), new Vector3(-14, -5, -100), new Vector3(14, -5, -1), Material.TestBlackMaterial),
               //new Triangle(new Vector3(-14, -5, -100), new Vector3(-14, -5, -1), new Vector3(14, -5, -1), Material.TestSpecularMaterial),
               //new Sphere(new Vector3(0, -210, -10), 200, Material.TestWhiteMaterial),
                //new Sphere(new Vector3(0, 0, -1000), 800, Material.TestDiffuseMaterial),
                new CheckboardPlane(-5, new Vector3(0, 1, 0), Material.TestDiffuseMaterial),
                l1,
                l2,
                //new Triangle(new Vector3(-15, -15, -20), new Vector3(15, -15, -20), new Vector3(-15, 15, -20), Material.TestDiffuseMaterial),
                //new Triangle(new Vector3(15, 15, -20), new Vector3(15, -15, 0), new Vector3(15, -15, -20), Material.TestDiffuseMaterial),
                //new Sphere(new Vector3(0,0,0), 50, Material.TestWhiteMaterial)
            });

            Renderer.Scene.TriangleLights = new List<Triangle>
            {
                l1,
                l2
            };

            /*Renderer.Scene.PointLights = new List<PointLight>
            {
                new PointLight(new Vector3(0, 0, 0), 25000f),
                //new PointLight(new Vector3(0, -5, -20), 250f)
            };*/

            //Renderer.Scene.Bvh.ConstructBVH(Renderer.Scene.Objects.ToList());
            Camera = new Camera();
            timer = Stopwatch.StartNew();
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
                screenBuffer[tuple.Item1, tuple.Item2] += c;
            }

            framecount++;

            for (int x = 0; x < Screen.width; x++)
            {
                for (int y = 0; y < Screen.height; y++)
                {
                    Screen.Plot(x, y, HelperFunctions.VectorColorToInt(screenBuffer[x, y] / framecount));
                }
            }

            var fps = 1000 / timer.ElapsedMilliseconds;
            Screen.Print("FPS: " + fps, 0, 0, 0xFFFFFF);
            Screen.Print("Frame time: " + timer.ElapsedMilliseconds, 0, 24, 0xFFFFFF);
            timer.Restart();
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
            var color = Renderer.Trace(r, 1);
            Debug.WriteLine(color);
        }

        public void HandleKeyPress(KeyboardState k)
        {
            if (k.IsKeyDown(Key.Left))
                Camera.MoveCamera(Matrix4.CreateRotationY(-0.1f));
            if (k.IsKeyDown(Key.Right))
                Camera.MoveCamera(Matrix4.CreateRotationY(0.1f));
            if (k.IsKeyDown(Key.Up))
                Camera.MoveCamera(Matrix4.CreateRotationX(0.1f));
            if (k.IsKeyDown(Key.Down))
                Camera.MoveCamera(Matrix4.CreateRotationX(-0.1f));
            framecount = 0;
            screenBuffer = new Vector3[Screen.width, Screen.height];
        }
    }
}
