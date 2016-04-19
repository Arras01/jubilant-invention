using System;
using OpenTK;
using Template.Objects;

namespace Template
{
    class WhittedRenderer : Renderer
    {
        public override Vector3 Trace(Ray ray, int depth)
        {
            if (depth > 5)
                return new Vector3(0, 0, 0);

            Scene.FindNearestIntersection(ray);

            if (Math.Abs(ray.NearestIntersection - float.MaxValue) < float.Epsilon)
                return new Vector3(0, 0.8f, 1f);

            if (ray.IntersectedMaterial.RefractiveIndex < 0.0001f)
            {
                if (ray.IntersectedMaterial.Specularity < 0.02f)
                    return ray.IntersectedMaterial.Color *
                              DirectIllumination(ray.NearestIntersection * ray.Direction, ray.IntersectionNormal);

                else if (ray.IntersectedMaterial.Specularity > 0.98f)
                {
                    return ray.IntersectedMaterial.Color * Trace(ReflectRay(ray, ray.IntersectionNormal), ++depth);
                }
                else
                    return ray.IntersectedMaterial.Color
                              * DirectIllumination(ray.NearestIntersection * ray.Direction + ray.Origin, ray.IntersectionNormal) *
                              (1 - ray.IntersectedMaterial.Specularity)
                              + Trace(ReflectRay(ray, ray.IntersectionNormal), ++depth) * ray.IntersectedMaterial.Specularity;
            }
            else
            {
                var n1n2 = ray.OriginRefractiveIndex / ray.IntersectedMaterial.RefractiveIndex;
                var cos1 = Vector3.Dot(ray.IntersectionNormal, -ray.Direction);
                bool exiting = false; //TODO: All of this normal flipping stuff should probably go in Intersect
                if (cos1 < 0) //make sure collision with backface of things works properly
                {
                    exiting = true;
                    ray.IntersectionNormal *= -1;
                }
                cos1 = Math.Abs(cos1);

                var k = 1 - Math.Pow(n1n2, 2) * (1 - Math.Pow(cos1, 2));

                if (k < 0)
                {
                    return ray.IntersectedMaterial.Color * Trace(ReflectRay(ray, ray.IntersectionNormal), ++depth);
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

                    Vector3 transmittedColor = ray.IntersectedMaterial.Color * Trace(t, ++depth) * (float)(1 - fr);
                    if (ray.IntersectedMaterial != null)
                    {
                        transmittedColor.X *=
                            (float) Math.Exp(-ray.IntersectedMaterial.Absorption.X*ray.NearestIntersection);
                        transmittedColor.Y *=
                            (float) Math.Exp(-ray.IntersectedMaterial.Absorption.Y*ray.NearestIntersection);
                        transmittedColor.Z *=
                            (float) Math.Exp(-ray.IntersectedMaterial.Absorption.Z*ray.NearestIntersection);
                    }

                    return transmittedColor
                         + ray.IntersectedMaterial.Color * Trace(ReflectRay(ray, ray.IntersectionNormal), ++depth) * (float)fr;
                }
            }
        }

        private Ray ReflectRay(Ray r, Vector3 normal)
        {
            var angle = Vector3.Dot(r.Direction, normal);
            if (angle < 0)
                normal *= -1;
            Vector3 newDirection = r.Direction - 2 * Vector3.Dot(r.Direction, normal) * normal;
            return new Ray(r.Origin + r.Direction * r.NearestIntersection + newDirection * 0.0001f, newDirection);
        }

        private float DirectIllumination(Vector3 intersection, Vector3 normal)
        {
            float result = 0;
            foreach (var pointLight in Scene.PointLights)
            {
                if (Scene.CheckFreePath(intersection, pointLight.Location))
                {
                    var angle = Vector3.Dot(normal, (pointLight.Location - intersection).Normalized());
                    if (angle < 0)
                        angle *= -1;
                    result += pointLight.Intensity
                              *(1/(float) Math.Pow((intersection - pointLight.Location).Length, 2))
                              *Math.Max(angle, 0);
                }
            }
            return result < 1 ? result : 1;
        }
    }
}
