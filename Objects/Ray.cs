using OpenTK;

namespace Template.Objects
{
    public class Ray
    {
        public readonly Vector3 Origin;
        public readonly Vector3 Direction;
        public readonly Vector3 InvDirection;
        public readonly int[] Sign = new int[3];
        public float NearestIntersection = float.MaxValue;
        public Vector3 IntersectionNormal;
        public Material IntersectedMaterial;
        public float OriginRefractiveIndex = 1;

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
            InvDirection = new Vector3(1 / direction.X, 1 / direction.Y, 1 / direction.Z);
            Sign[0] = InvDirection.X < 0 ? 1 : 0;
            Sign[1] = InvDirection.Y < 0 ? 1 : 0;
            Sign[2] = InvDirection.Z < 0 ? 1 : 0;
        }
    }
}
