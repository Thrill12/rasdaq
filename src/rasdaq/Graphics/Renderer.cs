using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace rasdaq.Graphics;

public class Renderer
{
    public static Renderer Instance { get; private set; } = new Renderer();

    private int vertexBufferObject;
    private int vertexArrayObject;

    private List<Sprite> sprites = new List<Sprite>();

    private List<float> vertices = new List<float>();
    public List<float> Vertices => vertices;

    public void Init()
    {
        vertexBufferObject = GL.GenBuffer();
        vertexArrayObject = GL.GenVertexArray();

        GL.BindVertexArray(vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

        vertices = new();
    }

    public void LoadSprite(Sprite sprite)
    {
        sprites.Add(sprite);
    }

    internal void Render()
    {
        foreach (Sprite sprite in sprites)
        {
            vertices.Clear();
            AddSpriteVertices(sprite);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Count * sizeof(float),
                vertices.ToArray(),
                BufferUsageHint.DynamicDraw
            );

            sprite.Shader.Use();
            sprite.Shader.SetUniform("transform", sprite.Transform.GetTransformation(), true);
            SetVertexAttributes(sprite);

            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count / 9);
        }
    }

    private static void SetVertexAttributes(Sprite sprite)
    {
        // aPosition — must be set every frame
        GL.VertexAttribPointer(
            sprite.Shader.GetAttribLocation("aPosition"),
            3,
            VertexAttribPointerType.Float,
            false,
            9 * sizeof(float),
            0
        );
        GL.EnableVertexAttribArray(sprite.Shader.GetAttribLocation("aPosition"));

        if (sprite.Texture != null)
        {
            // UVs
            GL.VertexAttribPointer(
                sprite.Shader.GetAttribLocation("aTexture"),
                2,
                VertexAttribPointerType.Float,
                false,
                9 * sizeof(float),
                3 * sizeof(float)
            );
            GL.EnableVertexAttribArray(sprite.Shader.GetAttribLocation("aTexture"));

            // Colors
            GL.VertexAttribPointer(
                sprite.Shader.GetAttribLocation("aColor"),
                4,
                VertexAttribPointerType.Float,
                false,
                9 * sizeof(float),
                5 * sizeof(float)
            );
            GL.EnableVertexAttribArray(sprite.Shader.GetAttribLocation("aColor"));

            sprite.Texture.Use();
            int texUniformLoc = sprite.Shader.GetUniformLocation("texture0");
            if (texUniformLoc >= 0)
            {
                GL.Uniform1(texUniformLoc, 0);
            }
        }
        else
        {
            GL.VertexAttribPointer(
                sprite.Shader.GetAttribLocation("aColor"),
                4,
                VertexAttribPointerType.Float,
                false,
                9 * sizeof(float),
                5 * sizeof(float)
            );
            GL.EnableVertexAttribArray(sprite.Shader.GetAttribLocation("aColor"));
            GL.DisableVertexAttribArray(sprite.Shader.GetAttribLocation("aTexture"));
        }
    }

    private void AddSpriteVertices(Sprite sprite)
    {
        for (int i = 0, uvIndex = 0; i < sprite.Vertices.Length; i += 3, uvIndex += 2)
        {
            vertices.Add(sprite.Vertices[i]);
            vertices.Add(sprite.Vertices[i + 1]);
            vertices.Add(sprite.Vertices[i + 2]);

            vertices.Add(sprite.UVs[uvIndex]);
            vertices.Add(sprite.UVs[uvIndex + 1]);

            vertices.Add(sprite.Color.R / 255f);
            vertices.Add(sprite.Color.G / 255f);
            vertices.Add(sprite.Color.B / 255f);
            vertices.Add(sprite.Color.A / 255f);
        }
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