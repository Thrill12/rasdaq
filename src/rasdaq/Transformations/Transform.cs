using OVector2 = OpenTK.Mathematics.Vector2;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq.Transformations;

public class Transform(Vector3 spawnPosition)
{
    public Vector3 position = spawnPosition;
    public Vector2 scale = new(1, 1);
    public float rotatedDegrees = 0.0f;

    internal void CoordUpdate(OVector2 delta)
    {
        position.X += delta.X;
        position.Y += delta.Y;
    }

    internal Matrix4 Get2DRotation()
    {
        return Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotatedDegrees));
    }

    /// <summary>
    /// Scale from NDC coordinates ((1,1) being top right of image), to image width/height. Then scale further based on user defined.
    /// </summary>
    /// <param name="imageWidth"></param>
    /// <param name="imageHeight"></param>
    /// <returns>Matrix transformation of scale</returns>
    internal Matrix4 ScaleFromNDC(float imageWidth, float imageHeight)
    {
        return Matrix4.CreateScale(imageWidth, imageHeight, 1.0f) * Scale((float)scale.X, (float)scale.Y);
    }

    internal static Matrix4 Scale(float xScale, float yScale)
    {
        return Matrix4.CreateScale(xScale, yScale, 1.0f);
    }

    internal Matrix4 Translate()
    {
        return Matrix4.CreateTranslation((float)position.X, (float)position.Y, (float)position.Z);
    }

    internal Matrix4 GetRenderedTransform(float imageWidth, float imageHeight)
    {
        var translation = Translate();
        var rotation = Get2DRotation();
        var scale = ScaleFromNDC(imageWidth, imageHeight);

        return scale * rotation * translation;
    }
}
