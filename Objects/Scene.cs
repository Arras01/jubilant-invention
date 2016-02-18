using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Template.Objects
{
    class Scene
    {
        public readonly List<ISceneObject> Objects = new List<ISceneObject>();

        public void BruteForceIntersect(Ray r)
        {
            foreach (var sceneObject in Objects)
            {
                sceneObject.Intersect(r);
            }
        }
    }
}
