using rasdaq.Core.ECS;
using rasdaq.Graphics.Shaders;
using rasdaq.Logging;
using System.Drawing;

namespace rasdaq.Graphics;

public class Sprite : Component
{
    private Shader _shader;
    private float[] _vertices;
    private float[] _uvs;
    private Color _color;
    private Texture? _texture;

    /// <summary>
    /// Texture of sprite.
    /// </summary>
    public Texture? Texture => _texture;
    /// <summary>
    /// Shader of Sprite.
    /// </summary>
    public Shader Shader => _shader;
    /// <summary>
    /// Colour of Sprite.
    /// </summary>
    public Color Color => _color;
    internal float[] UVs => _uvs;
    internal float[] Vertices => _vertices;

    internal override void Init()
    {
        Log.Trace("Initializing sprite");
        Renderer.LoadSprite(this);
    }

    public void SetShader(Shader newShader)
    {
        _shader = newShader;
    }

    /// <summary>
    /// Create an instance of <c>Sprite</c>.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    /// <param name="shader"></param>
    public Sprite(float width, float height, Color color, Shader? shader = null) :
        this(width, height, color, null, shader)
    { }

    /// <summary>
    /// Create an instance of <c>Sprite</c>.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="texture"></param>
    /// <param name="shader"></param>
    public Sprite(float width, float height, Texture texture, Shader? shader = null) :
        this(width, height, Color.White, texture, shader)
    { }

    /// <summary>
    /// Create an instance of <c>Sprite</c>.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="color"></param>
    /// <param name="texture"></param>
    /// <param name="shader"></param>
    public Sprite(float width, float height, Color? color = null, Texture? texture = null, Shader? shader = null) :
        this(BuildVertices(width, height), color, texture, shader)
    { }

    /// <summary>
    /// Set width and height of the sprite.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public void SetWidthHeight(float width, float height)
    {
        _vertices = BuildVertices(width, height);
    }

    internal Sprite(float[] vertices, Color? color = null, Texture? texture = null, Shader? shader = null)
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