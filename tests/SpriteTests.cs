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
    public void Sprite_WithWidthHeight_SetsPropertiesCorrectly()
    {
        float width = 10f;
        float height = 10f;
        Color color = Color.Red;

        var sprite = new Sprite(width, height, color, null, new MockShader());

        Assert.That(sprite.width, Is.EqualTo(width));
        Assert.That(sprite.height, Is.EqualTo(height));
        Assert.That(sprite.Color, Is.EqualTo(color));
        Assert.That(sprite.Texture, Is.Null);
    }

    [Test]
    public void Sprite_WithTexture_LoadsTexture()
    {
        Texture tex = new MockTexture();
        var sprite = new Sprite(1f, 1f, color: Color.White, texture: tex, shader: new MockShader());

        Assert.That(sprite.Texture, Is.Not.Null);
        Assert.That(sprite.Color, Is.EqualTo(Color.White));
    }

    [Test]
    public void Sprite_DefaultColor_IsWhite()
    {
        var sprite = new Sprite(1f, 1f, color: null, texture: null, shader: new MockShader());

        Assert.That(sprite.Color, Is.EqualTo(Color.White));
    }
}
