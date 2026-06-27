using rasdaq.Core.ECS;
using rasdaq.Logging;
using OVector2 = OpenTK.Mathematics.Vector2;

namespace rasdaq.Transformations;

/// <summary>
/// Component that allows entity to move with physics properties like velocity and distance-based movement
/// </summary>
public class PhysicsBody : Component
{
    private OVector2 _moveOnceVelocity = OVector2.Zero;
    private OVector2 _distanceToCover = OVector2.Zero;
    private bool _isDistanceToCoverVector = false;
    private OVector2 _distanceVelocity = OVector2.Zero;
    private OVector2 _distanceCovered;
    private OVector2 _velocity = OVector2.Zero;

    /// <summary>
    /// Whether the entity is affected by gravity.
    /// </summary>
    public bool ApplyGravity { get; set; } = true;

    /// <summary>
    /// Constant velocity of the entity. This velocity is applied every physics update
    /// </summary>
    public Vector2 Velocity
    {
        get => (Vector2)_velocity;
        set => _velocity = (OVector2)value;
    }

    public override void Start()
    {
        World.Physics.AddBody(this);
    }

    public override void Destroy()
    {
        base.Destroy();
        World.Physics.RemoveBody(this);
    }

    public override void Update(double deltaTime)
    {
        var delta = GetDeltaVector((float)deltaTime);
        Entity?.Transform.CoordUpdate(delta);
    }

    /// <summary>
    /// Move entity once this frame, at specified velocity. Ideal for input based movement.
    /// Note that if velocity property is also set, this velocity adds on to it
    /// </summary>
    /// <param name="velocity">velocity of the entity this frame</param>
    public void MoveOnce(Vector2 velocity)
    {
        _moveOnceVelocity += (OVector2)velocity;
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
        _distanceToCover = OVector2.Normalize((OVector2)velocity) * Math.Abs(distance);
        _distanceCovered = OVector2.Zero;
        _distanceVelocity = (OVector2)velocity;

        _isDistanceToCoverVector = thisDirectionOnly;
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

        if (_moveOnceVelocity != OVector2.Zero)
        {
            _velocity -= _moveOnceVelocity;
            _moveOnceVelocity = OVector2.Zero;
        }

        return delta;
    }

    private OVector2 MoveFrameDistance(float elapsedTime)
    {
        // if no distance to cover
        if (_distanceToCover == OVector2.Zero)
        {
            return OVector2.Zero;
        }

        OVector2 distanceThisFrame = _distanceVelocity * elapsedTime;
        OVector2 deltaVector;

        // if current distance to cover is LEQ distance to be covered by distance velocity
        if ((_distanceToCover - _distanceCovered).LengthSquared <= distanceThisFrame.LengthSquared)
            // traverse remaining distance
            deltaVector = _distanceToCover - _distanceCovered;
        else
            // move max distance as yet to finish full distance
            deltaVector = distanceThisFrame;

        ProcessRemainingDistance(deltaVector, elapsedTime);

        return deltaVector;
    }

    private void ProcessRemainingDistance(OVector2 deltaVector, float elapsedTime)
    {
        if (_isDistanceToCoverVector)
            // net vector covered this frame
            _distanceCovered += deltaVector + (_velocity * elapsedTime);
        else
            // net distance covered this frame, then vectorized
            _distanceCovered +=
                (deltaVector + (_velocity * elapsedTime)).Length
                * OVector2.Normalize(_distanceVelocity);

        if (
            _distanceToCover.X > 0 && _distanceCovered.X >= _distanceToCover.X
            || _distanceToCover.X < 0 && _distanceCovered.X <= _distanceToCover.X
        )
        {
            _distanceVelocity.X = 0;
            _distanceToCover.X = 0;
        }
        if (
            _distanceToCover.Y > 0 && _distanceCovered.Y >= _distanceToCover.Y
            || _distanceToCover.Y < 0 && _distanceCovered.Y <= _distanceToCover.Y
        )
        {
            _distanceVelocity.Y = 0;
            _distanceToCover.Y = 0;
        }

        // if we have covered full distance
        if (_distanceToCover == OVector2.Zero)
        {
            _distanceCovered = OVector2.Zero;
        }
    }
}
