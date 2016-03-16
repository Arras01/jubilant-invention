using System;
using OpenTK;

namespace Template.Objects
{

    class CheckboardPlane : Plane
    {
        public override bool Intersect(Ray r)
        {
            float denom = Vector3.Dot(r.Direction, Normal);
            if (denom < 0.0001f) return false;

            Vector3 placeDifference = Point - r.Origin;

            float t = Vector3.Dot(placeDifference, Normal) / denom;
            if (t < r.NearestIntersection && t > 0.0001f)
            {
                Vector3 i = r.Origin + r.Direction * t;
                r.IntersectedMaterial = 
                    Math.Abs(Math.Floor(i.X / 4) + Math.Floor(i.Z / 4)) % 2 < 0.0001f 
                        ? Material.TestBlackMaterial 
                        : Material.TestWhiteMaterial;
                r.NearestIntersection = t;
                r.IntersectionNormal = Normal;
                return true;
            }

            return false;
        }

        public CheckboardPlane(float distance, Vector3 normal, Material material) : base(distance, normal, material)
        {
        }
    }
}
