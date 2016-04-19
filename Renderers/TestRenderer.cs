using OpenTK;
using Template.Objects;

namespace Template
{
    class TestRenderer : Renderer
    {
        public override Vector3 Trace(Ray ray, int depth)
        {
            Scene.FindNearestIntersection(ray);
            return ray.NearestIntersection < float.MaxValue ? new Vector3(1, 1, 1) : new Vector3();
        }
    }
}
