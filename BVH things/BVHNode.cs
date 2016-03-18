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

        public bool CheckForAnyCollision(Ray r, List<Triangle> primitives, BVH bvh)
        {
            if (!vertexBounds.Intersect(r))
                return false;
            if (isLeaf)
            {
                return IntersectPrimitives(r, primitives, bvh);
            }
            else
            {
                return left.CheckForAnyCollision(r, primitives, bvh) 
                    || right.CheckForAnyCollision(r, primitives, bvh);
            }
        }

        public bool IntersectPrimitives(Ray r, List<Triangle> primitives, BVH bvh)
        {
            bool result = false;
            for (int i = 0; i < count; i++)
            {
                result = result || primitives[(int)bvh.Indices[i + first]].Intersect(r);
            }
            return result;
        }
    }
}
