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
        Sprite spr = new();

        Assert.That(spr != null);
    }
}
