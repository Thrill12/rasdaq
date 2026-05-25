using NUnit.Framework;
using rasdaq.Core;
using rasdaq.Core.ECS;

namespace tests;

[TestFixture]
public class WorldTests
{
    [Test]
    public void World_CanBeCreated()
    {
        World world = new();
        Assert.That(world != null);
    }

    [Test]
    public void World_AddEntity()
    {
        World world = new();
        GameLoop loop = world.GameLoop;
        Entity ent = new(Vector3.Zero);

        world.AddEntity(ent);

        Assert.That(world.Entities.Count, Is.EqualTo(1));

        Entity ent2 = new(Vector3.Zero);
        Entity ent3 = new(Vector3.Zero);

        world.AddEntity(ent2);
        world.AddEntity(ent3);

        loop.Tick(1);

        Assert.That(world.Entities.Count, Is.EqualTo(3));

        world.RemoveEntity(ent);
        world.RemoveEntity(ent2);
        world.RemoveEntity(ent3);

        loop.Tick(1);

        Assert.That(world.Entities.Count, Is.EqualTo(0));
    }
}
