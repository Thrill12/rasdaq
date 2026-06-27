using OpenTK.Mathematics;
using rasdaq.Logging;
using OVector3 = OpenTK.Mathematics.Vector3;

namespace rasdaq.Transformations;

/// <summary>
/// Represents a camera in 2D space. Camera position is defined by its x and y coordinates.
/// The `(x,y)` coordinates are of the bottom left corner of the camera's view
/// </summary>
public class Camera
{
    private Vector3 _targetCenter = new(0.0f, 0.0f, 1000.0f);

    /// <summary>
    /// Center of the camera's view
    /// </summary>
    public Vector3 Position
    {
        get { return _targetCenter; }
        set { _targetCenter = new Vector3(value.X, value.Y, 1000.0f); }
    }

    /// <summary>
    /// Bottom left corner of the camera's view
    /// </summary>
    internal Vector3 _Position
    {
        get
        {
            try
            {
                Vector2 windowSize = Application.WindowSize ?? Vector2.Zero;
                return _targetCenter - new Vector3(windowSize.X / 2f, windowSize.Y / 2f, 0f);
            }
            catch (Exception e)
            {
                Log.Exception(e, "Camera position calculation error");
                return _targetCenter;
            }
        }
    }

    internal Matrix4 GetView()
    {
        var front = new OVector3(0.0f, 0.0f, -1.0f);
        var up = new OVector3(0.0f, 1.0f, 0.0f);

        return Matrix4.LookAt((OVector3)_Position, (OVector3)_Position + front, up);
    }
}
