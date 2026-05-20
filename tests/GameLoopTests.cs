using NUnit.Framework;
using rasdaq.Core;
using rasdaq.Core.ECS;

namespace tests;

[TestFixture]
public class GameLoopTests
{
    [Test]
    public void GameLoop_Ticks()
    {
        World world = new();
        GameLoop loop = world.GameLoop;

        Entity trackerEntity = new(Vector3.Zero);
        LifecycleTracker tracker = new();
        trackerEntity.AddComponent(tracker);

        world.AddEntity(trackerEntity);

        loop.Tick(loop.MsPerUpdate);
        Assert.That(tracker.updateCounter, Is.EqualTo(1));
        Assert.That(tracker.frameUpdateCounter, Is.EqualTo(1));
        Assert.That(tracker.lateUpdateCounter, Is.EqualTo(1));

        tracker.Reset();

        loop.Tick(loop.MsPerUpdate * 10);
        Assert.That(tracker.updateCounter, Is.EqualTo(10));
        Assert.That(tracker.frameUpdateCounter, Is.EqualTo(1));
        Assert.That(tracker.lateUpdateCounter, Is.EqualTo(1));
    }

    [Test]
    public void GameLoop_MaxUpdates()
    {
        World world = new();
        GameLoop loop = world.GameLoop;

        Entity trackerEntity = new(Vector3.Zero);
        LifecycleTracker tracker = new();
        trackerEntity.AddComponent(tracker);

        world.AddEntity(trackerEntity);

        world.GameLoop.Tick(loop.MsPerUpdate * 100);

        Assert.That(tracker.updateCounter, Is.EqualTo(10));
    }

    [Test]
    public void GameLoop_CorrectOrderOfLifecycle()
    {
        World world = new();
        GameLoop loop = world.GameLoop;

        Entity trackerEntity = new();
        LifecycleTracker tracker = new();
        trackerEntity.AddComponent(tracker);

        world.AddEntity(trackerEntity);

        world.GameLoop.Tick(loop.MsPerUpdate);

        Assert.That(tracker.startCounter, Is.EqualTo(1));
        Assert.That(tracker.updateCounter, Is.EqualTo(1));
        Assert.That(tracker.frameUpdateCounter, Is.EqualTo(1));
        Assert.That(tracker.lateUpdateCounter, Is.EqualTo(1));
    }

    [Test]
    public void GameLoop_StartRunsOnlyOnce()
    {
        World world = new();
        GameLoop loop = world.GameLoop;

        Entity trackerEntity = new();
        LifecycleTracker tracker = new();
        trackerEntity.AddComponent(tracker);

        world.AddEntity(trackerEntity);

        world.GameLoop.Tick(loop.MsPerUpdate);
        world.GameLoop.Tick(loop.MsPerUpdate);
        world.GameLoop.Tick(loop.MsPerUpdate);
        world.GameLoop.Tick(loop.MsPerUpdate);

        Assert.That(tracker.startCounter, Is.EqualTo(1));
        Assert.That(tracker.updateCounter, Is.EqualTo(4));
    }
}

/// <summary>
/// Testing component only for tracking updates.
/// </summary>
internal class LifecycleTracker : Component
{
    public int startCounter;
    public int updateCounter;
    public int frameUpdateCounter;
    public int lateUpdateCounter;

    public override void Start()
    {
        startCounter++;
    }

    public override void Update(double deltaTime)
    {
        updateCounter++;
    }

    public override void FrameUpdate(double deltaTime)
    {
        frameUpdateCounter++;
    }

    public override void LateUpdate(double deltaTime)
    {
        lateUpdateCounter++;
    }

    public void Reset()
    {
        startCounter = 0;
        updateCounter = 0;
        frameUpdateCounter = 0;
        lateUpdateCounter = 0;
    }
}
