using OpenTK.Mathematics;
using OVector2 = OpenTK.Mathematics.Vector2;
using OVector3 = OpenTK.Mathematics.Vector3;

namespace rasdaq.Transformations;

// dont make this static
public class Camera
{
    public Vector3 Position { get; private set; } = new(0.0f, 0.0f, 1000.0f);

    internal Matrix4 GetView()
    {
        var front = new OVector3(0.0f, 0.0f, -1.0f);
        var up = new OVector3(0.0f, 1.0f, 0.0f);

        return Matrix4.LookAt((OVector3)Position, (OVector3)Position + front, up);
    }

    public void SetPosition(double x, double y)
    {
        Position = new(x, y, 1000.0f);
    }
}
