using System.Drawing;
using Template.Objects;

namespace Template
{
    public abstract class Renderer
    {
        public Scene Scene;

        public abstract Color Trace(Ray r);
    }
}
