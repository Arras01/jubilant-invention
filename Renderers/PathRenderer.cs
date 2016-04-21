using System;
using System.Diagnostics;
using OpenTK;
using Template.Objects;

namespace Template
{
    class PathRenderer : Renderer
    {
        private float surviveChance = 0.5f;

        public override Vector3 Trace(Ray ray, int lastSpecular)
        {
            Scene.BruteForceFindNearestIntersection(ray);

            if (Math.Abs(ray.NearestIntersection - float.MaxValue) < float.Epsilon)
                return new Vector3(0);

            Vector3 BRDF = ray.IntersectedMaterial.Color / (float)Math.PI;

            if (ray.IntersectedMaterial.IsLight)
            {
                if (lastSpecular == 1)
                    return ray.IntersectedMaterial.Light * ray.IntersectedMaterial.Color;
                else
                    return Vector3.Zero;
            }
            var s = Random.NextDouble();
            if (ray.IntersectedMaterial.Specularity > s)
            {
                Ray rf = ReflectRay(ray, ray.IntersectionNormal);
                return ray.IntersectedMaterial.Color * Trace(rf, 1);
            }

            Vector3 L;
            Vector3 Nl;
            float area;
            Material m;
            var lightPoint = GetRandomPointOnLightSource(ray.IntersectionPoint, out Nl, out L, out m, out area);
            var Ld = Vector3.Zero;
            if (Vector3.Dot(ray.IntersectionNormal, L) > 0 && Vector3.Dot(Nl, -L) > 0)
                if (Scene.CheckFreePath(ray.IntersectionPoint, lightPoint))
                {
                    var solidAngle = Vector3.Dot(Nl, -L) * area / (lightPoint - ray.IntersectionPoint).LengthSquared;
                    Ld = m.Color * m.Light * solidAngle * BRDF * Vector3.Dot(ray.IntersectionNormal, L);
                }
                
            //unfortunately seems to make images even noisier
            /*var c = ray.IntersectedMaterial.Color;
            surviveChance = MathHelper.Clamp((c.X + c.Y + c.Z) / 3, 0.1f, 0.9f);*/

            if (Random.NextDouble() > surviveChance)
            {
                return Vector3.Zero + Ld;
            }

            Vector3 R = DiffuseReflection(ray.IntersectionNormal);
            Ray r = new Ray(ray.IntersectionPoint, R);

            var Ei = Trace(r, 0) * Vector3.Dot(ray.IntersectionNormal, R) / surviveChance;
            if (Ei.X > 0)
            {
                //Debugger.Break();
            }
            return (float)Math.PI * 2f * BRDF * Ei + Ld;
        }

        private Ray ReflectRay(Ray r, Vector3 normal)
        {
            Vector3 newDirection = r.Direction - 2 * Vector3.Dot(r.Direction, normal) * normal;
            return new Ray(r.Origin + r.Direction * r.NearestIntersection + newDirection * 0.0001f, newDirection);
        }

        private Vector3 GetRandomPointOnLightSource(Vector3 source, out Vector3 lightNormal, out Vector3 lightDirection, out Material lightMaterial, out float area)
        {
            var l = Scene.TriangleLights[Random.Next(Scene.TriangleLights.Count)];
            var u = Random.NextDouble();
            var v = Random.NextDouble();
            lightNormal = l.normal;
            var result = (1 - (float)Math.Sqrt(u)) * l.V1
                + (float)(Math.Sqrt(u) * (1 - v)) * l.V2
                + (float)(v * Math.Sqrt(u)) * l.V3;
            lightDirection = (result - source).Normalized();
            area = 0.5f * Vector3.Cross(l.V2 - l.V1, l.V3 - l.V1).Length;
            lightMaterial = l.Material;
            return result;
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
