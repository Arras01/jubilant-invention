using OpenTK;

namespace Template.Objects
{
    public class PointLight
    {
        public Vector3 Location;
        public readonly float Intensity;

        public PointLight(Vector3 location, float intensity)
        {
            Location = location;
            Intensity = intensity;
        }
    }
}
