using OpenTK.Mathematics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq.Transformations;

public class Transform(float x, float y, float z)
{
    public float x = x;
    public float y = y;
    public float zOrdering = z;
    public float scaleX = 1.0f;
    public float scaleY = 1.0f;
    public float rotatedDegrees = 0.0f;

    internal void CoordUpdate(Vector2 delta)
    {
        x += delta.X;
        y += delta.Y;
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
        return Matrix4.CreateScale(imageWidth, imageHeight, 1.0f) * Scale(scaleX, scaleY);
    }

    internal static Matrix4 Scale(float xScale, float yScale)
    {
        return Matrix4.CreateScale(xScale, yScale, 1.0f);
    }

    internal Matrix4 Translate()
    {
        return Matrix4.CreateTranslation(x, y, zOrdering);
    }

    internal Matrix4 GetRenderedTransform(float imageWidth, float imageHeight)
    {
        var translation = Translate();
        var rotation = Get2DRotation();
        var scale = ScaleFromNDC(imageWidth, imageHeight);

        // return translation * rotation * scale;
        // return translation * scale * rotation;
        return scale * rotation * translation;
    }
}
