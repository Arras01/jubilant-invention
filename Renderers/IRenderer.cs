using System.Drawing;
using OpenTK;
using Template.Objects;

namespace Template
{
    public abstract class Renderer
    {
        public Scene Scene;

        public abstract Vector3 Trace(Ray r);
    }
}
