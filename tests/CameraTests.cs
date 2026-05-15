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
        camera.SetPosition(10f, 20f);

        Assert.Multiple(() =>
        {
            Assert.That(camera.Position.X, Is.EqualTo(10f));
            Assert.That(camera.Position.Y, Is.EqualTo(20f));
        });

    }

    // [Test]
    // public void MoveCameraPosition_SetOffset_OffsetFromCameraPosition()
    // {
    //     camera.SetPosition(5f, 5f);

    //     camera.SetPosition(-2f, -3f);

    //     Assert.Multiple(() =>
    //     {
    //         Assert.That(camera.Position.X, Is.EqualTo(3f));
    //         Assert.That(camera.Position.Y, Is.EqualTo(2f));
    //     });
    // }

    // [Test]
    // public void MoveCameraPosition_SetNullX_OnlyUpdateY()
    // {
    //     camera.SetPosition(5f, 5f);

    //     Camera.MoveCameraPosition(null, 2f);

    //     Assert.Multiple(() =>
    //     {
    //         Assert.That(Camera.CameraPosition.X, Is.EqualTo(5f));
    //         Assert.That(Camera.CameraPosition.Y, Is.EqualTo(7f));
    //     });
    // }

    // [Test]
    // public void MoveCameraPosition_SetNullY_OnlyUpdateX()
    // {
    //     Camera.SetCameraPosition(5f, 5f);

    //     Camera.MoveCameraPosition(2f, null);

    //     Assert.Multiple(() =>
    //     {
    //         Assert.That(Camera.CameraPosition.X, Is.EqualTo(7f));
    //         Assert.That(Camera.CameraPosition.Y, Is.EqualTo(5f));
    //     });

    // }

    // [Test]
    // public void MoveCameraPosition_SetNullOffsets_DoNotChangePosition()
    // {
    //     Camera.SetCameraPosition(8f, 9f);

    //     Camera.MoveCameraPosition(null, null);

    //     Assert.Multiple(() =>
    //     {
    //         Assert.That(Camera.CameraPosition.X, Is.EqualTo(8f));
    //         Assert.That(Camera.CameraPosition.Y, Is.EqualTo(9f));
    //     });

    // }
}
