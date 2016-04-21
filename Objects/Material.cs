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
            Color = new Vector3(1.0f, 0.3f, 0.3f)
        };

        public static Material TestSpecularMaterial => new Material
        {
            Color = new Vector3(0, 0, 0.8f),
            Specularity = 0.9f
        };

        public static Material TestRefractiveMaterial => new Material
        {
            Color = new Vector3(1, 1, 1),
            RefractiveIndex = 1.5f,
            Absorption = new Vector3(0.1f)
        };

        public static Material TestLightMaterial => new Material
        {
            Light = 10f,
            Color = Vector3.One
        };

        public static Material TestWhiteMaterial => new Material
        {
            Color = Vector3.One,
            Specularity = 0.4f
        };

        public static Material TestBlackMaterial => new Material
        {
            Color = new Vector3(0.1f, 0.1f, 0.1f),
            Specularity = 0.4f
        };
    }
}
