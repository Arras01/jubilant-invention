﻿using System.Collections.Generic;
using System.Linq;
using OpenTK;

namespace Template.Objects
{
    public class Scene
    {
        public IEnumerable<RenderableObject> Objects = new List<RenderableObject>();
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
