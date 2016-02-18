using System;
using System.Collections.Generic;
using OpenTK;
using Template.Objects;

namespace Template
{
    class Camera
    {
        public Vector3 Position;
        public Vector3 Direction;
        public float Distance;

        Vector3 topLeft;
        Vector3 topRight;
        Vector3 bottomLeft;


        public Camera()
        {
            Position = new Vector3();
            Direction = new Vector3(1, 0, 0);
            Distance = 1;
            Vector3 center = Position + Direction * Distance;
            topLeft = center + new Vector3(-1, -1, 0);
            topRight = center + new Vector3(1, -1, 0);
            bottomLeft = center + new Vector3(-1, 1, 0);
        }

        public void MoveCamera(Matrix4 m)
        {
            Position = Vector3.TransformPosition(Position, m);
            topLeft = Vector3.TransformPosition(topLeft, m);
            topRight = Vector3.TransformPosition(topRight, m);
            bottomLeft = Vector3.TransformPosition(bottomLeft, m);
        }

        public IEnumerable<Tuple<int, int, Ray>> GenerateRays(int width, int height)
        {
            for (int u = 0; u < width; u++)
            {
                for (int v = 0; v < height; v++)
                {
                    Vector3 d = topLeft + (float)u / width * (topRight - topLeft) + (float)v / height * (bottomLeft - topLeft);
                    yield return new Tuple<int, int, Ray>(u, v, new Ray(Position, (d - Position).Normalized()));
                }
            }
        }
    }
}
