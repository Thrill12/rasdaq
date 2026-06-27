using NUnit.Framework;
using rasdaq.Transformations;

namespace tests;

[TestFixture]
public class CameraTests
{
    Camera camera;

    [SetUp]
    public void Camera_Setup_ResetState()
    {
        camera = new Camera();
    }

    [Test]
    public void SetCameraPosition_SetValidCoordinates_UpdateCameraPosition()
    {
        camera.Position = new Vector2(10f, 20f);

        Assert.Multiple(() =>
        {
            Assert.That(camera._Position.X, Is.EqualTo(10f));
            Assert.That(camera._Position.Y, Is.EqualTo(20f));
        });
    }
}
