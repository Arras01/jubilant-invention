using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using OpenTK;
using Template.Objects;

namespace Template
{
    class BVH
    {
        public BVHNode root;
        private BVHNode[] pool;
        private int poolPtr;
        private uint[] indices;
        private List<AABB> AABBs;

        public void ConstructBVH(List<Triangle> primitives)
        {
            AABBs = GetBoundingBoxes(primitives);

            indices = new uint[primitives.Count];
            for (uint i = 0; i < primitives.Count; i++)
            {
                indices[i] = i;
            }

            pool = new BVHNode[2 * primitives.Count - 1];
            for (var i = 0; i < pool.Length; i++)
            {
                pool[i] = new BVHNode();
            }

            root = pool[0];
            poolPtr = 2;

            root.first = 0;
            root.count = primitives.Count;
            root.vertexBounds = CalculateVertexBounds(root.first, root.count);
            root.centroidBounds = CalculateCentroidBounds(root.first, root.count);
            Subdivide(root);
        }

        List<AABB> GetBoundingBoxes(List<Triangle> primitives)
        {
            return primitives.Select(t => new AABB(t)).ToList();
        }

        //There is probably a better way than having 4 bounding box functions that all more or less do the same thing,
        //but I couldn't think of one

        AABB CalculateCentroidBounds(int first, int count)
        {
            float xmin, ymin, zmin, xmax, ymax, zmax;
            xmin = float.PositiveInfinity;
            ymin = float.PositiveInfinity;
            zmin = float.PositiveInfinity;
            xmax = float.NegativeInfinity;
            ymax = float.NegativeInfinity;
            zmax = float.NegativeInfinity;

            for (int i = 0; i < count; i++)
            {
                var prim = AABBs[(int)indices[first + i]];
                xmin = Math.Min(xmin, prim.Centroid.X);
                ymin = Math.Min(ymin, prim.Centroid.Y);
                zmin = Math.Min(zmin, prim.Centroid.Z);
                xmax = Math.Max(xmax, prim.Centroid.X);
                ymax = Math.Max(ymax, prim.Centroid.Y);
                zmax = Math.Max(zmax, prim.Centroid.Z);
            }

            return new AABB(new Vector3(xmin, ymin, zmin), new Vector3(xmax, ymax, zmax));
        }

        AABB[] CalculateBinBounds(int first, int count, float k0, float k1, int bins, Axis axis, out int[] primcounts)
        {
            AABB[] result = new AABB[bins];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = new AABB(
                    new Vector3(float.PositiveInfinity, float.PositiveInfinity, float.PositiveInfinity)
                  , new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity));
            }
            primcounts = new int[bins];

            for (int i = 0; i < count; i++)
            {
                var prim = AABBs[(int)indices[first + i]];
                int binID = 0;
                switch (axis)
                {
                    case Axis.X:
                        binID = (int)(k1 * (prim.Centroid.X - k0));
                        break;
                    case Axis.Y:
                        binID = (int)(k1 * (prim.Centroid.Y - k0));
                        break;
                    case Axis.Z:
                        binID = (int)(k1 * (prim.Centroid.Z - k0));
                        break;
                }
                primcounts[binID]++;
                result[binID].Bounds[0].X = Math.Min(result[binID].Bounds[0].X, prim.Bounds[0].X);
                result[binID].Bounds[0].Y = Math.Min(result[binID].Bounds[0].Y, prim.Bounds[0].Y);
                result[binID].Bounds[0].Z = Math.Min(result[binID].Bounds[0].Z, prim.Bounds[0].Z);
                result[binID].Bounds[1].X = Math.Max(result[binID].Bounds[1].X, prim.Bounds[1].X);
                result[binID].Bounds[1].Y = Math.Max(result[binID].Bounds[1].Y, prim.Bounds[1].Y);
                result[binID].Bounds[1].Z = Math.Max(result[binID].Bounds[1].Z, prim.Bounds[1].Z);
            }

