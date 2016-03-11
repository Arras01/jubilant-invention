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

        public static Material TestDiffuseMaterial => new Material
        {
            Color = new Vector3(1f, 0f, 0f)
        };

        public static Material TestSpecularMaterial => new Material
        {
            Color = Vector3.One,
            Specularity = 1
        };
    }
}
