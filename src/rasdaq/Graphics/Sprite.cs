using rasdaq.Core.ECS;
using rasdaq.Graphics.Shaders;
using rasdaq.Transformations;
using System.Drawing;

namespace rasdaq.Graphics;

public class Sprite : Component
{
    private Shader _shader;
    // private Transform _transform = new();
    private float[] _vertices;
    private float[] _uvs;
    private Color _color;
    private Texture? _texture;

    /// <summary>
    /// Texture of sprite.
    /// </summary>
    public Texture? Texture => _texture;
    // public Transform Transform => _transform;
    /// <summary>
    /// Shader of Sprite.
    /// </summary>
    public Shader Shader => _shader;
    /// <summary>
    /// Colour of Sprite.
    /// </summary>
    public Color Color => _color;
    public float[] UVs => _uvs;
    public float[] Vertices => _vertices;

    internal override void Init()
    {
        Renderer.Instance.LoadSprite(this);
    }

    public void SetShader(Shader newShader)
    {
        _shader = newShader;
    }

    public Sprite(float width, float height, Color color, Shader? shader = null) :
        this(width, height, color, null, shader)
    { }

    public Sprite(float width, float height, Texture texture, Shader? shader = null) :
        this(width, height, Color.White, texture, shader)
    { }

    public Sprite(float width, float height, Color? color = null, Texture? texture = null, Shader? shader = null) :
        this(BuildVertices(width, height), color, texture, shader)
    { }

    public void SetWidthHeight(float width, float height)
    {
        _vertices = BuildVertices(width, height);
    }

    public Sprite(float[] vertices, Color? color = null, Texture? texture = null, Shader? shader = null)
    {
        _vertices = vertices;
        _color = color ?? Color.White;
        _texture = texture;

        _shader = shader ?? (texture != null
            ? new Shader(Common.TEXTURE_SHADER, Common.TEXTURE_SHADER_FRAG)
            : new Shader(Common.COLOR_SHADER, Common.COLOR_SHADER_FRAG));

        _uvs =
        [
            0.0f,
            0.0f, // BL
            1.0f,
            0.0f, // BR
            1.0f,
            1.0f, // TR
            0.0f,
            1.0f, // TL
            1.0f,
            1.0f, // TR (repeated)
            0.0f,
            0.0f, // BL (repeated)
        ];
    }

    private static float[] BuildVertices(float width, float height)
    {
        float hw = width / 2;
        float hh = height / 2;
        return
        [
            -hw,
            -hh,
            0.0f, // BL
            hw,
            -hh,
            0.0f, // BR
            hw,
            hh,
            0.0f, // TR
            -hw,
            hh,
            0.0f, // TL
            hw,
            hh,
            0.0f, // TR (repeated)
            -hw,
            -hh,
            0.0f, // BL (repeated)
        ];
    }
}