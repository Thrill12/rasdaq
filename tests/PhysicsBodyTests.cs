using NUnit.Framework;
using NUnit.Framework.Internal;
using rasdaq.Core.ECS;
using rasdaq.Transformations;

namespace tests;

class MockEntity(Vector3 v) : Entity(v)
{ }

[TestFixture]
public class PhysicsBodyTests
{
    MockEntity mockEntity;
    PhysicsBody body;

    [SetUp]
    public void Init()
    {
        mockEntity = new MockEntity(Vector3.Zero);
        body = new PhysicsBody();
        mockEntity.AddComponent(body);
    }

    private void RunFrames(int frames = 1, float deltaTime = 1)
    {
        for (int frame = 0; frame < frames; frame++)
        {
            body.Update(deltaTime);
        }
    }

    [Test]
    public void MoveOnce_SetOnceVelocity_MoveOnceVelocity()
    {
        Vector2 moveOnceVelocity = new(100, 0);

        body.MoveOnce(moveOnceVelocity);
        RunFrames(1, 1);
        var newPosition = new Vector3(100, 0, 0);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(newPosition));
    }

    [Test]
    public void MoveOnce_SetOnceVelocity_MoveOnceVelocityIn2Frames()
    {
        Vector2 moveOnceVelocity = new(100, 0);

        body.MoveOnce(moveOnceVelocity);

        // should move only once in the two frames
        Vector3 positionAfter2Frames = new(moveOnceVelocity.X, moveOnceVelocity.Y, 0);
        RunFrames(2, 1);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(positionAfter2Frames));
    }

    [Test]
    public void SetVelocity_SetVelocity_MoveVelocityIn2Seconds()
    {
        Vector2 velocity = new(100, 0);

        body.Velocity = velocity;

        Vector3 positionAfter2Sec = new(velocity.X * 2, velocity.Y * 2);
        RunFrames(1, 2);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(positionAfter2Sec));
    }

    [Test]
    public void MoveDistance_SetTenRight_MoveTenRight()
    {
        Vector2 velocity = new(1, 0);
        float distance = 10f;

        body.MoveDistance(velocity, distance, false);

        Vector3 positionAfter10Sec = new(velocity.X * 10, velocity.Y);
        RunFrames(1, 10);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(positionAfter10Sec));
    }

    [Test]
    public void MoveDistance_SetTenRight_MoveTenRightOverTwoFrames()
    {
        Vector2 velocity = new(5, 0);
        float distance = 10f;

        body.MoveDistance(velocity, distance, false);

        Vector3 vectorAfter2Frames = new(velocity.X * 2, velocity.Y);

        RunFrames(2, 1);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(vectorAfter2Frames));
    }

    [Test]
    public void MoveDistance_SetElevenRight_MoveElevenRightOverThreeFrames()
    {
        Vector2 velocity = new(5, 0);
        float distance = 11f;

        body.MoveDistance(velocity, distance, false);

        Vector3 positionAfter3Frames = new(11, velocity.Y);

        RunFrames(3, 1);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(positionAfter3Frames));
    }

    [Test]
    public void MoveDistance_SetTenRightWithVelocity_MoveTenRightWithVelocity()
    {
        Vector2 velocity = new(1, 0);
        Vector2 distanceVelocity = new(10, 0);
        float distance = 10f;

        body.MoveDistance(distanceVelocity, distance, false);
        body.Velocity = velocity;
        Vector3 positionAfterFrame = new(velocity.X + distanceVelocity.X, velocity.Y + distanceVelocity.Y);

        RunFrames(1, 1);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(positionAfterFrame));
    }

    [Test]
    public void MoveDistance_SetTenRightWithVelocity_MovetWithVelocityTwoFrames()
    {
        Vector2 velocity = new(5, 0);
        Vector2 distanceVelocity = new(5, 0);
        float distance = 10f;

        body.MoveDistance(distanceVelocity, distance, false);
        body.Velocity = velocity;
        // velocity * 2 frames + distanceVelocity only one frame as met distance on first frame
        Vector3 positionAfterFrame = new(velocity.X * 2 + distanceVelocity.X, velocity.Y * 2 + distanceVelocity.Y);

        RunFrames(2, 1);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(positionAfterFrame));
    }

    [Test]
    public void MoveDistance_SetTenRightWithCounterVelocity_MovetWithVelocityTwoFrames()
    {
        Vector2 velocity = new(-10, 0);
        Vector2 distanceVelocity = new(5, 0);
        float distance = 5f;

        body.MoveDistance(distanceVelocity, distance, false);
        body.Velocity = velocity;
        // velocity * 2 frames + distanceVelocity only one frame as met distance on first frame
        Vector3 positionAfterFrame = new(velocity.X * 2 + distanceVelocity.X, velocity.Y * 2 + distanceVelocity.Y);

        RunFrames(2, 1);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(positionAfterFrame));
    }

    [Test]
    public void MoveDistance_SetNegativeDistance_IgnoreDistanceDirectionTwoFrames()
    {
        Vector2 distanceVelocity = new(5, 0);
        float distance = -5f;

        body.MoveDistance(distanceVelocity, distance, false);
        Vector3 positionAfter2Frames = new(distanceVelocity.X, distanceVelocity.Y);

        RunFrames(2, 1);

        Assert.That(mockEntity.Transform.position, Is.EqualTo(positionAfter2Frames));
    }

    [Test]
    public void MoveVector_SetTenRight_MoveTenRight()
    {
        Vector2 velocity = new(10, 0);
        float distance = 10f;

        body.MoveDistance(velocity, distance, true);

        Vector3 vectorAfter1Frame = new(velocity.X, velocity.Y);

        RunFrames(1, 1);
        Assert.That(mockEntity.Transform.position, Is.EqualTo(vectorAfter1Frame));
    }

    [Test]
    public void MoveVector_SetTenRightWithVelocity_MoveTenRightWithVelocity()
    {
        Vector2 velocity = new(1, 0);
        Vector2 distanceVelocity = new(10, 0);
        float distance = 10f;

        body.MoveDistance(distanceVelocity, distance, true);
        body.Velocity = velocity;
        Vector3 vectorAfterFrame = new(velocity.X + distanceVelocity.X, velocity.Y + distanceVelocity.Y);

        RunFrames(1, 1);
        Assert.That(mockEntity.Transform.position, Is.EqualTo(vectorAfterFrame));
    }

    [Test]
    public void MoveVector_SetTenRightWithCounterVelocity_MoveTenRight()
    {
        Vector2 velocity = new(-0.5f, 0);
        Vector2 distanceVelocity = new(1f, 0);
        float distance = 10f;

        body.MoveDistance(distanceVelocity, distance, true);
        body.Velocity = velocity;

        // should be 9.5
        RunFrames(19, 1);

        // now set velocity to 0.1 in the same direction
        velocity = new(0.1f, 0);
        body.Velocity = velocity;

        // should move 10f and then velocity units in next two frames
        Vector3 vectorAfterFrame = new(distanceVelocity.X * 10 + (velocity.X * 2), 0);
        RunFrames(2);

        Assert.That(mockEntity.Transform.position.X, Is.EqualTo(vectorAfterFrame.X).Within(0.001f));
        Assert.That(mockEntity.Transform.position.Y, Is.EqualTo(vectorAfterFrame.Y).Within(0.001f));
        Assert.That(mockEntity.Transform.position.Z, Is.EqualTo(vectorAfterFrame.Z).Within(0.001f));
    }
}
