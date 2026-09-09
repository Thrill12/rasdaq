using OpenTK.Mathematics;
using OVector3 = OpenTK.Mathematics.Vector3;

namespace rasdaq.Transformations;

/// <summary>
/// Represents a camera in 2D space. Camera position is defined by its x and y coordinates.
/// The `(x,y)` coordinates are of the bottom left corner of the camera's view
/// </summary>
public class Camera
{
    /// <summary>
    /// Bottom left corner of the camera's view
    /// </summary>
    public Vector3 Position { get; private set; } = new(0.0f, 0.0f, 1000.0f);

    internal Matrix4 GetView()
    {
        var front = new OVector3(0.0f, 0.0f, -1.0f);
        var up = new OVector3(0.0f, 1.0f, 0.0f);

        return Matrix4.LookAt((OVector3)Position, (OVector3)Position + front, up);
    }

    /// <summary>
    /// Sets the camera's position
    /// </summary>
    /// <param name="x">X coordinate of the bottom left corner of the camera's view</param>
    /// <param name="y">Y coordinate of the bottom left corner of the camera's view</param>
    public void SetPosition(double x, double y)
    {
        Position = new(x, y, 1000.0f);
    }
}
