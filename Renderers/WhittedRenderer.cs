using System;
using OpenTK;
using OpenTK.Graphics.ES11;
using Template.Objects;

namespace Template
{
    class WhittedRenderer : Renderer
    {
        public override Vector3 Trace(Ray r, int depth)
        {
            if (depth > 20)
                return new Vector3();

            Scene.BruteForceFindNearestIntersection(r);
            if (Math.Abs(r.NearestIntersection - float.MaxValue) < float.Epsilon)
                return new Vector3(0, 0.8f, 1f);

            //if (r.IntersectedMaterial.RefractiveIndex < 0.0001f)
            {
                if (r.IntersectedMaterial.Specularity < 0.02f)
                    return r.IntersectedMaterial.Color *
                              DirectIllumination(r.NearestIntersection * r.Direction, r.IntersectionNormal);

                else if (r.IntersectedMaterial.Specularity > 0.98f)
                {
                    return r.IntersectedMaterial.Color * Trace(ReflectRay(r, r.IntersectionNormal), ++depth);
                }
                else
                    return r.IntersectedMaterial.Color
                              * DirectIllumination(r.NearestIntersection * r.Direction, r.IntersectionNormal) *
                              (1 - r.IntersectedMaterial.Specularity)
                              + Trace(ReflectRay(r, r.IntersectionNormal), ++depth) * r.IntersectedMaterial.Specularity;
            }
            /*else
            {
                var n1n2 = 1 / 
                var k = 1 - 
            }*/
        }

        private Ray ReflectRay(Ray r, Vector3 normal)
        {
            Vector3 newDirection = r.Direction - 2 * Vector3.Dot(r.Direction, normal) * normal;
            return new Ray(r.Origin + r.Direction * r.NearestIntersection + newDirection * 0.0001f, newDirection);
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
