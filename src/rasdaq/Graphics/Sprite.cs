using OpenTK.Mathematics;
using rasdaq.Core.ECS;
using rasdaq.Graphics.Shaders;
using rasdaq.Transformations;
using System.Drawing;
using System.Numerics;

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

    public Sprite(float width, float height, Color color, float centreX = 0, float centreY = 0, Shader? shader = null) :
        this(width, height, centreX, centreY, color, null, shader)
    { }

    public Sprite(float width, float height, Texture texture, float centreX = 0, float centreY = 0, Shader? shader = null) :
        this(width, height, centreX, centreY, Color.White, texture, shader)
    { }

    public Sprite(float width, float height, float centreX = 0, float centreY = 0, Color? color = null, Texture? texture = null, Shader? shader = null) :
        this(BuildVertices(width, height, centreX, centreY), color, texture, shader)
    { }

    public void SetWidthHeight(float width, float height, float centreX = 0, float centreY = 0)
    {
        _vertices = BuildVertices(width, height, centreX, centreY);
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

    private static float[] BuildVertices(float width, float height, float centreX, float centreY)
    {
        float hw = width / 2;
        float hh = height / 2;

        float leftX = centreX - hw;
        float rightX = centreX + hw;

        float topY = centreY + hh;
        float bottomY = centreY - hh;

        //     return
        //     [
        //         -hw,
        //         -hh,
        //         0.0f, // BL
        //         hw,
        //         -hh,
        //         0.0f, // BR
        //         hw,
        //         hh,
        //         0.0f, // TR
        //         -hw,
        //         hh,
        //         0.0f, // TL
        //         hw,
        //         hh,
        //         0.0f, // TR (repeated)
        //         -hw,
        //         -hh,
        //         0.0f, // BL (repeated)
        //     ];
        // }
        return
        [
            leftX, bottomY, 0, // BL
            rightX, bottomY, 0, // BR
            rightX, topY, 0, // TR
            leftX, topY, 0, // TL
            rightX, topY, 0, // TR (repeated)
            leftX, bottomY, 0 // BL (repeated)
        ];
    }
}