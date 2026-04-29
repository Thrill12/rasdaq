using OpenTK.Mathematics;
using rasdaq.Core.ECS;

namespace rasdaq.Transformations;

public class Transform
{
    private float localX = 0;
    private float localY = 0;
    private float localRotateZ = 0;
    private Vector2 moveOnceVelocity = Vector2.Zero;
    private Vector2 velocity = Vector2.Zero;
    private Vector2 distanceToCover = Vector2.Zero;
    bool isDistanceToCoverVector = false;
    private Vector2 distanceVelocity = Vector2.Zero;
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
        localX += (float)(velocity.X * elapsedTime);
        localY += (float)(velocity.Y * elapsedTime);

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
        if (distanceToCover != Vector2.Zero)
        {
            Vector2 distanceThisFrame;
            if (!isDistanceToCoverVector)
                distanceThisFrame = distanceVelocity * (float)elapsedTime;
            else
            {
                if (velocity == Vector2.Zero)
                    distanceThisFrame = Vector2.Zero;
                else
                    distanceThisFrame = velocity * (float)elapsedTime;
            }
            var deltaX = Math.Abs(distanceToCover.X) >= Math.Abs(distanceThisFrame.X) ? distanceThisFrame.X : distanceToCover.X;
            var deltaY = Math.Abs(distanceToCover.Y) >= Math.Abs(distanceThisFrame.Y) ? distanceThisFrame.Y : distanceToCover.Y;

            localX += deltaX;
            localY += deltaY;

            distanceToCover.X -= deltaX;
            distanceToCover.Y -= deltaY;

            if (distanceToCover == Vector2.Zero)
            {
                velocity -= distanceVelocity;
                distanceVelocity = Vector2.Zero;
            }
        }
    }

    public void MoveDistance(Vector2 velocity, float distance)
    {
        distanceToCover = Vector2.Normalize(velocity) * distance;
        this.velocity = velocity;
        distanceVelocity = velocity;
        isDistanceToCoverVector = false;
    }

    public void MoveVector(Vector2 velocity, float distance)
    {
        distanceToCover = Vector2.Normalize(velocity) * distance;
        this.velocity = velocity;
        distanceVelocity = velocity;
        isDistanceToCoverVector = true;
    }

    private Vector2d scaleFactor = Vector2d.One;

    public void Scale(double xScaleFactor = 1.0, double yScaleFactor = 1.0)
    {
        scaleFactor = new Vector2d(xScaleFactor, yScaleFactor);
    }



    internal Matrix4 _GetTransformation(double elapsedTime)
    {
        SetFrameMovement(elapsedTime);
        return Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(localRotateZ)) *
            Matrix4.CreateScale((float)scaleFactor.X, (float)scaleFactor.Y, 1.0f) *
            Matrix4.CreateTranslation(localX, localY, 0);
    }
}