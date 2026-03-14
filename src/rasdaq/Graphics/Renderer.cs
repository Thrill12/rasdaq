using OpenTK.Graphics.OpenGL4;
using rasdaq.Graphics.Shaders;

namespace rasdaq.Graphics;

public class Renderer
{
    public static Renderer Instance { get; private set; } = new Renderer();

    private int vertexBufferObject;
    private int vertexArrayObject;

    private Shader _shader;
    public Shader Shader => _shader;

    private List<Sprite> sprites = new List<Sprite>();

    private List<float> vertices = new List<float>();
    public List<float> Vertices => vertices;

    public void Init()
    {
        _shader = new Shader(
            "src/rasdaq/Graphics/Shaders/texture_shader.vert",
            "src/rasdaq/Graphics/Shaders/texture_shader.frag"
        );
        _shader.Use();

        vertexBufferObject = GL.GenBuffer();
        vertexArrayObject = GL.GenVertexArray();

        GL.BindVertexArray(vertexArrayObject);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);

        // aPosition always at index 0, offset 0
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 9 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

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

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferObject);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Count * sizeof(float),
                vertices.ToArray(),
                BufferUsageHint.DynamicDraw
            );

            sprite.Shader.Use();
            if (sprite.Texture != null)
            {
                // UVs
                GL.VertexAttribPointer(
                    1,
                    2,
                    VertexAttribPointerType.Float,
                    false,
                    9 * sizeof(float),
                    3 * sizeof(float)
                );
                GL.EnableVertexAttribArray(1);

                // Colors
                GL.VertexAttribPointer(
                    2,
                    4,
                    VertexAttribPointerType.Float,
                    false,
                    9 * sizeof(float),
                    5 * sizeof(float)
                );
                GL.EnableVertexAttribArray(2);

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
                    1,
                    3,
                    VertexAttribPointerType.Float,
                    false,
                    9 * sizeof(float),
                    5 * sizeof(float)
                );
                GL.EnableVertexAttribArray(1);
                GL.DisableVertexAttribArray(2);
            }
            GL.BindVertexArray(vertexArrayObject);
            GL.DrawArrays(PrimitiveType.Triangles, 0, vertices.Count / 8);
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
