using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Template.Objects
{
    interface ISceneObject
    {
        void Intersect(Ray r);
    }
}
