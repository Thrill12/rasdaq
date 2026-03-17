using rasdaq.Graphics;
using rasdaq.Graphics.Shaders;
using System.Drawing;

namespace tests;

class MockShader : Shader
{
    public MockShader() : base()
    {

    }
}

[TestFixture]
public class SpriteTests
{

    string fragShader = "#version 330 core\r\n\r\nin vec2 TextureCoord;\r\nin vec4 VertColor;\r\n\r\nout vec4 FragColor;\r\n\r\nuniform sampler2D texture0;\r\n\r\nvoid main()\r\n{\r\n    FragColor = texture(texture0, TextureCoord) * VertColor;\r\n}";
    string vertShader = "#version 330 core\r\n\r\nin vec3 aPosition;\r\nin vec2 aTexture;\r\nin vec4 aColor;\r\n\r\nout vec2 TextureCoord;\r\nout vec4 VertColor;\r\n\r\nvoid main()\r\n{\r\n    gl_Position = vec4(aPosition, 1.0);\r\n\r\n    TextureCoord = aTexture;\r\n    VertColor = aColor;\r\n}";

    [SetUp]
    public void Init()
    {

    }

    [Test]
    public void Sprite_WithVertices_SetsPropertiesCorrectly()
    {
        // Arrange
        float[] vertices = [0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f];
        Color color = Color.Red;

        // Act
        var sprite = new Sprite(vertices, color, null, new MockShader());

        // Assert
        Assert.That(sprite.Vertices, Is.EqualTo(vertices));
        Assert.That(sprite.Color, Is.EqualTo(color));
        Assert.That(sprite.Texture, Is.Null);
    }

    [Test]
    public void Sprite_WithWidthHeight_GeneratesCorrectQuadVertices()
    {
        // Arrange
        float width = 2f,
            height = 3f;

        // Act
        var sprite = new Sprite(width, height);

        // Assert
        Assert.That(sprite.Vertices.Length, Is.EqualTo(18)); // 6 vertices * 3 floats
        Assert.That(sprite.Vertices[0], Is.EqualTo(-1f)); // left
        Assert.That(sprite.Vertices[1], Is.EqualTo(-1.5f)); // bottom
        Assert.That(sprite.Vertices[3], Is.EqualTo(1f)); // right
        Assert.That(sprite.Vertices[7], Is.EqualTo(1.5f)); // top
    }

    [Test]
    public void Sprite_WithTexture_LoadsTexture()
    {
        // Arrange
        string texturePath = "samples/pong/assets/praise_the_lord.png";

        // Act
        var sprite = new Sprite(1f, 1f, Color.White, texturePath, new MockShader());

        // Assert
        Assert.That(sprite.Texture, Is.Not.Null);
        Assert.That(sprite.Color, Is.EqualTo(Color.White));
    }

    [Test]
    public void Sprite_DefaultColor_IsWhite()
    {
        // Act
        var sprite = new Sprite(1f, 1f);

        // Assert
        Assert.That(sprite.Color, Is.EqualTo(Color.White));
    }
}
