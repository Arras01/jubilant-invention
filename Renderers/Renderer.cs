using System;
using OpenTK;
using Template.Objects;

namespace Template
{
    public abstract class Renderer
    {
        public Scene Scene;
        public static Random Random = new Random();

        public abstract Vector3 Trace(Ray ray, int depth);
    }
}
