using OpenTK.Graphics.OpenGL4;
using rasdaq.Graphics.Shaders;

namespace rasdaq.Graphics;

public class Sprite : IDisposable
{
    private int vertexBufferObject;
    private int vertexArrayObject;
    Shader _shader;

    public Sprite(float[] vertices, Shader? shader = null)
    {
        _shader = shader ?? new Shader();

        vertexBufferObject = GL.GenBuffer();
        vertexArrayObject = GL.GenVertexArray();

        GL.BindVertexArray(vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertices.Length * sizeof(float),
            vertices,
            BufferUsageHint.StaticDraw
        );

        int aPositionLocation = _shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(
            aPositionLocation,
            3,
            VertexAttribPointerType.Float,
            false,
            3 * sizeof(float),
            0
        );
        GL.EnableVertexAttribArray(aPositionLocation);

        if (Application.Instance == null)
        {
            throw new InvalidOperationException(
                "No instance of Application found. Ensure that an Application is created before creating a Sprite."
            );
        }

        Application.Instance.LoadSprite(this);
    }

    internal void Render()
    {
        _shader.Use();
        GL.BindVertexArray(vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
    }

    public void Dispose()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(vertexBufferObject);
        GL.DeleteVertexArray(vertexArrayObject);
        GL.BindVertexArray(0);
        _shader.Dispose();

        Console.WriteLine("Sprite disposed.");
    }

    ~Sprite()
    {
        Dispose();
    }
}
