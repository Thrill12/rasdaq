using System;
using OpenTK.Graphics.OpenGL4;
using rasdaq.Graphics.Shaders;

namespace rasdaq.Graphics;

public class SpriteRenderer
{
    public static SpriteRenderer Instance { get; private set; } = new SpriteRenderer();

    private int vertexBufferObject;
    private int vertexArrayObject;

    private List<Sprite> sprites = new List<Sprite>();

    private Shader _shader;
    public Shader Shader => _shader;

    private List<float> vertices = new List<float>();
    public List<float> Vertices => vertices;

    public void Init()
    {
        // Initialize shaders
        _shader = new Shader();
        _shader.Use();

        // Initialize buffers
        vertexBufferObject = GL.GenBuffer();
        vertexArrayObject = GL.GenVertexArray();

        GL.BindVertexArray(vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

        // Define vertex attributes
        int aPositionLocation = Shader.GetAttribLocation("aPosition");
        GL.VertexAttribPointer(
            aPositionLocation,
            3,
            VertexAttribPointerType.Float,
            false,
            6 * sizeof(float),
            0
        );
        GL.EnableVertexAttribArray(aPositionLocation);

        // vertex colors
        int aColorLocation = Shader.GetAttribLocation("aColor");
        GL.VertexAttribPointer(
            aColorLocation,
            3,
            VertexAttribPointerType.Float,
            false,
            6 * sizeof(float),
            3 * sizeof(float)
        );
        GL.EnableVertexAttribArray(aColorLocation);

        vertices = new List<float>();
    }

    public void LoadSprite(Sprite sprite)
    {
        sprites.Add(sprite);
    }

    internal void Render()
    {
        vertices.Clear();
        foreach (Sprite sprite in sprites)
        {
            for (int i = 0; i < sprite.Vertices.Length; i += 3)
            {
                vertices.Add(sprite.Vertices[i]); // x
                vertices.Add(sprite.Vertices[i + 1]); // y
                vertices.Add(sprite.Vertices[i + 2]); // z

                vertices.Add(sprite.Color.R / 255f); // r
                vertices.Add(sprite.Color.G / 255f); // g
                vertices.Add(sprite.Color.B / 255f); // b
            }
        }

        // Upload vertices to VBO
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
        GL.BufferData(
            BufferTarget.ArrayBuffer,
            vertices.Count * sizeof(float),
            vertices.ToArray(),
            BufferUsageHint.DynamicDraw
        );

        // Use shader and draw all sprites in one draw call
        _shader.Use();
        GL.BindVertexArray(vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count / 3);
    }

    public void Dispose()
    {
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.DeleteBuffer(vertexBufferObject);
        GL.DeleteVertexArray(vertexArrayObject);
        GL.BindVertexArray(0);

        foreach (Sprite sprite in sprites)
        {
            sprite.Shader.Dispose();
        }
    }
}
