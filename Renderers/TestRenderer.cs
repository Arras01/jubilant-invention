using OpenTK;
using Template.Objects;

namespace Template
{
    class TestRenderer : Renderer
    {
        public override Vector3 Trace(Ray r, int depth)
        {
            Scene.BvhFindNearestIntersection(r);
            return r.NearestIntersection < float.MaxValue ? new Vector3(1, 1, 1) : new Vector3();
        }
    }
}
