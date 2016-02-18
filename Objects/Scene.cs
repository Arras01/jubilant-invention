using System.Collections.Generic;

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

        public void BruteForceFindAnyIntersection(Ray r)
        {
            foreach (var sceneObject in Objects)
            {
                if (sceneObject.Intersect(r))
                    break;
            }
        }
    }
}
