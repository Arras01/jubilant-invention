using System;
using System.Diagnostics;
using OpenTK;
using Template.Objects;

namespace Template
{
    class PathRenderer : Renderer
    {
        private float surviveChance = 0.5f;

        public override Vector3 Trace(Ray ray, int depth)
        {
            return Sample(ray, true, depth);
        }

        public Vector3 Sample(Ray ray, bool lastSpecular, int depth)
        {
            Scene.FindNearestIntersection(ray);

            //prevent stack overflows
            if (depth > 100)
                return Vector3.Zero;

            if (Math.Abs(ray.NearestIntersection - float.MaxValue) < float.Epsilon)
                return new Vector3(0);

            Vector3 BRDF = ray.IntersectedMaterial.Color / (float)Math.PI;

            if (ray.IntersectedMaterial.IsLight)
            {
                if (lastSpecular)
                    return ray.IntersectedMaterial.Light * ray.IntersectedMaterial.Color;
                else
                    return Vector3.Zero;
            }
            var s = Random.NextDouble();
            if (ray.IntersectedMaterial.Specularity > s)
            {
                Ray rf = ReflectRay(ray, ray.IntersectionNormal);
                return ray.IntersectedMaterial.Color * Sample(rf, true, ++depth);
            }

            #region refraction
            if (ray.IntersectedMaterial.RefractiveIndex > 0.001f)
            {
                var n1n2 = ray.OriginRefractiveIndex / ray.IntersectedMaterial.RefractiveIndex;
                var cos1 = Vector3.Dot(ray.IntersectionNormal, -ray.Direction);
                bool exiting = false;
                if (cos1 < 0) //make sure collision with backface of things works properly
                {
                    exiting = true;
                    ray.IntersectionNormal *= -1;
                }
                cos1 = Math.Abs(cos1);

                var k = 1 - Math.Pow(n1n2, 2) * (1 - Math.Pow(cos1, 2));

                if (k < 0)
                {
                    Ray rf = ReflectRay(ray, ray.IntersectionNormal);
                    return ray.IntersectedMaterial.Color * Trace(rf, 1);
                }
                else
                {
                    var n2 = ray.IntersectedMaterial.RefractiveIndex;
                    if (exiting)
                        n2 = 1;
                    //Something still seems wrong about the reflection, but I'm pretty sure the formulas are correct
                    var r0 =
                        Math.Pow(
                            (ray.OriginRefractiveIndex - n2) /
                            (ray.OriginRefractiveIndex + n2), 2);
                    var fr = r0 + (1 - r0) * Math.Pow(1 - cos1, 5);

                    var newDirection = n1n2 * ray.Direction - ray.IntersectionNormal * (float)(n1n2 * cos1 - Math.Sqrt(k));
                    newDirection.Normalize();
                    Ray t = new Ray(ray.Origin + ray.Direction * ray.NearestIntersection + newDirection * 0.0001f
                        , newDirection);

                    if (!exiting)
                        t.OriginRefractiveIndex = ray.IntersectedMaterial.RefractiveIndex;

                    var roll = Random.NextDouble();
                    if (roll < fr)
                        return ray.IntersectedMaterial.Color * Sample(ReflectRay(ray, ray.IntersectionNormal), true, ++depth);
                    else
                    {
                        var transmittedColor = ray.IntersectedMaterial.Color * Sample(t, true, ++depth);
                        if (ray.IntersectedMaterial != null)
                        {
                            transmittedColor.X *=
                                (float)Math.Exp(-ray.IntersectedMaterial.Absorption.X * ray.NearestIntersection);
                            transmittedColor.Y *=
                                (float)Math.Exp(-ray.IntersectedMaterial.Absorption.Y * ray.NearestIntersection);
                            transmittedColor.Z *=
                                (float)Math.Exp(-ray.IntersectedMaterial.Absorption.Z * ray.NearestIntersection);
                        }
                        return transmittedColor;
                    }
                }
            }
            #endregion

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
            var c = ray.IntersectedMaterial.Color;
            surviveChance = MathHelper.Clamp((c.X + c.Y + c.Z) / 3, 0.1f, 0.9f);

            if (Random.NextDouble() > surviveChance)
            {
                return Vector3.Zero + Ld;
            }

            Vector3 R = DiffuseReflection(ray.IntersectionNormal);
            Ray r = new Ray(ray.IntersectionPoint, R);

            float PDF = (float) (Vector3.Dot(ray.IntersectionNormal, R) / Math.PI);
            var Ei = (Sample(r, false, ++depth) * Vector3.Dot(ray.IntersectionNormal, R) / PDF) / surviveChance;

            return BRDF*Ei + Ld;
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

            /*do
            {
                var x = Random.NextDouble()*2 - 1;
                var y = Random.NextDouble()*2 - 1;
                var z = Random.NextDouble();
                result = new Vector3((float) x, (float) y, (float) z);
            } while (result.Length > 1);*/

            var m = new Matrix4(new Vector4(u), new Vector4(v), new Vector4(normal), new Vector4(0, 0, 0, 1));

            var finalresult = Vector3.TransformVector(result, m);

            return finalresult.Normalized();
        }
    }
}
