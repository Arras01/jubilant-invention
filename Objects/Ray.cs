using OpenTK;

namespace Template.Objects
{
    public class Ray
    {
        public readonly Vector3 Origin;
        public readonly Vector3 Direction;
        public float? NearestIntersection = null;
        public Vector3 IntersectionNormal;
        public ISceneObject IntersectedObject;

        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }
    }
}
