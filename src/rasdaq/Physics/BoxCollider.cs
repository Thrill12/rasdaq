using rasdaq.Core.ECS;

public class BoxCollider : Component
{
    public float Width { get; set; }
    public float Height { get; set; }
    public List<BoxCollider> Collisions { get; internal set; } = new();

    public BoxCollider(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public bool IsCollidingWith(BoxCollider other)
    {
        return Collisions.Contains(other);
    }
}
