using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenTK.Mathematics;
using rasdaq.Graphics;
using rasdaq.Graphics.Shaders;
using rasdaq.Transformations;
using System.Drawing;

namespace tests;

// class MockShader : Shader
// {
//     public MockShader() : base()
//     {

//     }
// }

// class MockTexture : Texture
// {
//     public MockTexture() : base()
//     {

//     }
// }

[TestFixture]
public class TransformTests
{
    [SetUp]
    public void Init()
    {

    }

    [Test]
    public void Rotate2D_SetDegrees_RotateZDegrees()
    {
        var transform = new Transform();
        float testAngle = 30;

        transform.Rotate2D(testAngle);

        var zRotation = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(testAngle));

        Assert.That(transform._GetTransformation(1), Is.EqualTo(zRotation));
    }

    [Test]
    public void MoveOnce_SetOnceVelocity_MoveOnceVelocity()
    {
        var transform = new Transform();
        Vector2 moveOnceVelocity = new(100);

        transform.MoveOnce(moveOnceVelocity);

        var correctTranslation = Matrix4.CreateTranslation(moveOnceVelocity.X, moveOnceVelocity.Y, 0);

        Assert.That(transform._GetTransformation(1), Is.EqualTo(correctTranslation));
    }

    [Test]
    public void MoveOnce_SetOnceVelocity_MoveOnceVelocityIn2Seconds()
    {
        var transform = new Transform();
        Vector2 moveOnceVelocity = new(100);

        transform.MoveOnce(moveOnceVelocity);

        // should move only once in the two frames
        Vector2 vectorAfter2Frames = moveOnceVelocity;
        var correctTranslation = Matrix4.CreateTranslation(vectorAfter2Frames.X, vectorAfter2Frames.Y, 0);

        // transform after first frame
        transform._GetTransformation(1);
        // transform on second frame
        var secondFrame = transform._GetTransformation(1);

        Assert.That(secondFrame, Is.EqualTo(correctTranslation));
    }

    [Test]
    public void SetVelocity_SetVelocity_MoveVelocityIn2Seconds()
    {
        var transform = new Transform();
        Vector2 velocity = new(100);

        transform.SetVelocity(velocity);

        Vector2 vectorAfter2Sec = new(velocity.X * 2, velocity.Y * 2);
        var correctTranslation = Matrix4.CreateTranslation(vectorAfter2Sec.X, vectorAfter2Sec.Y, 0);

        Assert.That(transform._GetTransformation(2), Is.EqualTo(correctTranslation));
    }

    [Test]
    public void MoveDistance_SetTenRight_MoveTenRight()
    {
        var transform = new Transform();
        Vector2 velocity = new(1, 0);
        float distance = 10f;

        transform.MoveDistance(velocity, distance);

        Vector2 vectorAfter10Sec = new(velocity.X * 10, velocity.Y);
        var correctTranslation = Matrix4.CreateTranslation(vectorAfter10Sec.X, vectorAfter10Sec.Y, 0);

        Assert.That(transform._GetTransformation(10), Is.EqualTo(correctTranslation));
    }

    [Test]
    public void MoveDistance_SetTenRight_MoveTenRightOverTwoFrames()
    {
        var transform = new Transform();
        Vector2 velocity = new(1, 0);
        float distance = 10f;

        transform.MoveDistance(velocity, distance);

        Vector2 vectorAfter10Sec = new(velocity.X * 10, velocity.Y);

        // frame 1
        transform._GetTransformation(5);
        // frame 2
        transform._GetTransformation(5);

        Assert.That(new Vector2(transform.WorldX, transform.WorldY), Is.EqualTo(vectorAfter10Sec));
    }


    // [Test]
    // public void Sprite_WithVertices_SetsPropertiesCorrectly()
    // {
    //     float[] vertices = [0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f];
    //     Color color = Color.Red;

    //     var sprite = new Sprite(vertices, color, null, new MockShader());

    //     Assert.That(sprite.Vertices, Is.EqualTo(vertices));
    //     Assert.That(sprite.Color, Is.EqualTo(color));
    //     Assert.That(sprite.Texture, Is.Null);
    // }

    // [Test]
    // public void Sprite_WithWidthHeight_GeneratesCorrectQuadVertices()
    // {
    //     float width = 2f,
    //         height = 3f;

    //     var sprite = new Sprite(width, height, Color.White, new MockShader());

    //     Assert.That(sprite.Vertices.Length, Is.EqualTo(18)); // 6 vertices * 3 floats
    //     Assert.That(sprite.Vertices[0], Is.EqualTo(-1f)); // left
    //     Assert.That(sprite.Vertices[1], Is.EqualTo(-1.5f)); // bottom
    //     Assert.That(sprite.Vertices[3], Is.EqualTo(1f)); // right
    //     Assert.That(sprite.Vertices[7], Is.EqualTo(1.5f)); // top
    // }

    // [Test]
    // public void Sprite_WithTexture_LoadsTexture()
    // {
    //     Texture tex = new MockTexture();
    //     var sprite = new Sprite(1f, 1f, Color.White, tex, new MockShader());

    //     Assert.That(sprite.Texture, Is.Not.Null);
    //     Assert.That(sprite.Color, Is.EqualTo(Color.White));
    // }

    // [Test]
    // public void Sprite_DefaultColor_IsWhite()
    // {
    //     var sprite = new Sprite(1f, 1f, null, null, new MockShader());

    //     Assert.That(sprite.Color, Is.EqualTo(Color.White));
    // }
}