using System.Drawing;
using OpenTK.Graphics.OpenGL4;
using rasdaq.Graphics.Shaders;

namespace rasdaq.Graphics;

public class Sprite
{
    private Shader _shader;

    public Shader Shader => _shader;

    private float[] _vertices;
    public float[] Vertices => _vertices;

    private Color _color;
    public Color Color => _color;

    public Sprite(float[] vertices, Color? color = null, Shader? shader = null)
    {
        _vertices = vertices;
        _color = color ?? Color.White;
        _shader = shader ?? new Shader();
        SpriteRenderer.Instance.LoadSprite(this);
    }
}
