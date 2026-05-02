using OpenTK.Mathematics;
using rasdaq.Graphics.Shaders;

public class Camera
{
    public static Vector3 CameraPosition { get; private set; } = new(0.0f, 0.0f, 3.0f);

    internal static void SetCameraThisFrame(Vector2i windowSize, Matrix4 model, Shader shader)
    {
        shader.SetUniform("projection", SetProjectionThisFrame(windowSize), true);
        shader.SetUniform("view", GetView(), true);
        shader.SetUniform("transform", model, true);
    }

    private static Matrix4 SetProjectionThisFrame(Vector2i windowSize)
    {
        return Matrix4.CreateOrthographicOffCenter(0, windowSize.X, 0, windowSize.Y, 0.1f, 100);
    }

    public static Matrix2d GetCameraCoords()
    {
        return Matrix2d.Identity;
    }

    private static Matrix4 GetView()
    {
        var front = new Vector3(0.0f, 0.0f, -1.0f);
        var up = new Vector3(0.0f, 1.0f, 0.0f);

        return Matrix4.LookAt(CameraPosition, CameraPosition + front, up);
    }

    public static void SetCameraPosition(float x, float y)
    {
        CameraPosition = new(x, y, 3.0f);
    }

    public static void MoveCameraPosition(float? x, float? y)
    {
        var cX = x ?? 0.0f;
        var cY = y ?? 0.0f;
        CameraPosition += new Vector3(cX, cY, 0);
    }
}