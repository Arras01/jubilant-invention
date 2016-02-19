using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Template.Objects
{
    public class Scene
    {
        public IEnumerable<ISceneObject> Objects = new List<ISceneObject>();
        public IEnumerable<PointLight> PointLights = new List<PointLight>(); 

        public void BruteForceFindNearestIntersection(Ray r)
        {
            foreach (var sceneObject in Objects)
            {
                sceneObject.Intersect(r);
            }
        }

        public bool BruteForceFindAnyIntersection(Ray r)
        {
            return Objects.Any(sceneObject => sceneObject.Intersect(r));
        }

        public bool BruteForceCheckFreePath(Vector3 a, Vector3 b)
        {
            float distance = (b - a).Length;
            Vector3 direction = (b - a).Normalized();
            Ray lightRay = new Ray(a + direction * 0.0001f, direction);
            foreach (var sceneObject in Objects)
            {
                sceneObject.Intersect(lightRay);
                if (lightRay.NearestIntersection < distance)
                    return false;
            }
            return true;
        }
    }
}
