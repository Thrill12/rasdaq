using rasdaq.Core.ECS;

public class BoxCollider : Component
{
    public float Width { get; set; }
    public float Height { get; set; }
    public List<BoxCollider> Collisions { get; internal set; } = new();

    public Action<BoxCollider>? OnCollisionEnter { get; set; }
    public Action<BoxCollider>? OnCollisionExit { get; set; }
    public Action<BoxCollider>? OnCollisionStay { get; set; }

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
