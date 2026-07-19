using OpenTK.Mathematics;
using System.Runtime.CompilerServices;
using OVector2 = OpenTK.Mathematics.Vector2;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq.Transformations;

/// <summary>
/// Defines the position, scale and rotation of an `Entity`
/// </summary>
/// <param name="spawnPosition"></param>
public class Transform(Vector3 spawnPosition)
{
    /// <summary>
    /// position of the centre of the entity
    /// </summary>
    public Vector3 position = spawnPosition;
    /// <summary>
    /// <para>Scale of the entity. (1, 1) is the default, and means the image will be rendered at its original size.</para>
    /// <para>The (X, Y) components of the scale are applied to the width and height of the entity respectively</para>
    /// </summary>
    public Vector2 scale = new(1, 1);
    /// <summary>
    /// Rotation in degrees. Used for 2D rotation around Z axis.
    /// </summary>
    public float rotation = 0.0f;

    /// <summary>
    /// Rotate the entity from a point in the world space.
    ///
    /// Note that this does not rotate the entity itself.
    /// Instead, it moves the entity in a circular arc, with `point` being the centre of the arc.
    ///
    /// </summary>
    /// <param name="point">centre of rotation</param>
    /// <param name="rotation">rotation in degrees</param>
    public void RotateFromPoint(Vector2 point, float rotation)
    {
        // get radius
        Vector2 entityPos = new(position.X, position.Y);
        Vector2 radius = (OVector2)entityPos - (OVector2)point;

        // get trig
        var sin = Math.Sin(MathHelper.DegreesToRadians(rotation));
        var cos = Math.Cos(MathHelper.DegreesToRadians(rotation));

        // get x,y from origin
        var xFromOrigin = radius.X * cos - radius.Y * sin;
        var yFromOrigin = radius.X * sin + radius.Y * cos;

        // set position and round to 4 dp
        position.X = Math.Round(point.X + xFromOrigin, 4);
        position.Y = Math.Round(point.Y + yFromOrigin, 4);
    }

    internal void CoordUpdate(OVector2 delta)
    {
        position.X += delta.X;
        position.Y += delta.Y;
    }

    internal Matrix4 Get2DRotation()
    {
        return Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(rotation));
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
        // return scale * rotation * translation;
    }
}
