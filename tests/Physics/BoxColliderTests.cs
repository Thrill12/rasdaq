using NUnit.Framework;
using rasdaq.Core.ECS;
using rasdaq.Logging;
using rasdaq.Transformations;

namespace tests;

[TestFixture]
public class BoxColliderTests
{
    World world;

    [SetUp]
    public void BoxCollider_Setup_ResetState()
    {
        world = new World();
    }

    private void RunFrames(int frames = 1, float deltaTime = 1)
    {
        for (int frame = 0; frame < frames; frame++)
        {
            world.Update(deltaTime);
        }
    }

    [Test]
    public void Collision_Stay()
    {
        Entity mockEntity = new(Vector3.Zero);
        BoxCollider collider = new(100, 100);
        mockEntity.AddComponent(new PhysicsBody() { ApplyGravity = false });
        mockEntity.AddComponent(collider);

        int collisionStayCount = 0;
        collider.OnCollisionStay += (other) =>
        {
            collisionStayCount++;
        };

        Entity otherEntity = new(Vector3.Zero);
        BoxCollider otherCollider = new(100, 100);
        otherEntity.AddComponent(new PhysicsBody() { ApplyGravity = false });
        otherEntity.AddComponent(otherCollider);

        int otherColliderStayCount = 0;
        otherCollider.OnCollisionStay += (other) =>
        {
            otherColliderStayCount++;
        };

        world.AddEntity(mockEntity);
        world.AddEntity(otherEntity);

        // This adds entities
        RunFrames(1);

        RunFrames(5);

        Assert.That(collisionStayCount, Is.EqualTo(5));
        Assert.That(otherColliderStayCount, Is.EqualTo(5));
    }

    [Test]
    public void Collision_Enter()
    {
        Entity mockEntity = new(Vector3.Zero);
        BoxCollider collider = new(100, 100);
        mockEntity.AddComponent(new PhysicsBody() { ApplyGravity = false });
        mockEntity.AddComponent(collider);

        int collisionEnterCount = 0;
        collider.OnCollisionEnter += (other) =>
        {
            collisionEnterCount++;
        };

        Entity otherEntity = new(Vector3.Zero);
        BoxCollider otherCollider = new(100, 100);
        otherEntity.AddComponent(new PhysicsBody() { ApplyGravity = false });
        otherEntity.AddComponent(otherCollider);

        int otherColliderEnterCount = 0;
        otherCollider.OnCollisionEnter += (other) =>
        {
            otherColliderEnterCount++;
        };

        world.AddEntity(mockEntity);
        world.AddEntity(otherEntity);

        // This adds entities
        RunFrames(1);

        // Enter should fire exactly once regardless of how many frames pass
        RunFrames(5);

        Assert.That(collisionEnterCount, Is.EqualTo(1));
        Assert.That(otherColliderEnterCount, Is.EqualTo(1));
    }

    [Test]
    public void Collision_Exit()
    {
        Entity mockEntity = new(Vector3.Zero);
        BoxCollider collider = new(100, 100);
        PhysicsBody body = new() { ApplyGravity = false };
        mockEntity.AddComponent(body);
        mockEntity.AddComponent(collider);

        int collisionExitCount = 0;
        collider.OnCollisionExit += (other) =>
        {
            collisionExitCount++;
        };

        Entity otherEntity = new(Vector3.Zero);
        BoxCollider otherCollider = new(100, 100);
        otherEntity.AddComponent(new PhysicsBody() { ApplyGravity = false });
        otherEntity.AddComponent(otherCollider);

        int otherColliderExitCount = 0;
        otherCollider.OnCollisionExit += (other) =>
        {
            otherColliderExitCount++;
        };

        world.AddEntity(mockEntity);
        world.AddEntity(otherEntity);

        // This adds entities
        RunFrames(1);

        // Let them collide for a few frames
        RunFrames(3);

        // Move mockEntity far away so collision ends
        mockEntity.Transform.position = new Vector3(1000, 1000, 0);

        RunFrames(1);

        Assert.That(collisionExitCount, Is.EqualTo(1));
        Assert.That(otherColliderExitCount, Is.EqualTo(1));

        // Ensure exit doesn't fire again on subsequent frames
        RunFrames(5);

        Assert.That(collisionExitCount, Is.EqualTo(1));
        Assert.That(otherColliderExitCount, Is.EqualTo(1));
    }
}
