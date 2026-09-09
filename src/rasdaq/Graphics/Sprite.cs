using rasdaq.Core.ECS;
using rasdaq.Graphics.Shaders;
using System.Drawing;

namespace rasdaq.Graphics;

public class Sprite : Component
{
    private Shader _shader;
    private float[] _ndcVertices;
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
    public float[] UVs => _uvs;
    public float[] NdcVertices => _ndcVertices;
    public float width;
    public float height;

    internal override void Init()
    {
        Renderer.Instance.LoadSprite(this);
    }

    public void SetShader(Shader newShader)
    {
        _shader = newShader;
    }

    /// <summary>
    /// Create Sprite object with color
    /// </summary>
    /// <param name="width">width of sprite, in pixels</param>
    /// <param name="height">height of sprite, in pixels</param>
    /// <param name="color">color of sprite</param>
    /// <param name="shader">Shader object for the sprite</param>
    public Sprite(float width, float height, Color color, Shader? shader = null)
        : this(width, height, color, null, shader) { }

    /// <summary>
    /// Create sprite object with texture (such as image)
    /// </summary>
    /// <param name="width">width of sprite, in pixels</param>
    /// <param name="height">height of sprite, in pixels</param>
    /// <param name="texture">texture of sprite</param>
    /// <param name="shader">Shader object for the sprite</param>
    public Sprite(float width, float height, Texture texture, Shader? shader = null)
        : this(width, height, Color.White, texture, shader) { }

    /// <summary>
    /// Create sprite object with texture and a color tint
    /// </summary>
    /// <param name="width">width of sprite, in pixels</param>
    /// <param name="height">height of sprite, in pixels</param>
    /// <param name="color">color tint of sprite</param>
    /// <param name="texture">texture of sprite</param>
    /// <param name="shader">Shader object for the sprite</param>
    public Sprite(
        float width,
        float height,
        Color? color = null,
        Texture? texture = null,
        Shader? shader = null
    )
    {
        _ndcVertices = BuildVertices();
        this.width = width;
        this.height = height;
        _color = color ?? Color.White;
        _texture = texture;

        _shader =
            shader
            ?? (
                texture != null
                    ? new Shader(Common.TEXTURE_SHADER, Common.TEXTURE_SHADER_FRAG)
                    : new Shader(Common.COLOR_SHADER, Common.COLOR_SHADER_FRAG)
            );

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

    private static float[] BuildVertices()
    {
        float leftX = -0.5f;
        float rightX = 0.5f;

        float topY = 0.5f;
        float bottomY = -0.5f;

        return
        [
            leftX,
            bottomY,
            0, // BL
            rightX,
            bottomY,
            0, // BR
            rightX,
            topY,
            0, // TR
            leftX,
            topY,
            0, // TL
            rightX,
            topY,
            0, // TR (repeated)
            leftX,
            bottomY,
            0, // BL (repeated)
        ];
    }

    public override void Destroy()
    {
        Renderer.Instance.RemoveSprite(this);
        Shader.Dispose();
    }
}
