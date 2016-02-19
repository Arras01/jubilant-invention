﻿using System;
using System.Drawing;
using OpenTK;
using Template.Objects;

namespace Template
{
    class WhittedRenderer : Renderer
    {
        public override Color Trace(Ray r)
        {
            Scene.BruteForceFindNearestIntersection(r);
            if (r.NearestIntersection == null)
                return Color.Black;
            return HelperFunctions.MultiplyColor(Color.Red,
                DirectIllumination(r.NearestIntersection.Value * r.Direction, r.IntersectionNormal));
        }

        private float DirectIllumination(Vector3 intersection, Vector3 normal)
        {
            float result = 0;
            foreach (var pointLight in Scene.PointLights)
            {
                if (Scene.BruteForceCheckFreePath(intersection, pointLight.Location))
                    result += pointLight.Intensity
                              * (1 / (float)Math.Pow((intersection - pointLight.Location).Length, 2))
                              * Math.Abs(Vector3.Dot(normal, (intersection - pointLight.Location).Normalized()));
            }
            return result < 1 ? result : 1;
        }
    }
}