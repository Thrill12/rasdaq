
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
}
