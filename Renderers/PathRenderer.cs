using System;
using System.Diagnostics;
using OpenTK;
using Template.Objects;

namespace Template
{
    class PathRenderer : Renderer
    {
        public override Vector3 Trace(Ray ray, int depth)
        {
            /*if (depth > 10)
            {
                return Vector3.Zero;
            }*/

            Scene.BruteForceFindNearestIntersection(ray);

            if (Math.Abs(ray.NearestIntersection - float.MaxValue) < float.Epsilon)
                return new Vector3(0);

            if (ray.IntersectedMaterial.IsLight)
                return ray.IntersectedMaterial.Light * ray.IntersectedMaterial.Color;

            var s = Random.NextDouble();
            /*if (ray.IntersectedMaterial.Specularity > s)
            {
                Ray rf = ReflectRay(ray, ray.IntersectionNormal);
                return ray.IntersectedMaterial.Color * Trace(rf, ++depth);
            }*/

            Vector3 R = WeightedDiffuseReflection(ray.IntersectionNormal);
            Ray r = new Ray(ray.IntersectionPoint, R);
            Vector3 BRDF = ray.IntersectedMaterial.Color / (float)Math.PI;
            float PDF = (float)(Vector3.Dot(ray.IntersectionNormal, R) / Math.PI);
            //float PDF = (float) (1 / (2 * Math.PI));

            float surviveChance = 0.5f;
            if (Random.NextDouble() > surviveChance)
            {
                return Vector3.Zero;
            }

            var t = Trace(r, ++depth);
            var Ei = (t * Vector3.Dot(ray.IntersectionNormal, R) / PDF) / surviveChance;

            //if (Math.Abs(Ei.X) > 0.0001f && Math.Abs(Ei.X - Math.PI * 2) > 0.0001f)
            //    Debugger.Break();

            return BRDF * Ei;
        }

        private Ray ReflectRay(Ray r, Vector3 normal)
        {
            Vector3 newDirection = r.Direction - 2 * Vector3.Dot(r.Direction, normal) * normal;
            return new Ray(r.Origin + r.Direction * r.NearestIntersection + newDirection * 0.0001f, newDirection);
        }

        private Vector3 WeightedDiffuseReflection(Vector3 normal)
        {
            Vector3 u, v;
            if (normal.X > 0.99f)
            {
                u = Vector3.Cross(normal, new Vector3(0, 1, 0));
            }
            else
                u = Vector3.Cross(normal, new Vector3(1, 0, 0));

            v = Vector3.Cross(normal, u);

            Vector3 result;
            var r0 = Random.NextDouble();
            var r1 = Random.NextDouble();
            var r = Math.Sqrt(r0);
            var theta = (2 * Math.PI * r1);
            var x = r0 * Math.Cos(theta);
            var y = r * Math.Sin(theta);
            result = new Vector3((float)x, (float)y, (float)Math.Sqrt(1 - r0));

            var m = new Matrix4(new Vector4(u), new Vector4(v), new Vector4(normal), new Vector4(0, 0, 0, 1));

            var finalresult = Vector3.TransformVector(result, m);

            return finalresult.Normalized();
        }

        private Vector3 OldDiffuseReflection(Vector3 normal)
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
