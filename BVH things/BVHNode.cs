using System.Collections.Generic;
using Template.Objects;

namespace Template
{
    public class BVHNode
    {
        public AABB vertexBounds;
        public AABB centroidBounds;
        public bool isLeaf = true;
        public int left;
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
                leftNode(bvh).Traverse(r, primitives, bvh);
                rightNode(bvh).Traverse(r, primitives, bvh);
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
                return leftNode(bvh).CheckForAnyCollision(r, primitives, bvh) 
                    || rightNode(bvh).CheckForAnyCollision(r, primitives, bvh);
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

        public BVHNode leftNode(BVH bvh)
        {
            return bvh.Pool[left];
        }

        public BVHNode rightNode(BVH bvh)
        {
            return bvh.Pool[left+1];
        }
    }
}
