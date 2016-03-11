using OpenTK;

namespace Template.Objects
{
    public class Material
    {
        public Vector3 Color;
        public float Specularity;
        public float Light;
        public float Refraction;

        public bool IsLight => Light > 0;

        public static Material TestDiffuseMaterial => new Material()
        {
            Color = new Vector3(255, 20, 30)
        };
    }
}
