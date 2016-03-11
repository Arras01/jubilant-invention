using System;
using OpenTK;

namespace Template.Objects
{

    class CheckboardPlane : Plane
    {
        public override bool Intersect(Ray r)
        {
            float t = -Vector3.Dot(r.Origin, Normal) + Distance / Vector3.Dot(r.Direction, Normal);
            if (t < r.NearestIntersection && t > 0.0001f)
            {
                Vector3 i = r.Origin + r.Direction * t;
                if (Math.Abs(Math.Floor(i.X / 4) + Math.Floor(i.Z / 4)) % 2 < 0.0001f)
                {
                    r.IntersectedMaterial = Material.TestBlackMaterial;
                }
                else
                {
                    r.IntersectedMaterial = Material.TestWhiteMaterial;
                }
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
