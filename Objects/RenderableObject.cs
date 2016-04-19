namespace Template.Objects
{
    public abstract class RenderableObject
    {
        public abstract bool Intersect(Ray r);
        public Material Material;
        public abstract AABB GetAABB();
    }
}
