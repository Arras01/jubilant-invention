using OpenTK;

namespace Template.Objects
{
    public class Material
    {
        public Vector3 Color;
        public float Specularity;
        public float Light;
        public float RefractiveIndex;
        public Vector3 Absorption;

        public bool IsLight => Light > 0;

        public static Material TestDiffuseMaterial => new Material
        {
            Color = new Vector3(1f, 0f, 0f)
        };

        public static Material TestSpecularMaterial => new Material
        {
            Color = Vector3.One,
            Specularity = 1f
        };

        public static Material TestRefractiveMaterial => new Material
        {
            Color = new Vector3(1, 1, 1),
            RefractiveIndex = 1.5f,
            Absorption = new Vector3(0.1f)
        };

        public static Material TestWhiteMaterial => new Material
        {
            Color = Vector3.One,
            Specularity = 0.1f
        };

        public static Material TestBlackMaterial => new Material
        {
            Specularity = 0.1f
        };
    }
}
