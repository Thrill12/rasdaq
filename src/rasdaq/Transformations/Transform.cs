using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq.Transformations;

public class Transform
{
    public float WorldX { get; private set; } = 0;
    public float WorldY { get; private set; } = 0;
    private float localRotateZ = 0;
    private Vector2 moveOnceVelocity = Vector2.Zero;
    private Vector2 velocity = Vector2.Zero;
    private Vector2 distanceToCover = Vector2.Zero;
    bool isDistanceToCoverVector = false;
    private Vector2 distanceVelocity = Vector2.Zero;
    private Vector2 distanceCovered;
    private Vector2d scaleFactor = Vector2d.One;

    public void Rotate2D(float degrees)
    {
        localRotateZ = degrees;
    }

    public void MoveOnce(Vector2 velocity)
    {
        moveOnceVelocity += velocity;
        this.velocity += velocity;
        // Console.WriteLine(this.velocity);
    }

    private void SetFrameMovement(double elapsedTime)
    {
        MoveFrameDistance(elapsedTime);
        WorldX += (float)(velocity.X * elapsedTime);
        WorldY += (float)(velocity.Y * elapsedTime);

        if (moveOnceVelocity != Vector2.Zero)
        {
            velocity -= moveOnceVelocity;
            moveOnceVelocity = Vector2.Zero;
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    private void MoveFrameDistance(double elapsedTime)
    {
        // if no distance to cover
        if (distanceToCover == Vector2.Zero) { return; }

        Vector2 distanceThisFrame = distanceVelocity * (float)elapsedTime;
        Vector2 deltaVector;

        // if current distance to cover is LEQ distance to be covered by distance velocity 
        if ((distanceToCover - distanceCovered).LengthSquared <= distanceThisFrame.LengthSquared)
            // traverse remaining distance
            deltaVector = distanceToCover - distanceCovered;
        else
            // move max distance as yet to finish full distance
            deltaVector = distanceThisFrame;

        WorldX += deltaVector.X;
        WorldY += deltaVector.Y;

        if (isDistanceToCoverVector)
            // net vector covered this frame
            distanceCovered += deltaVector + (velocity * (float)elapsedTime);
        else
            // net distance covered this frame, then vectorized
            distanceCovered += (deltaVector + (velocity * (float)elapsedTime)).Length * Vector2.Normalize(distanceVelocity);

        // if we have covered full distance
        if (distanceCovered.X >= distanceToCover.X && distanceCovered.Y >= distanceToCover.Y)
        {
            distanceVelocity = Vector2.Zero;
            distanceToCover = Vector2.Zero;
            distanceCovered = Vector2.Zero;
        }
    }

    public void MoveDistance(Vector2 velocity, float distance)
    {
        distanceToCover = Vector2.Normalize(velocity) * distance;
        distanceCovered = Vector2.Zero;
        distanceVelocity = velocity;
        isDistanceToCoverVector = false;
    }

    public void MoveVector(Vector2 velocity, float distance)
    {
        distanceToCover = Vector2.Normalize(velocity) * distance;
        distanceCovered = Vector2.Zero;
        distanceVelocity = velocity;
        isDistanceToCoverVector = true;
    }

    public void Scale(double xScaleFactor = 1.0, double yScaleFactor = 1.0)
    {
        scaleFactor = new Vector2d(xScaleFactor, yScaleFactor);
    }

    internal Matrix4 _GetTransformation(double elapsedTime)
    {
        SetFrameMovement(elapsedTime);
        return Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(localRotateZ)) *
            Matrix4.CreateScale((float)scaleFactor.X, (float)scaleFactor.Y, 1.0f) *
            Matrix4.CreateTranslation(WorldX, WorldY, 0);
    }
}