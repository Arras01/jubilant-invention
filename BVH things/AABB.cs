using System;
using OpenTK;
using Template.Objects;

namespace Template
{
    public class AABB
    {
        public readonly Vector3[] Bounds = new Vector3[2];
        public Vector3 Centroid;

        public AABB(Triangle t)
        {
            Bounds[0] = new Vector3(Math.Min(Math.Min(t.V1.X, t.V2.X), t.V3.X)
                        , Math.Min(Math.Min(t.V1.Y, t.V2.Y), t.V3.Y)
                        , Math.Min(Math.Min(t.V1.Z, t.V2.Z), t.V3.Z));
            Bounds[1] = new Vector3(Math.Max(Math.Max(t.V1.X, t.V2.X), t.V3.X)
                        , Math.Max(Math.Max(t.V1.Y, t.V2.Y), t.V3.Y)
                        , Math.Max(Math.Max(t.V1.Z, t.V2.Z), t.V3.Z));
            RecalculateCentroid();
        }

        public AABB(Vector3 vmin, Vector3 vmax)
        {
            Bounds[0] = vmin;
            Bounds[1] = vmax;
            RecalculateCentroid();
        }

        public void RecalculateCentroid()
        {
            Centroid = (Bounds[0] + Bounds[1]) / 2;
        }


        public float Intersect(Ray r)
        {
            float tmin, tmax, tymin, tymax, tzmin, tzmax;

            tmin = (Bounds[r.Sign[0]].X - r.Origin.X) * r.InvDirection.X;
            tmax = (Bounds[1 - r.Sign[0]].X - r.Origin.X) * r.InvDirection.X;
            tymin = (Bounds[r.Sign[1]].Y - r.Origin.Y) * r.InvDirection.Y;
            tymax = (Bounds[1 - r.Sign[1]].Y - r.Origin.Y) * r.InvDirection.Y;

            if ((tmin > tymax) || (tymin > tmax))
                return -1;
            if (tymin > tmin)
                tmin = tymin;
            if (tymax < tmax)
                tmax = tymax;

            tzmin = (Bounds[r.Sign[2]].Z - r.Origin.Z) * r.InvDirection.Z;
            tzmax = (Bounds[1 - r.Sign[2]].Z - r.Origin.Z) * r.InvDirection.Z;

            if ((tmin > tzmax) || (tzmin > tmax))
                return -1;
            if (tzmin > tmin)
                tmin = tzmin;
            if (tzmax < tmax)
                tmax = tzmax;

            return tmin;
        }

        public float Surface => (Bounds[1].X - Bounds[0].X) * (Bounds[1].Y - Bounds[0].Y)
                              + (Bounds[1].X - Bounds[0].X) * (Bounds[1].Z - Bounds[0].Z)
                              + (Bounds[1].Y - Bounds[0].Y) * (Bounds[1].Z - Bounds[0].Z);
    }
}
