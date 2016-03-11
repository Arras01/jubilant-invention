using OpenTK;

namespace Template.Objects
{
    public class Material
    {
        public Vector3 Color;
        public float Specularity;
        public float Light;
        public float RefractiveIndex;

        public bool IsLight => Light > 0;

        public static Material TestDiffuseMaterial => new Material
        {
            Color = new Vector3(1f, 0f, 0f)
        };

        public static Material TestSpecularMaterial => new Material
        {
            Color = Vector3.One,
            Specularity = 0.5f
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
