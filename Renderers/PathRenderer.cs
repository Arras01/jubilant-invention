using System;
using OpenTK;
using Template.Objects;

namespace Template
{
    class PathRenderer : Renderer
    {
        public override Vector3 Trace(Ray ray, int depth)
        {
            if (depth > 10)
            {
                return Vector3.Zero;
            }

            Scene.BruteForceFindNearestIntersection(ray);

            if (Math.Abs(ray.NearestIntersection - float.MaxValue) < float.Epsilon)
                return new Vector3(0);

            if (ray.IntersectedMaterial.IsLight)
                return ray.IntersectedMaterial.Light * ray.IntersectedMaterial.Color;

            var s = Random.NextDouble();
            if (ray.IntersectedMaterial.Specularity > s)
            {
                Ray rf = ReflectRay(ray, ray.IntersectionNormal);
                return ray.IntersectedMaterial.Color * Trace(rf, ++depth);
            }

            Vector3 R = DiffuseReflection(ray.IntersectionNormal);
            Vector3 BRDF = ray.IntersectedMaterial.Color / (float)Math.PI;
            Ray r = new Ray(ray.IntersectionPoint, R);

            var Ei = Trace(r, ++depth) * Vector3.Dot(ray.IntersectionNormal, R);
            return (float)Math.PI * 2f * BRDF * Ei;
        }

        private Ray ReflectRay(Ray r, Vector3 normal)
        {
            Vector3 newDirection = r.Direction - 2 * Vector3.Dot(r.Direction, normal) * normal;
            return new Ray(r.Origin + r.Direction * r.NearestIntersection + newDirection * 0.0001f, newDirection);
        }

        private Vector3 DiffuseReflection(Vector3 normal)
        {
            Vector3 result;
            do
            {
                var x = Random.NextDouble() * 2 - 1;
                var y = Random.NextDouble() * 2 - 1;
                var z = Random.NextDouble() * 2 - 1;
                result = new Vector3((float)x, (float)y, (float)z);
            } while (result.Length > 1 || Vector3.Dot(result, normal) < 0);

            return result.Normalized();
        }
    }
}
