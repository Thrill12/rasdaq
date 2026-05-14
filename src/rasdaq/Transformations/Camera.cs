using OpenTK.Mathematics;
using rasdaq.Graphics;
using rasdaq.Graphics.Shaders;

namespace rasdaq.Transformations;

// dont make this static
public class Camera
{
    public Vector3 Position { get; private set; } = new(0.0f, 0.0f, 1000.0f);

    // instead add a function to let camera follow sprite by setting coords
    // public static Sprite? SpriteToFollow { private get; set; }

    // internal static void SetCameraThisFrame(Vector2i windowSize, Matrix4 model, Shader shader)
    // {
    //     // if (SpriteToFollow?.Entity is not null)
    //     //     SetCameraPosition(SpriteToFollow.Entity.Transform.LocalX, SpriteToFollow.Entity.Transform.LocalY);
    //     shader.SetUniform("projection", SetProjectionThisFrame(windowSize), true);
    //     shader.SetUniform("view", GetView(), true);
    //     shader.SetUniform("transform", model, true);
    // }

    // private static Matrix4 SetProjectionThisFrame(Vector2i windowSize)
    // {
    //     return Matrix4.CreateOrthographicOffCenter(0, windowSize.X, 0, windowSize.Y, 0.1f, 100);
    // }

    internal Matrix4 GetView()
    {
        var front = new Vector3(0.0f, 0.0f, -1.0f);
        var up = new Vector3(0.0f, 1.0f, 0.0f);

        return Matrix4.LookAt(Position, Position + front, up);
    }

    public void SetCameraPosition(float x, float y)
    {
        Position = new(x, y, 3.0f);
    }

    // public static void MoveCameraPosition(float? x, float? y)
    // {
    //     var cX = x ?? 0.0f;
    //     var cY = y ?? 0.0f;
    //     CameraPosition += new Vector3(cX, cY, 0);
    // }
}
