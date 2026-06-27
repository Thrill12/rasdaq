using rasdaq.Logging;
using rasdaq.Transformations;

public class Physics
{
    private readonly List<PhysicsBody> _bodies = new();
    internal List<PhysicsBody> Bodies => _bodies;

    /// <summary>
    /// The gravity value to apply to all physics bodies that have ApplyGravity set to true.
    ///
    /// <value>Default: 9.81</value>
    /// </summary>
    public double Gravity { get; set; } = 9.81;

    /// <summary>
    /// Time interval for updates to aim to occur at.
    /// For example, if this is set to 0.01, the game will aim to update every 10 milliseconds (100 updates per second).
    ///
    /// <value>Default: 0.01</value>
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
        // Process each unique pair once
        for (int i = 0; i < _bodies.Count; i++)
        {
            for (int j = i + 1; j < _bodies.Count; j++)
            {
                CheckCollisions(_bodies[i], _bodies[j]);
            }
        }

        for (int i = 0; i < _bodies.Count; i++)
        {
            if (_bodies[i].ApplyGravity)
            {
                ApplyGravity(_bodies[i]);
            }
        }
    }

    /// <summary>
    /// Handles collisions and sends updates. The order is on purpose:
    /// - Check for any collisions that are no longer happening and need to be removed - send their event
    /// - Send collision stay events for collisions that are currently happening
    /// - Check for any new collisions that need to be added - send their event
    /// </summary>
    /// <param name="body">Physics body to check.</param>
    private void CheckCollisions(PhysicsBody bodyA, PhysicsBody bodyB)
    {
        BoxCollider? colliderA = bodyA.Entity?.GetComponent<BoxCollider>();
        BoxCollider? colliderB = bodyB.Entity?.GetComponent<BoxCollider>();

        if (colliderA == null || colliderB == null)
            return;

        bool isColliding = CheckCollision(colliderA, colliderB);
        bool wasColliding = colliderA.Collisions.Contains(colliderB);

        if (wasColliding && !isColliding)
        {
            // Collision ended - notify both sides
            colliderA.Collisions.Remove(colliderB);
            colliderB.Collisions.Remove(colliderA);
            colliderA.OnCollisionExit?.Invoke(colliderB);
            colliderB.OnCollisionExit?.Invoke(colliderA);
        }
        else if (wasColliding && isColliding)
        {
            // Collision ongoing - notify both sides once
            colliderA.OnCollisionStay?.Invoke(colliderB);
            colliderB.OnCollisionStay?.Invoke(colliderA);
        }
        else if (!wasColliding && isColliding)
        {
            // New collision - notify both sides
            colliderA.Collisions.Add(colliderB);
            colliderB.Collisions.Add(colliderA);
            colliderA.OnCollisionEnter?.Invoke(colliderB);
            colliderB.OnCollisionEnter?.Invoke(colliderA);
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
