/// <summary>
/// A simple 3D vector struct. Does not support math operations currently
/// </summary>
/// <param name="x">X component</param>
/// <param name="y">Y component</param>
/// <param name="z">Z component</param>
public struct Vector3(double x = 0, double y = 0, double z = 0)
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;
    public double Z { get; set; } = z;
    public static readonly Vector3 Zero = new(0, 0, 0);

    public override readonly string ToString() => $"({X}, {Y}, {Z})";

    public static implicit operator OpenTK.Mathematics.Vector3(Vector3 v)
    {
        return new OpenTK.Mathematics.Vector3((float)v.X, (float)v.Y, (float)v.Z);
    }

    public static implicit operator Vector3(OpenTK.Mathematics.Vector3 v)
    {
        return new Vector3(v.X, v.Y, v.Z);
    }

    public static implicit operator Vector2(Vector3 v)
    {
        return new Vector2(v.X, v.Y);
    }

    public static implicit operator Vector3(Vector2 v)
    {
        return new Vector3(v.X, v.Y, 0);
    }

    public static implicit operator OpenTK.Mathematics.Vector2(Vector3 v)
    {
        return new OpenTK.Mathematics.Vector2((float)v.X, (float)v.Y);
    }

    public static Vector3 operator -(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
    }

    public static Vector3 operator +(Vector3 v1, Vector3 v2)
    {
        return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
    }

    public static Vector3 operator *(Vector3 v, double scalar)
    {
        return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
    }

    public static Vector3 operator *(double scalar, Vector3 v)
    {
        return new Vector3(v.X * scalar, v.Y * scalar, v.Z * scalar);
    }

    public static Vector3 operator /(Vector3 v, double scalar)
    {
        return new Vector3(v.X / scalar, v.Y / scalar, v.Z / scalar);
    }
}
