using NUnit.Framework;
using rasdaq.Core.ECS;

namespace tests;

[TestFixture]
public class EntityTests
{
    [Test]
    public void GetComponent_ReturnsCorrectType()
    {
        var entity = new Entity(Vector3.Zero);
        var tracker = new LifecycleTracker();
        entity.AddComponent(tracker);

        entity.Update(1);

        var result = entity.GetComponent<LifecycleTracker>();

        Assert.That(result != null);
        Assert.That(tracker, Is.EqualTo(result));
    }

    [Test]
    public void GetComponent_ReturnsNull_WhenNotPresent()
    {
        var entity = new Entity(Vector3.Zero);

        var result = entity.GetComponent<LifecycleTracker>();

        Assert.That(result == null);
    }

    [Test]
    public void AddComponent_SetsEntityReference()
    {
        var entity = new Entity(Vector3.Zero);
        var tracker = new LifecycleTracker();
        entity.AddComponent(tracker);

        Assert.That(entity, Is.EqualTo(tracker.Entity));
    }

    [Test]
    public void AddEntityToWorld()
    {
        World world = new();
        Entity entity = new();
        world.AddEntity(entity);

        Assert.That(world.Entities.Contains(entity));
        Assert.That(entity.World, Is.EqualTo(world));
    }
}
