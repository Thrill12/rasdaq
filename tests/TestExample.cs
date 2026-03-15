using rasdaq.Graphics;

namespace tests;

public class TestExample
{
    [SetUp]
    public void Setup()
    {
        // Setup before each test is run
    }

    [Test]
    public void ShouldCreateSpriteClass()
    {
        Sprite spr = new(1f, 1f);

        Assert.That(spr != null);
    }
}
