using NUnit.Framework;
using rasdaq.Core.ECS;
using rasdaq.Interfaces;

namespace tests;

[TestFixture]
internal class FlushEnumerableTests
{
    [Test]
    public void Flush_AddsCorrectly()
    {
        FlushEnumerable<Entity> Entities = new();

        Entity ent = new();

        Assert.That(Entities.Objects.Count, Is.EqualTo(0));
        Entities.Add(ent);

        Assert.That(Entities.Objects.Count, Is.EqualTo(1));
    }

    [Test]
    public void Flush_AddsMultipleCorrectly()
    {
        FlushEnumerable<Entity> Entities = new();

        Entity ent = new();
        Entity ent2 = new();
        Entity ent3 = new();

        Assert.That(Entities.Objects.Count, Is.EqualTo(0));
        Entities.Add(ent);
        Entities.Add(ent2);
        Entities.Add(ent3);

        Assert.That(Entities.Objects.Count, Is.EqualTo(3));
    }

    [Test]
    public void Flush_RemovesCorrectly()
    {
        FlushEnumerable<Entity> Entities = new();

        Entity ent = new();

        Entities.Add(ent);

        Assert.That(Entities.Objects.Count, Is.EqualTo(1));

        Entities.Remove(ent);

        Assert.That(Entities.Objects.Count, Is.EqualTo(0));
    }

    [Test]
    public void Flush_RemovesMultipleCorrectly()
    {
        FlushEnumerable<Entity> Entities = new();

        Entity ent = new();
        Entity ent2 = new();
        Entity ent3 = new();

        Assert.That(Entities.Objects.Count, Is.EqualTo(0));
        Entities.Add(ent);
        Entities.Add(ent2);
        Entities.Add(ent3);

        Assert.That(Entities.Objects.Count, Is.EqualTo(3));

        Entities.Remove(ent);
        Entities.Remove(ent2);
        Entities.Remove(ent3);

        Assert.That(Entities.Objects.Count, Is.EqualTo(0));
    }
}
