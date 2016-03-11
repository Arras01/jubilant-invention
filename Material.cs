using OpenTK;

namespace Template.Objects
{
    class Material
    {
        Vector3 Color;
        float specularity;
        float light;
        float refractionThing;

        public bool IsLight => light > 0;
    }
}
