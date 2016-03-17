namespace Template
{
    class BVHNode
    {
        public AABB vertexBounds;
        public AABB centroidBounds;
        public bool isLeaf = true;
        public BVHNode left, right;
        public int first, count;
    }
}
