
/// <summary>
/// A simple 2D vector struct. Does not support math operations currently
/// </summary>
/// <param name="x">X component</param>
/// <param name="y">Y component</param>
public struct Vector2(double x = 0, double y = 0)
{
    public double X { get; set; } = x;
    public double Y { get; set; } = y;

    public override readonly string ToString() => $"({X}, {Y})";

    public static readonly Vector2 Zero = new(0, 0);

    public static implicit operator OpenTK.Mathematics.Vector2(Vector2 v)
    {
        return new OpenTK.Mathematics.Vector2((float)v.X, (float)v.Y);
    }

    public static implicit operator Vector2(OpenTK.Mathematics.Vector2 v)
    {
        return new Vector2(v.X, v.Y);
    }

    public static implicit operator Vector2(OpenTK.Mathematics.Vector2i v)
    {
        return new Vector2(v.X, v.Y);
    }
}
