using NUnit.Framework;
using rasdaq.Core.ECS;
using rasdaq.Transformations;

namespace tests;

public class PhysicsTests
{
    [Test]
    public void Physics_AppliesGravityToBodies()
    {
        World world = new();
        Entity entity = new(Vector3.Zero);
        var physics = new Physics();
        var body = new PhysicsBody();

        entity.AddComponent(body);
        world.AddEntity(entity);

        physics.AddBody(body);

        double initialVelocityY = body.Velocity.Y;

        physics.Update();

        Assert.That(
            body.Velocity.Y,
            Is.EqualTo(initialVelocityY - (float)(physics.Gravity * physics.FixedUpdateTime))
                .Within(0.001)
        );
    }

    [Test]
    public void Physics_CollisionDetects()
    {
        var physics = new Physics();

        var entityA = new Entity();
        var bodyA = new PhysicsBody();
        var colliderA = new BoxCollider(1, 1);
        entityA.AddComponent(bodyA);
        entityA.AddComponent(colliderA);

        var entityB = new Entity();
        var bodyB = new PhysicsBody();
        var colliderB = new BoxCollider(1, 1);
        entityB.AddComponent(bodyB);
        entityB.AddComponent(colliderB);

        physics.AddBody(bodyA);
        physics.AddBody(bodyB);

        // Position entities to collide
        entityA.Transform.position = new Vector2(0, 0);
        entityB.Transform.position = new Vector2(0.5f, 0.5f);

        physics.Update();

        Assert.That(colliderA.Collisions, Contains.Item(colliderB));
        Assert.That(colliderB.Collisions, Contains.Item(colliderA));
    }

    [Test]
    public void Physics_CollisionDoesntDetectNonCollision()
    {
        var physics = new Physics();

        var entityA = new Entity();
        var bodyA = new PhysicsBody();
        var colliderA = new BoxCollider(1, 1);
        entityA.AddComponent(bodyA);
        entityA.AddComponent(colliderA);

        var entityB = new Entity();
        var bodyB = new PhysicsBody();
        var colliderB = new BoxCollider(1, 1);
        entityB.AddComponent(bodyB);
        entityB.AddComponent(colliderB);

        physics.AddBody(bodyA);
        physics.AddBody(bodyB);

        // Position entities to not collide
        entityA.Transform.position = new Vector2(0, 0);
        entityB.Transform.position = new Vector2(2, 2);

        physics.Update();

        Assert.That(colliderA.Collisions, Does.Not.Contain(colliderB));
        Assert.That(colliderB.Collisions, Does.Not.Contain(colliderA));
    }

    [Test]
    public void Physics_DetectsMoreThanTwo()
    {
        var physics = new Physics();

        var entityA = new Entity();
        var bodyA = new PhysicsBody();
        var colliderA = new BoxCollider(1, 1);
        entityA.AddComponent(bodyA);
        entityA.AddComponent(colliderA);

        var entityB = new Entity();
        var bodyB = new PhysicsBody();
        var colliderB = new BoxCollider(1, 1);
        entityB.AddComponent(bodyB);
        entityB.AddComponent(colliderB);

        var entityC = new Entity();
        var bodyC = new PhysicsBody();
        var colliderC = new BoxCollider(1, 1);
        entityC.AddComponent(bodyC);
        entityC.AddComponent(colliderC);

        physics.AddBody(bodyA);
        physics.AddBody(bodyB);
        physics.AddBody(bodyC);

        // Position entities to collide
        entityA.Transform.position = new Vector2(0, 0);
        entityB.Transform.position = new Vector2(0f, 0f);
        entityC.Transform.position = new Vector2(0f, 0f);

        physics.Update();

        Assert.That(colliderA.Collisions, Contains.Item(colliderB));
        Assert.That(colliderA.Collisions, Contains.Item(colliderC));
    }
}
