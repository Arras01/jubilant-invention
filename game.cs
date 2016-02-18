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
            Camera = new Camera();
        }
        /// <summary>
        /// Called once per frame at the start of rendering
        /// </summary>
        public void Tick()
        {
            Screen.Print("hello world!", 2, 2, 0xffffff);
            foreach (var tuple in Camera.GenerateRays(Screen.width, Screen.height))
            {
                Screen.Plot(tuple.Item1, tuple.Item2, HelperFunctions.ColorToInt(Renderer.Trace(tuple.Item3)));
            }
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
