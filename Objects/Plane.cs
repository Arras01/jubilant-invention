using OpenTK;

namespace Template.Objects
{
    class Plane : RenderableObject
    {
        public float Distance;
        public Vector3 Normal;

        public Plane(float distance, Vector3 normal, Material material)
        {
            Distance = distance;
            Normal = normal;
            Material = material;
        }

        public override bool Intersect(Ray r)
        {
            float t = -Vector3.Dot(r.Origin, Normal) + Distance / Vector3.Dot(r.Direction, Normal);
            if (t < r.NearestIntersection && t > 0.0001f)
            {
                r.IntersectedMaterial = Material;
                r.NearestIntersection = t;
                r.IntersectionNormal = Normal;
                return true;
            }

            return false;
        }
    }
}
