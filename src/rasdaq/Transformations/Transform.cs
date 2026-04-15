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

    public void Move(float x, float y)
    {
        localX += x;
        localY += y;

        Matrix4 trans = Matrix4.CreateTranslation(localX, localY, 0);

        finalTransformation *= trans;


        System.Console.WriteLine(localX + " and " + localY);

        // shader.SetUniform("transform", trans, true);
    }

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

    internal Matrix4 GetTransformation()
    {
        return
            Rotate(Matrix4.CreateRotationY, localRotateY) *
            Matrix4.CreateTranslation(localX, localY, 0);
    }
}