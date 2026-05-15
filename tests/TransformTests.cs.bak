using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenTK.Mathematics;
using rasdaq.Transformations;

namespace tests;

[TestFixture]
public class TransformTests
{
    [SetUp]
    public void Init()
    {

    }

    private void RunTransformFrames(Transform transform, int frames, float deltaTime = 1)
    {
        for (int frame = 0; frame < frames; frame++)
        {
            transform._GetTransformation(deltaTime);
        }
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
        Vector2 velocity = new(5, 0);
        float distance = 10f;

        transform.MoveDistance(velocity, distance);

        Vector2 vectorAfter10Sec = new(velocity.X * 2, velocity.Y);

        RunTransformFrames(transform, 2);

        Assert.That(new Vector2(transform.LocalX, transform.LocalY), Is.EqualTo(vectorAfter10Sec));
    }

    [Test]
    public void MoveDistance_SetElevenRight_MoveElevenRightOverThreeFrames()
    {
        var transform = new Transform();
        Vector2 velocity = new(5, 0);
        float distance = 11f;

        transform.MoveDistance(velocity, distance);

        Vector2 vectorAfter3Frames = new(11, velocity.Y);

        RunTransformFrames(transform, 3);

        Assert.That(new Vector2(transform.LocalX, transform.LocalY), Is.EqualTo(vectorAfter3Frames));
    }

    [Test]
    public void MoveDistance_SetTenRightWithVelocity_MoveTenRightWithVelocity()
    {
        var transform = new Transform();
        Vector2 velocity = new(1, 0);
        Vector2 distanceVelocity = new(10, 0);
        float distance = 10f;

        transform.MoveDistance(distanceVelocity, distance);
        transform.SetVelocity(velocity);
        Vector2 vectorAfterFrame = new(velocity.X + distanceVelocity.X, velocity.Y + distanceVelocity.Y);

        RunTransformFrames(transform, 1);
        Assert.That(new Vector2(transform.LocalX, transform.LocalY), Is.EqualTo(vectorAfterFrame));
    }

    [Test]
    public void MoveDistance_SetTenRightWithVelocity_MovetWithVelocityTwoFrames()
    {
        var transform = new Transform();
        Vector2 velocity = new(5, 0);
        Vector2 distanceVelocity = new(5, 0);
        float distance = 10f;

        transform.MoveDistance(distanceVelocity, distance);
        transform.SetVelocity(velocity);
        // velocity * 2 frames + distanceVelocity only one frame as met distance on first frame
        Vector2 vectorAfterFrame = new(velocity.X * 2 + distanceVelocity.X, velocity.Y * 2 + distanceVelocity.Y);

        RunTransformFrames(transform, 2);
        Assert.That(new Vector2(transform.LocalX, transform.LocalY), Is.EqualTo(vectorAfterFrame));
    }

    [Test]
    public void MoveDistance_SetTenRightWithCounterVelocity_MovetWithVelocityTwoFrames()
    {
        var transform = new Transform();
        Vector2 velocity = new(-10, 0);
        Vector2 distanceVelocity = new(5, 0);
        float distance = 5f;

        transform.MoveDistance(distanceVelocity, distance);
        transform.SetVelocity(velocity);
        // velocity * 2 frames + distanceVelocity only one frame as met distance on first frame
        Vector2 vectorAfterFrame = new(velocity.X * 2 + distanceVelocity.X, velocity.Y * 2 + distanceVelocity.Y);

        RunTransformFrames(transform, 2);
        Assert.That(new Vector2(transform.LocalX, transform.LocalY), Is.EqualTo(vectorAfterFrame));
    }

    [Test]
    public void MoveDistance_SetNegativeDistance_IgnoreDistanceDirectionTwoFrames()
    {
        var transform = new Transform();
        Vector2 distanceVelocity = new(5, 0);
        float distance = -5f;

        transform.MoveDistance(distanceVelocity, distance);
        Vector2 vectorAfter2Frames = new(distanceVelocity.X, distanceVelocity.Y);

        RunTransformFrames(transform, 2);
        Assert.That(new Vector2(transform.LocalX, transform.LocalY), Is.EqualTo(vectorAfter2Frames));
    }

    [Test]
    public void MoveVector_SetTenRight_MoveTenRight()
    {
        var transform = new Transform();
        Vector2 velocity = new(10, 0);
        float distance = 10f;

        transform.MoveVector(velocity, distance);

        Vector2 vectorAfter1Frame = new(velocity.X, velocity.Y);

        RunTransformFrames(transform, 1);
        Assert.That(new Vector2(transform.LocalX, transform.LocalY), Is.EqualTo(vectorAfter1Frame));
    }

    [Test]
    public void MoveVector_SetTenRightWithVelocity_MoveTenRightWithVelocity()
    {
        var transform = new Transform();
        Vector2 velocity = new(1, 0);
        Vector2 distanceVelocity = new(10, 0);
        float distance = 10f;

        transform.MoveVector(distanceVelocity, distance);
        transform.SetVelocity(velocity);
        Vector2 vectorAfterFrame = new(velocity.X + distanceVelocity.X, velocity.Y + distanceVelocity.Y);

        RunTransformFrames(transform, 1);
        Assert.That(new Vector2(transform.LocalX, transform.LocalY), Is.EqualTo(vectorAfterFrame));
    }

    [Test]
    public void MoveVector_SetTenRightWithCounterVelocity_MoveTenRight()
    {
        var transform = new Transform();
        Vector2 velocity = new(-0.5f, 0);
        Vector2 distanceVelocity = new(1f, 0);
        float distance = 10f;

        transform.MoveVector(distanceVelocity, distance);
        transform.SetVelocity(velocity);

        // should be 9.5
        RunTransformFrames(transform, 19);

        // now set velocity to 0.1 in the same direction
        velocity = new(0.1f, 0);
        transform.SetVelocity(velocity);

        // should move 10f and then velocity units in next two frames
        Vector2 vectorAfterFrame = new(distanceVelocity.X * 10 + (velocity.X * 2), 0);
        RunTransformFrames(transform, 2);

        Assert.That(transform.LocalX, Is.EqualTo(vectorAfterFrame.X).Within(0.001f));
        Assert.That(transform.LocalY, Is.EqualTo(vectorAfterFrame.Y).Within(0.001f));

    }

    [Test]
    public void Scale_SetScale_TransformToScale()
    {
        var transform = new Transform();
        double xScaleFactor = 2.0f;
        transform.Scale(xScaleFactor);

        Assert.That(transform._GetTransformation(1), Is.EqualTo(Matrix4.CreateScale((float)xScaleFactor, 1, 1)));
    }
}
