using System.Diagnostics;
using System.Drawing;
using Template.Objects;

namespace Template
{
    class WhittedRenderer : Renderer
    {
        public override Color Trace(Ray r)
        {
            if (Scene.BruteForceFindAnyIntersection(r))
                return Color.Red;
            else
            {
                return Color.Green;
            }
        }
    }
}
