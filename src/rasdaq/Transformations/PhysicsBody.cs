using rasdaq.Core.ECS;
using OVector2 = OpenTK.Mathematics.Vector2;

namespace rasdaq.Transformations;

public class PhysicsBody : Component
{
    private OVector2 moveOnceVelocity = OVector2.Zero;
    private OVector2 distanceToCover = OVector2.Zero;
    private bool isDistanceToCoverVector = false;
    private OVector2 distanceVelocity = OVector2.Zero;
    private OVector2 distanceCovered;
    private OVector2 _velocity = OVector2.Zero;
    public Vector2 Velocity
    {
        get => (Vector2)_velocity;
        set => _velocity = (OVector2)value;
    }

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
        moveOnceVelocity += (OVector2)velocity;
        _velocity += (OVector2)velocity;
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
        distanceToCover = OVector2.Normalize((OVector2)velocity) * Math.Abs(distance);
        distanceCovered = OVector2.Zero;
        distanceVelocity = (OVector2)velocity;

        isDistanceToCoverVector = thisDirectionOnly;
    }

    /// <summary>
    /// Internally called every frame to get delta movement of entity
    /// </summary>
    /// <param name="elapsedTime">elapsed time since last frame</param>
    /// <returns>delta vector</returns>
    internal OVector2 GetDeltaVector(float elapsedTime)
    {
        var delta = MoveFrameDistance(elapsedTime);
        delta += _velocity * elapsedTime;

        if (moveOnceVelocity != OVector2.Zero)
        {
            _velocity -= moveOnceVelocity;
            moveOnceVelocity = OVector2.Zero;
        }

        return delta;
    }

    private OVector2 MoveFrameDistance(float elapsedTime)
    {
        // if no distance to cover
        if (distanceToCover == OVector2.Zero) { return OVector2.Zero; }

        OVector2 distanceThisFrame = distanceVelocity * elapsedTime;
        OVector2 deltaVector;

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

    private void ProcessRemainingDistance(OVector2 deltaVector, float elapsedTime)
    {
        if (isDistanceToCoverVector)
            // net vector covered this frame
            distanceCovered += deltaVector + (_velocity * elapsedTime);
        else
            // net distance covered this frame, then vectorized
            distanceCovered += (deltaVector + (_velocity * elapsedTime)).Length * OVector2.Normalize(distanceVelocity);

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
        if (distanceToCover == OVector2.Zero)
        {
            distanceCovered = OVector2.Zero;
        }
    }
}
