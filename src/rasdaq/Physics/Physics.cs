using rasdaq.Logging;
using rasdaq.Transformations;

public class Physics
{
    private readonly List<PhysicsBody> _bodies = new();

    public double Gravity { get; set; } = 9.81;

    /// <summary>
    /// Time interval for updates to aim to occur at.
    /// For example, if this is set to 0.01, the game will aim to update every 10 milliseconds (100 updates per second).
    /// </summary>
    public double FixedUpdateTime { get; set; } = 0.01f;

    internal void AddBody(PhysicsBody body)
    {
        _bodies.Add(body);
    }

    internal void RemoveBody(PhysicsBody body)
    {
        _bodies.Remove(body);
    }

    internal void Update()
    {
        for (int i = 0; i < _bodies.Count; i++)
        {
            PhysicsBody? body = _bodies[i];
            if (body == null)
            {
                continue;
            }

            CheckCollisions(body);

            if (body.ApplyGravity)
            {
                ApplyGravity(body);
            }
        }
    }

    private void CheckCollisions(PhysicsBody body)
    {
        // Update collisions for each physics body which has a BoxCollider
        BoxCollider? collider = body.Entity?.GetComponent<BoxCollider>();

        if (collider == null)
        {
            return;
        }

        collider.Collisions.Clear();

        for (int j = 0; j < _bodies.Count; j++)
        {
            PhysicsBody? otherBody = _bodies[j];

            if (otherBody == body)
            {
                continue;
            }

            BoxCollider? otherCollider = otherBody.Entity?.GetComponent<BoxCollider>();

            if (otherCollider != null && CheckCollision(collider, otherCollider))
            {
                collider.Collisions.Add(otherCollider);
            }
        }
    }

    private void ApplyGravity(PhysicsBody body)
    {
        body.Velocity = new Vector2(
            body.Velocity.X,
            body.Velocity.Y - (float)(Gravity * FixedUpdateTime)
        );
    }

    private static bool CheckCollision(BoxCollider a, BoxCollider b)
    {
        Vector2 aPos = a.Entity.Transform.position;
        Vector2 bPos = b.Entity.Transform.position;

        return (Math.Abs(aPos.X - bPos.X) * 2 < (a.Width + b.Width))
            && (Math.Abs(aPos.Y - bPos.Y) * 2 < (a.Height + b.Height));
    }
}
