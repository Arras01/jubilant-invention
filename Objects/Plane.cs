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

        public override AABB GetAABB()
        {
            throw new System.NotImplementedException();
        }

        public override bool Intersect(Ray r)
        {
            float denom = Vector3.Dot(r.Direction, Normal);
            if (denom > 0.0001f) return false;

            Vector3 placeDifference = Point - r.Origin;

            float t = Vector3.Dot(placeDifference, Normal) / denom;
            if (t < r.NearestIntersection && t > 0.0001f)
            {
                r.IntersectedMaterial = Material;
                r.NearestIntersection = t;
                r.IntersectionNormal = Normal;
                return true;
            }

            return false;
        }

        public Vector3 Point => Normal * Distance;
    }
}
