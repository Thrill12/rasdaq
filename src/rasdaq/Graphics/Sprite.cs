using rasdaq.Graphics.Shaders;
using System.Drawing;

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

    public void SetShader(Shader newShader)
    {
        _shader = newShader;
    }

    public Sprite(float width, float height, Color color, Shader? shader = null)
    {
        _ = new Sprite(width, height, color, null, shader);
    }

    public Sprite(float width, float height, string texturePath, Shader? shader = null)
    {
        _ = new Sprite(width, height, Color.White, texturePath, shader);
    }

    public Sprite(float width, float height, Color? color = null, string? texturePath = null, Shader? shader = null)
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

        _ = new Sprite(_vertices, color, texturePath, shader);
    }

    public Sprite(float[] vertices, Color? color = null, string? texturePath = null, Shader? shader = null)
    {
        _vertices = vertices;
        _color = color ?? Color.White;

        if (shader == null)
        {
            if (texturePath != null)
            {
                _texture = new Texture(texturePath);
                _shader = new Shader(Common.TEXTURE_SHADER, Common.TEXTURE_SHADER_FRAG);
            }
            else
            {
                _shader = new Shader();
            }
        }
        else
        {
            _shader = shader;
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
