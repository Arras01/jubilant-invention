using OpenTK;

namespace Template.Objects
{
    public class Ray
    {
        public readonly Vector3 Origin;
        public readonly Vector3 Direction;
        public float NearestIntersection = float.MaxValue;
        public Vector3 IntersectionNormal;
        public Material IntersectedMaterial;

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
}
