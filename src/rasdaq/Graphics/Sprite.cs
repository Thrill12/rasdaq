using System.Drawing;
using rasdaq.Graphics.Shaders;

namespace rasdaq.Graphics;

public class Sprite
{
    private Shader _shader;

    public Shader Shader => _shader;

    private float[] _vertices;
    public float[] Vertices => _vertices;

    private float[] _uvs;
    public float[] UVs => _uvs;

    private Color _color;
    public Color Color => _color;

    private Texture? _texture;
    public Texture? Texture => _texture;

    public Sprite(float width, float height, Color color)
    {
        _ = new Sprite(width, height, color, null);
    }

    public Sprite(float width, float height, string texturePath)
    {
        _ = new Sprite(width, height, Color.White, texturePath);
    }

    public Sprite(float width, float height, Color? color = null, string? texturePath = null)
    {
        float hw = width / 2;
        float hh = height / 2;
        _vertices =
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

        _ = new Sprite(_vertices, color, texturePath);
    }

    public Sprite(float[] vertices, Color? color = null, string? texturePath = null)
    {
        _vertices = vertices;
        _color = color ?? Color.White;

        if (texturePath != null)
        {
            _texture = new Texture(texturePath);
            _shader = new Shader(Common.TEXTURE_SHADER, Common.TEXTURE_SHADER_FRAG);
        }
        else
        {
            _shader = new Shader();
        }

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

        Renderer.Instance.LoadSprite(this);
    }
}
