using OpenTK.Mathematics;
using rasdaq.Core.ECS;

namespace rasdaq.Transformations;

public class PhysicsBody : Component
{
    private Vector2 moveOnceVelocity = Vector2.Zero;
    private Vector2 distanceToCover = Vector2.Zero;
    private bool isDistanceToCoverVector = false;
    private Vector2 distanceVelocity = Vector2.Zero;
    private Vector2 distanceCovered;
    public Vector2 Velocity { get; set; } = Vector2.Zero;

    public override void Update(double deltaTime)
    {
        var delta = GetDeltaVector((float)deltaTime);
        Entity?.Transform.CoordUpdate(delta);
    }

    /// <summary>
    /// Move entity once this frame, at specified velocity. Ideal for input based movement.
    /// Note that if velocity property is also set, then this velocity adds on to it
    /// </summary>
    /// <param name="velocity">velocity of the entity this frame</param>
    public void MoveOnce(Vector2 velocity)
    {
        moveOnceVelocity += velocity;
        Velocity += velocity;
    }

    /// <summary>
    /// Moves the entity until it has traveled the specified distance
    /// </summary>
    /// <param name="velocity">speed and direction of entity</param>
    /// <param name="distance">how much distance the entity covers</param>
    /// <param name="thisDirectionOnly">
    /// If true, only movement in the same direction as <paramref name="velocity"/>
    /// counts toward the traveled distance, and movement in other directions is ignored.
    /// </param>
    public void MoveDistance(Vector2 velocity, float distance, bool thisDirectionOnly)
    {
        distanceToCover = Vector2.Normalize(velocity) * Math.Abs(distance);
        distanceCovered = Vector2.Zero;
        distanceVelocity = velocity;

        isDistanceToCoverVector = thisDirectionOnly;
    }

    /// <summary>
    /// Internally called every frame to get delta movement of entity
    /// </summary>
    /// <param name="elapsedTime">elapsed time since last frame</param>
    /// <returns>delta vector</returns>
    internal Vector2 GetDeltaVector(float elapsedTime)
    {
        var delta = MoveFrameDistance(elapsedTime);
        delta += Velocity * elapsedTime;

        if (moveOnceVelocity != Vector2.Zero)
        {
            Velocity -= moveOnceVelocity;
            moveOnceVelocity = Vector2.Zero;
        }

        return delta;
    }

    private Vector2 MoveFrameDistance(float elapsedTime)
    {
        // if no distance to cover
        if (distanceToCover == Vector2.Zero) { return Vector2.Zero; }

        Vector2 distanceThisFrame = distanceVelocity * elapsedTime;
        Vector2 deltaVector;

        // if current distance to cover is LEQ distance to be covered by distance velocity 
        if ((distanceToCover - distanceCovered).LengthSquared <= distanceThisFrame.LengthSquared)
            // traverse remaining distance
            deltaVector = distanceToCover - distanceCovered;
        else
            // move max distance as yet to finish full distance
            deltaVector = distanceThisFrame;

        ProcessRemainingDistance(deltaVector, elapsedTime);

        return deltaVector;
    }

    private void ProcessRemainingDistance(Vector2 deltaVector, float elapsedTime)
    {
        if (isDistanceToCoverVector)
            // net vector covered this frame
            distanceCovered += deltaVector + (Velocity * elapsedTime);
        else
            // net distance covered this frame, then vectorized
            distanceCovered += (deltaVector + (Velocity * elapsedTime)).Length * Vector2.Normalize(distanceVelocity);

        System.Console.WriteLine((distanceCovered));

        if (
            distanceToCover.X > 0 && distanceCovered.X >= distanceToCover.X ||
            distanceToCover.X < 0 && distanceCovered.X <= distanceToCover.X
        )
        {
            distanceVelocity.X = 0;
            distanceToCover.X = 0;
        }
        if (
            distanceToCover.Y > 0 && distanceCovered.Y >= distanceToCover.Y ||
            distanceToCover.Y < 0 && distanceCovered.Y <= distanceToCover.Y
        )
        {
            distanceVelocity.Y = 0;
            distanceToCover.Y = 0;
        }

        // if we have covered full distance
        if (distanceToCover == Vector2.Zero)
        {
            distanceCovered = Vector2.Zero;
        }
    }
}