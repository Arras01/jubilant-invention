using System.Collections.Generic;
using System.Linq;

namespace Template.Objects
{
    public class Scene
    {
        public IEnumerable<ISceneObject> Objects = new List<ISceneObject>();

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
    }
}
