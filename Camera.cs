﻿using System;
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
            Direction = new Vector3(0, 0, -1);
            Distance = 1;
            Vector3 center = Position + Direction * Distance;
            topLeft = center + new Vector3(-1, -1, 0);
            topRight = center + new Vector3(1, -1, 0);
            bottomLeft = center + new Vector3(-1, 1, 0);
            MoveCamera(Matrix4.CreateRotationZ((float) Math.PI));
        }

        //breaks everything
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
                    yield return GenerateRay(u, v, width, height);
                }
            }
        }

        public Tuple<int, int, Ray> GenerateRay(int u, int v, int width, int height)
        {
            Vector3 d = topLeft + (float)u / width * (topRight - topLeft) + (float)v / height * (bottomLeft - topLeft);
            return new Tuple<int, int, Ray>(u, v, new Ray(Position, (d - Position).Normalized()));
        }
    }
}
