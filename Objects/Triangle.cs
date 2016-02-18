using OpenTK;

namespace Template.Objects
{
    class Triangle : ISceneObject
    {
        public readonly Vector3 V1;
        public readonly Vector3 V2;
        public readonly Vector3 V3;

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
        }

        public bool Intersect(Ray r)
        {
            //Find vectors for two edges sharing V1
            var e1 = V2 - V1;
            var e2 = V3 - V1;

            //Begin calculating determinant - also used to calculate u parameter
            var p = Vector3.Cross(r.Direction, e2);

            //if determinant is near zero, ray lies in plane of triangle
            var det = Vector3.Dot(e1, p);

            //NOT CULLING
            if (det > -float.Epsilon && det < float.Epsilon) return false;
            var invDet = 1f / det;

            //calculate distance from V1 to ray origin
            var T = r.Origin - V1;

            //Calculate u parameter and test bound
            var u = Vector3.Dot(T, p) * invDet;

            //The intersection lies outside of the triangle
            if (u < 0f || u > 1f) return false;

            //Prepare to test v parameter
            var Q = Vector3.Cross(T, e1);

            //Calculate V parameter and test bound
            var v = Vector3.Dot(r.Direction, Q) * invDet;

            //The intersection lies outside of the triangle
            if (v < 0f || u + v > 1f) return false;

            var t = Vector3.Dot(e2, Q) * invDet;

            if (t <= float.Epsilon) return false; //ray intersection

            if (t < r.NearestIntersection)
            {
                r.NearestIntersection = t;
                r.IntersectedObject = this;
            }
            return true;
        }
    }
}
