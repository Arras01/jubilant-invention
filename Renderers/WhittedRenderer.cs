using System;
using OpenTK;
using Template.Objects;

namespace Template
{
    class WhittedRenderer : Renderer
    {
        public override Vector3 Trace(Ray r)
        {
            Scene.BruteForceFindNearestIntersection(r);
            if (Math.Abs(r.NearestIntersection - float.MaxValue) < float.Epsilon)
                return new Vector3(0, 0xBF, 0xFF);
            return r.IntersectedMaterial.Color *
                DirectIllumination(r.NearestIntersection * r.Direction, r.IntersectionNormal);
        }

        private float DirectIllumination(Vector3 intersection, Vector3 normal)
        {
            float result = 0;
            foreach (var pointLight in Scene.PointLights)
            {
                if (Scene.BruteForceCheckFreePath(intersection, pointLight.Location))
                    result += pointLight.Intensity
                              * (1 / (float)Math.Pow((intersection - pointLight.Location).Length, 2))
                              * Math.Max(Vector3.Dot(normal, (pointLight.Location - intersection).Normalized()), 0);
            }
            return result < 1 ? result : 1;
        }
    }
}