            foreach (var aabb in result)
            {
                aabb.RecalculateCentroid();
            }

            return result;
        }

        AABB SumAABBs(AABB[] source, int first, int count)
        {
            float xmin, ymin, zmin, xmax, ymax, zmax;
            xmin = float.PositiveInfinity;
            ymin = float.PositiveInfinity;
            zmin = float.PositiveInfinity;
            xmax = float.NegativeInfinity;
            ymax = float.NegativeInfinity;
            zmax = float.NegativeInfinity;

            for (int i = 0; i < count; i++)
            {
                var prim = source[first + i];
                xmin = Math.Min(xmin, prim.Bounds[0].X);
                ymin = Math.Min(ymin, prim.Bounds[0].Y);
                zmin = Math.Min(zmin, prim.Bounds[0].Z);
                xmax = Math.Max(xmax, prim.Bounds[1].X);
                ymax = Math.Max(ymax, prim.Bounds[1].Y);
                zmax = Math.Max(zmax, prim.Bounds[1].Z);
            }

            return new AABB(new Vector3(xmin, ymin, zmin), new Vector3(xmax, ymax, zmax));
        }

        AABB CalculateVertexBounds(int first, int count)
        {
            float xmin, ymin, zmin, xmax, ymax, zmax;
            xmin = float.PositiveInfinity;
            ymin = float.PositiveInfinity;
            zmin = float.PositiveInfinity;
            xmax = float.NegativeInfinity;
            ymax = float.NegativeInfinity;
            zmax = float.NegativeInfinity;

            for (int i = 0; i < count; i++)
            {
                var prim = AABBs[(int)indices[first + i]];
                xmin = Math.Min(xmin, prim.Bounds[0].X);
                ymin = Math.Min(ymin, prim.Bounds[0].Y);
                zmin = Math.Min(zmin, prim.Bounds[0].Z);
                xmax = Math.Max(xmax, prim.Bounds[1].X);
                ymax = Math.Max(ymax, prim.Bounds[1].Y);
                zmax = Math.Max(zmax, prim.Bounds[1].Z);
            }

            return new AABB(new Vector3(xmin, ymin, zmin), new Vector3(xmax, ymax, zmax));
        }

        float CalculateSAH(AABB[] binBoxes, int[] triangleCounts, int split)
        {
            var left = SumAABBs(binBoxes, 0, split + 1);
            var right = SumAABBs(binBoxes, split + 1, binBoxes.Length - (split + 1));
            var leftArea = (left.Bounds[1].X - left.Bounds[0].X) * (left.Bounds[1].Y - left.Bounds[0].Y)
                         + (left.Bounds[1].X - left.Bounds[0].X) * (left.Bounds[1].Z - left.Bounds[0].Z)
                         + (left.Bounds[1].Y - left.Bounds[0].Y) * (left.Bounds[1].Z - left.Bounds[0].Z);
            var rightArea = (right.Bounds[1].X - right.Bounds[0].X) * (right.Bounds[1].Y - right.Bounds[0].Y)
                          + (right.Bounds[1].X - right.Bounds[0].X) * (right.Bounds[1].Z - right.Bounds[0].Z)
                          + (right.Bounds[1].Y - right.Bounds[0].Y) * (right.Bounds[1].Z - right.Bounds[0].Z);
            int leftCount = 0, rightCount = 0;
            for (int j = 0; j < split + 1; j++)
            {
                leftCount += triangleCounts[j];
            }
            for (int j = split + 1; j < binBoxes.Length; j++)
            {
                rightCount += triangleCounts[j];
            }

            return leftArea * leftCount + rightArea * rightCount;
        }

        private void Subdivide(BVHNode b)
        {
            if (b.count < 3)
                return;
            b.left = pool[poolPtr++];
            b.right = pool[poolPtr++];
            FindSplit(b);
            Subdivide(b.left);
            Subdivide(b.right);
            b.isLeaf = false;
        }

        public void FindSplit(BVHNode b)
        {
            int bins = 8;

            var cbounds = b.centroidBounds;

            int[] binTriangleCounts = new int[8];

            Axis axis;
            var width = cbounds.Bounds[1].X - cbounds.Bounds[0].X;
            var height = cbounds.Bounds[1].Y - cbounds.Bounds[0].Y;
            var depth = cbounds.Bounds[1].Z - cbounds.Bounds[0].Z;

            if (width > height && width > depth)
                axis = Axis.X;
            else if (height > depth)
                axis = Axis.Y;
            else
                axis = Axis.Z;

            float k0, k1, binsize;

            switch (axis)
            {
                case Axis.X:
                    k0 = cbounds.Bounds[0].X;
                    k1 = bins * (1 - 0.0001f) / (cbounds.Bounds[1].X - cbounds.Bounds[0].X);
                    binsize = (cbounds.Bounds[1].X - cbounds.Bounds[0].X) / bins;
                    break;
                case Axis.Y:
                    k0 = cbounds.Bounds[0].Y;
                    k1 = bins * (1 - 0.0001f) / (cbounds.Bounds[1].Y - cbounds.Bounds[0].Y);
                    binsize = (cbounds.Bounds[1].Y - cbounds.Bounds[0].Y) / bins;
                    break;
                case Axis.Z:
                    k0 = cbounds.Bounds[0].Z;
                    k1 = bins * (1 - 0.0001f) / (cbounds.Bounds[1].Z - cbounds.Bounds[0].Z);
                    binsize = (cbounds.Bounds[1].Z - cbounds.Bounds[0].Z) / bins;
                    break;
                default: throw new InvalidEnumArgumentException();
            }
            AABB[] binBoxes = CalculateBinBounds(b.first, b.count, k0, k1, bins, axis, out binTriangleCounts);
            int best = 0;
            float bestHeuristic = float.MaxValue;

            if (b.count == 11)
                Debugger.Break();

            for (int i = 0; i < bins - 1; i++)
            {
                var SAH = CalculateSAH(binBoxes, binTriangleCounts, i);
                if (SAH < bestHeuristic)
                {
                    bestHeuristic = SAH;
                    best = i;
                }
            }

            int split = Partition((best + 1) * binsize + k0, axis, b);
            b.left.first = b.first;
            b.left.count = split - b.first + 1;
            b.right.first = split;
            b.right.count = b.count - b.left.count;
            b.left.vertexBounds = CalculateVertexBounds(b.left.first, b.left.count);
            b.left.centroidBounds = CalculateCentroidBounds(b.left.first, b.left.count);
            b.right.vertexBounds = CalculateVertexBounds(b.right.first, b.right.count);
            b.right.centroidBounds = CalculateCentroidBounds(b.right.first, b.right.count);
        }

        int Partition(float splitPos, Axis axis, BVHNode b)
        {
            int left = b.first;
            int right = b.first + b.count - 1;

            while (true)
            {
                switch (axis)
                {
                    case Axis.X:
                        while (AABBs[(int)indices[left]].Centroid.X < splitPos)
                            left++;

                        while (AABBs[(int)indices[right]].Centroid.X > splitPos)
                            right--;
                        break;
                    case Axis.Y:
                        while (AABBs[(int)indices[left]].Centroid.Y < splitPos)
                            left++;

                        while (AABBs[(int)indices[right]].Centroid.Y > splitPos)
                            right--;
                        break;
                    case Axis.Z:
                        while (AABBs[(int)indices[left]].Centroid.Z < splitPos)
                            left++;

                        while (AABBs[(int)indices[right]].Centroid.Z > splitPos)
                            right--;
                        break;
                }

                if (left < right)
                {
                    var temp = indices[right];
                    indices[right] = indices[left];
                    indices[left] = temp;
                }
                else
                {
                    return right;
                }
            }
        }

        List<AABB> TrueArray => indices.Select(x => AABBs[(int)x]).ToList();

        enum Axis
        {
            X,
            Y,
            Z
        }
    }
}
