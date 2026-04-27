using OpenTK.Mathematics;
using rasdaq.Core.ECS;

namespace rasdaq.Transformations;

public class Transform
{
    private float localX = 0;
    private float localY = 0;
    private float localRotateX = 0;
    private float localRotateY = 0;
    private float localRotateZ = 0;
    internal Matrix4 finalTransformation = Matrix4.Identity;

    private void RotateZ(float degrees)
    {
        localRotateZ += degrees;
        Rotate(Matrix4.CreateRotationZ, localRotateZ);
    }

    public void Rotate2D(float degrees)
    {
        RotateZ(degrees);
    }

    public void RotateY(float degrees)
    {
        localRotateY += degrees;
        Rotate(Matrix4.CreateRotationY, localRotateY);
    }

    public void RotateX(float degrees)
    {
        localRotateX += degrees;
        Rotate(Matrix4.CreateRotationX, localRotateX);

    }

    private Matrix4 Rotate(Func<float, Matrix4> rotate, float degrees)
    {
        Matrix4 trans = rotate(MathHelper.DegreesToRadians(degrees));
        return trans;
        // finalTransformation *= trans;
        // System.Console.WriteLine(finalTransformation);
    }

    private Vector2 moveOnceVelocity = Vector2.Zero;
    private Vector2 velocity = Vector2.Zero;

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

    Vector2 distanceToCover = Vector2.Zero;

    private void MoveFrameDistance(double elapsedTime)
    {
        if (distanceToCover != Vector2.Zero)
        {
            Console.WriteLine(distanceToCover);
            var distanceThisFrame = Vector2.Normalize(distanceVelocity) * (float)elapsedTime;
            Console.WriteLine(velocity);
            Console.WriteLine(distanceThisFrame);
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

    private Vector2 distanceVelocity = Vector2.Zero;
    public void MoveDistance(Vector2 velocity, float distance)
    {
        distanceToCover = Vector2.Normalize(velocity) * distance;
        this.velocity = velocity;
        this.distanceVelocity = velocity;
    }

    internal Matrix4 _GetTransformation(double elapsedTime)
    {
        SetFrameMovement(elapsedTime);
        return Matrix4.CreateTranslation(localX / 1920 * 2f, localY / 1080 * 2f, 0);
    }
}