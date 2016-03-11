using OpenTK;
using Template.Objects;

namespace Template
{
    class TestRenderer : Renderer
    {
        public override Vector3 Trace(Ray r, int depth)
        {
            return Scene.BruteForceFindAnyIntersection(r) ? new Vector3(255, 255, 255) : new Vector3();
        }
    }
}
