using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Template.Objects
{
    public class Scene
    {
        public List<RenderableObject> Objects = new List<RenderableObject>();
        public List<PointLight> PointLights = new List<PointLight>();
        public List<Triangle> TriangleLights = new List<Triangle>();
        public BVH Bvh = new BVH();

        public void FindNearestIntersection(Ray r)
        {
            BruteForceFindNearestIntersection(r);
        }

        public void FindAnyIntersection(Ray r)
        {
            BruteForceFindAnyIntersection(r);
        }

        public bool CheckFreePath(Vector3 a, Vector3 b)
        {
            return BruteForceCheckFreePath(a, b);
        }

        public void BruteForceFindNearestIntersection(Ray r)
        {
            foreach (var sceneObject in Objects)
            {
                sceneObject.Intersect(r);
            }
        }

        public void BvhFindNearestIntersection(Ray r)
        {
            Bvh.Root.Traverse(r, Objects, Bvh);
        }

        public bool BruteForceFindAnyIntersection(Ray r)
        {
            return Objects.Any(sceneObject => sceneObject.Intersect(r));
        }

        public bool BvhCheckFreePath(Vector3 a, Vector3 b)
        {
            const float epsilon = 0.0001f;
            float distance = (b - a).Length;
            Vector3 direction = (b - a).Normalized();
            Ray lightRay = new Ray(a + direction * epsilon, direction);
            lightRay.NearestIntersection = distance - 2*epsilon;
            Bvh.Root.CheckForAnyCollision(lightRay, Objects, Bvh);
            return true;
        }

        public bool BruteForceCheckFreePath(Vector3 a, Vector3 b)
        {
            const float epsilon = 0.0001f;
            float distance = (b - a).Length;
            Vector3 direction = (b - a).Normalized();
            Ray lightRay = new Ray(a + direction * epsilon, direction);
            foreach (var sceneObject in Objects)
            {
                sceneObject.Intersect(lightRay);
                if (lightRay.NearestIntersection < distance - 2 * epsilon)
                    return false;
            }
            return true;
        }
    }
}
