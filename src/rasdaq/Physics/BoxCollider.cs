using rasdaq.Core.ECS;

public class BoxCollider : Component
{
    public float Width { get; set; }
    public float Height { get; set; }

    /// <summary>
    /// List of colliders that this collider is currently colliding with. This list is updated every physics update.
    /// </summary>
    public List<BoxCollider> Collisions { get; internal set; } = new();

    /// <summary>
    /// Event triggered when a collision is started.
    /// </summary>
    public Action<BoxCollider>? OnCollisionEnter { get; set; }

    /// <summary>
    /// Event triggered when a collision is ended.
    /// </summary>
    public Action<BoxCollider>? OnCollisionExit { get; set; }

    /// <summary>
    /// Event triggered when a collision is ongoing.
    /// </summary>
    public Action<BoxCollider>? OnCollisionStay { get; set; }

    public BoxCollider(float width, float height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// Checks if this collider is currently colliding with another.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool IsCollidingWith(BoxCollider other)
    {
        return Collisions.Contains(other);
    }
}
