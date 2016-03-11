using System.Collections.Generic;
using System.Linq;
using FileFormatWavefront;
using FileFormatWavefront.Model;
using OpenTK;
using Template.Objects;
using Scene = Template.Objects.Scene;

namespace Template
{
    public static class ObjLoader
    {
        public static Scene LoadScene(string objName)
        {
            Scene s = new Scene();
            var result = FileFormatObj.Load(objName, false);
            var allFaces = result.Model.Groups.SelectMany(g => g.Faces).Concat(result.Model.UngroupedFaces);
            s.Objects = new List<RenderableObject>(allFaces.Select(f => ConvertFaceToTriangle(f, result.Model.Vertices)));
            return s;
        }

        private static Triangle ConvertFaceToTriangle(Face f, IReadOnlyList<Vertex> vertices)
        {
            return new Triangle(ConvertVertexToVector3(vertices[f.Indices[0].vertex]),
                                      ConvertVertexToVector3(vertices[f.Indices[1].vertex]),
                                      ConvertVertexToVector3(vertices[f.Indices[2].vertex]));
        }

        private static Vector3 ConvertVertexToVector3(Vertex v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
    }
}
