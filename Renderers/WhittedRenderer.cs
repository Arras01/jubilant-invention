using System.Drawing;
using Template.Objects;

namespace Template
{
    class WhittedRenderer : Renderer
    {
        public override Color Trace(Ray r)
        {
            return Color.Aquamarine;
        }
    }
}
