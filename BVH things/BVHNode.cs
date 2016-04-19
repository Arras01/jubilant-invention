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

        public void Traverse(Ray r, List<RenderableObject> primitives, BVH bvh)
        {
            /*if (!vertexBounds.Intersect(r))
                return;*/
            if (isLeaf)
            {
                IntersectPrimitives(r, primitives, bvh);
            }
            else
            {
                var leftHit = leftNode(bvh).vertexBounds.Intersect(r);
                var rightHit = rightNode(bvh).vertexBounds.Intersect(r);
                if (leftHit < rightHit)
                {
                    if (leftHit > 0)
                        leftNode(bvh).Traverse(r, primitives, bvh);
                    if (rightHit > 0)
                        rightNode(bvh).Traverse(r, primitives, bvh);
                }
                else
                {
                    if (rightHit > 0)
                        rightNode(bvh).Traverse(r, primitives, bvh);
                    if (leftHit > 0)
                        leftNode(bvh).Traverse(r, primitives, bvh);
                }
            }
        }

        public bool CheckForAnyCollision(Ray r, List<RenderableObject> primitives, BVH bvh)
        {
            if (vertexBounds.Intersect(r) < 0.0001f)
                return false;
            if (isLeaf)
            {
                return IntersectPrimitives(r, primitives, bvh);
            }
            else
            {
                var leftHit = leftNode(bvh).vertexBounds.Intersect(r);
                var rightHit = rightNode(bvh).vertexBounds.Intersect(r);
                bool result = false;
                if (leftHit < rightHit)
                {
                    if (leftHit > 0)
                        result = leftNode(bvh).CheckForAnyCollision(r, primitives, bvh);
                    if (result)
                        return true;
                    return rightHit > 0 && rightNode(bvh).CheckForAnyCollision(r, primitives, bvh);
                }
                else
                {
                    if (rightHit > 0)
                        result = rightNode(bvh).CheckForAnyCollision(r, primitives, bvh);
                    if (result)
                        return true;
                    return leftHit > 0 && leftNode(bvh).CheckForAnyCollision(r, primitives, bvh);
                }
            }
        }

        public bool IntersectPrimitives(Ray r, List<RenderableObject> primitives, BVH bvh)
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
            return bvh.Pool[left + 1];
        }
    }
}
