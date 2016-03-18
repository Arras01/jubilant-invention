using System;
using OpenTK;

namespace Template.Objects
{
    public class Triangle : RenderableObject
    {
        public readonly Vector3 V1;
        public readonly Vector3 V2;
        public readonly Vector3 V3;
        private readonly float d;
        readonly Vector3 normal;

        public Triangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            V1 = v1;
            V2 = v2;
            V3 = v3;
            Vector3 v0v1 = v2 - v1;
            Vector3 v0v2 = v3 - v1;
            // no need to normalize
            normal = Vector3.Cross(v0v1, v0v2);
            //normal = Vector3.Cross(v2 - v1, v3 - v1).Normalized();
            d = Vector3.Dot(normal, V1);
        }

        public override bool Intersect(Ray r)
        {
            // Step 1: finding P

            // check if ray and plane are parallel ?
            float NdotRayDirection = Vector3.Dot(normal, r.Direction);
            if (Math.Abs(NdotRayDirection) < float.Epsilon) // almost 0
                return false; // they are parallel so they don't intersect !

            // compute t (equation 3)
            float t = (Vector3.Dot(normal, r.Origin) + d) / NdotRayDirection;
            // check if the triangle is in behind the ray
            if (t < 0.0001f) return false; // the triangle is behind

            // compute the intersection point using equation 1
            Vector3 P = r.Origin + t * r.Direction;

            // Step 2: inside-outside test
            Vector3 C; // vector perpendicular to triangle's plane

            // edge 0
            Vector3 edge0 = V2 - V1;
            Vector3 vp0 = P - V1;
            C = Vector3.Cross(edge0, vp0);
            if (Vector3.Dot(normal, C) < 0) return false; // P is on the right side

            // edge 1
            Vector3 edge1 = V3 - V2;
            Vector3 vp1 = P - V2;
            C = Vector3.Cross(edge1, vp1);
            if (Vector3.Dot(normal, C) < 0) return false; // P is on the right side

            // edge 2
            Vector3 edge2 = V1 - V3;
            Vector3 vp2 = P - V3;
            C = Vector3.Cross(edge2, vp2);
            if (Vector3.Dot(normal, C) < 0) return false; // P is on the right side;

            if (t < r.NearestIntersection)
            {
                r.NearestIntersection = t;
                r.IntersectedMaterial = Material;
                r.IntersectionNormal = normal;
            }

            return true; // this ray hits the triangle 
        }

        /*public override bool Intersect(Ray r)
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
                r.IntersectedMaterial = Material;
                r.IntersectionNormal = normal;
            }
            return true;
        }*/
    }
}
