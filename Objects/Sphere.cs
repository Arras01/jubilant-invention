using System;
using OpenTK;

namespace Template.Objects
{
    class Sphere : RenderableObject
    {
        public Vector3 Position;
        public float Radius;
        public readonly float Radius2;

        public Sphere(Vector3 position, float radius, Material material)
        {
            Position = position;
            Radius = radius;
            Radius2 = (float)Math.Pow(radius, 2);
            Material = material;
        }
#if false
        public override bool Intersect(Ray r)
        {
            Vector3 c = Position - r.Origin;
            float t = Vector3.Dot(c, r.Direction);
            Vector3 q = c - t * r.Direction;
            float p2 = Vector3.Dot(q, q);
            if (p2 > Radius2)
                return false;
            t -= (float)Math.Sqrt(Radius2 - p2);
            if ((t < r.NearestIntersection) && (t > 0))
            {
                r.IntersectedMaterial = Material;
                r.NearestIntersection = t;
                r.IntersectionNormal = (r.Origin + r.Direction * t - Position).Normalized();
                return true;
            }
            return false;
        }
#else
        public override bool Intersect(Ray r)
        {
            var a = Vector3.Dot(r.Direction, r.Direction);
            var b = 2 * Vector3.Dot(r.Direction, r.Origin - Position);
            var c = Vector3.Dot(r.Origin - Position, r.Origin - Position) - Radius2;
            var d = b * b - 4 * a * c;
            if (d < 0)
                return false;

            float t = (float)Math.Min((-b + Math.Sqrt(d)) / 2 * a, (-b - Math.Sqrt(d)) / 2 * a);
            if (t < r.NearestIntersection && t > 0.0001f)
            {
                r.IntersectedMaterial = Material;
                r.NearestIntersection = t;
                r.IntersectionNormal = (r.Origin + r.Direction * t - Position).Normalized();
                return true;
            }
            return false;
        }
#endif
    }
}
