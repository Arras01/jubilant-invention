namespace Template.Objects
{
    public interface ISceneObject
    {
        bool Intersect(Ray r);
    }
}
