using NUnit.Framework;
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

class MockTexture : Texture
{
    public MockTexture() : base()
    {

    }
}

[TestFixture]
public class SpriteTests
{
    [SetUp]
    public void Init()
    {

    }

    [Test]
    public void Sprite_WithVertices_SetsPropertiesCorrectly()
    {
        float[] vertices = [0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f];
        Color color = Color.Red;

        var sprite = new Sprite(vertices, color, null, new MockShader());

        Assert.That(sprite.Vertices, Is.EqualTo(null));
        Assert.That(sprite.Color, Is.EqualTo(color));
        Assert.That(sprite.Texture, Is.Null);
    }

    [Test]
    public void Sprite_WithWidthHeight_GeneratesCorrectQuadVertices()
    {
        float width = 2f,
            height = 3f;

        var sprite = new Sprite(width, height, Color.White, new MockShader());

        Assert.That(sprite.Vertices.Length, Is.EqualTo(18)); // 6 vertices * 3 floats
        Assert.That(sprite.Vertices[0], Is.EqualTo(-1f)); // left
        Assert.That(sprite.Vertices[1], Is.EqualTo(-1.5f)); // bottom
        Assert.That(sprite.Vertices[3], Is.EqualTo(1f)); // right
        Assert.That(sprite.Vertices[7], Is.EqualTo(1.5f)); // top
    }

    [Test]
    public void Sprite_WithTexture_LoadsTexture()
    {
        Texture tex = new MockTexture();
        var sprite = new Sprite(1f, 1f, Color.White, tex, new MockShader());

        Assert.That(sprite.Texture, Is.Not.Null);
        Assert.That(sprite.Color, Is.EqualTo(Color.White));
    }

    [Test]
    public void Sprite_DefaultColor_IsWhite()
    {
        var sprite = new Sprite(1f, 1f, null, null, new MockShader());

        Assert.That(sprite.Color, Is.EqualTo(Color.White));
    }
}
