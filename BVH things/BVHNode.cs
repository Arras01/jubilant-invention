using System.Collections.Generic;
using Template.Objects;

namespace Template
{
    public class BVHNode
    {
        public AABB vertexBounds;
        public AABB centroidBounds;
        public bool isLeaf = true;
        public BVHNode left, right;
        public int first, count;

        public void Traverse(Ray r, List<Triangle> primitives, BVH bvh)
        {
            if (!vertexBounds.Intersect(r))
                return;
            if (isLeaf)
            {
                IntersectPrimitives(r, primitives, bvh);
            }
            else
            {
                left.Traverse(r, primitives, bvh);
                right.Traverse(r, primitives, bvh);
            }
        }

        public void IntersectPrimitives(Ray r, List<Triangle> primitives, BVH bvh)
        {
            for (int i = 0; i < count; i++)
            {
                primitives[(int)bvh.Indices[i + first]].Intersect(r);
            }
        }
    }
}
