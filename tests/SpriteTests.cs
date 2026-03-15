using System.Drawing;
using System.IO.Abstractions;
using NUnit.Framework;
using OpenTK.Graphics.OpenGL4;
using rasdaq.Graphics;

namespace rasdaq.Tests;

[TestFixture]
public class SpriteTests
{
    [SetUp]
    public void Init()
    {
        var fileSystem = new MockFileSystem();
    }

    [Test]
    public void Sprite_WithVertices_SetsPropertiesCorrectly()
    {
        // Arrange
        float[] vertices = [0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f];
        Color color = Color.Red;

        // Act
        var sprite = new Sprite(vertices, color);

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
        var sprite = new Sprite(1f, 1f, Color.White, texturePath);

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
