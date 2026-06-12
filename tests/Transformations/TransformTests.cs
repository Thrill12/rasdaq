using NUnit.Framework;
using NUnit.Framework.Internal;
using OpenTK.Mathematics;
using rasdaq.Logging;
using rasdaq.Transformations;

namespace tests;

[TestFixture]
public class TransformTests
{
    Transform transform;

    [SetUp]
    public void Init()
    {
        transform = new(Vector3.Zero);
    }
    void AssertMatrixEqual(Matrix4 actual, Matrix4 expected, float epsilon = 0.0001f)
    {
        Assert.That(actual.M11, Is.EqualTo(expected.M11).Within(epsilon));
        Assert.That(actual.M12, Is.EqualTo(expected.M12).Within(epsilon));
        Assert.That(actual.M13, Is.EqualTo(expected.M13).Within(epsilon));
        Assert.That(actual.M14, Is.EqualTo(expected.M14).Within(epsilon));

        Assert.That(actual.M21, Is.EqualTo(expected.M21).Within(epsilon));
        Assert.That(actual.M22, Is.EqualTo(expected.M22).Within(epsilon));
        Assert.That(actual.M23, Is.EqualTo(expected.M23).Within(epsilon));
        Assert.That(actual.M24, Is.EqualTo(expected.M24).Within(epsilon));

        Assert.That(actual.M31, Is.EqualTo(expected.M31).Within(epsilon));
        Assert.That(actual.M32, Is.EqualTo(expected.M32).Within(epsilon));
        Assert.That(actual.M33, Is.EqualTo(expected.M33).Within(epsilon));
        Assert.That(actual.M34, Is.EqualTo(expected.M34).Within(epsilon));

        Assert.That(actual.M41, Is.EqualTo(expected.M41).Within(epsilon));
        Assert.That(actual.M42, Is.EqualTo(expected.M42).Within(epsilon));
        Assert.That(actual.M43, Is.EqualTo(expected.M43).Within(epsilon));
        Assert.That(actual.M44, Is.EqualTo(expected.M44).Within(epsilon));
    }

    [Test]
    public void Get2DRotation_90ZRotation_Return90ZMatrix()
    {
        var angle = 90;
        transform.rotation = angle;
        var correctMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
        Assert.That(transform.Get2DRotation(), Is.EqualTo(correctMatrix));
    }

    [Test]
    public void ScaleFromNDC_ScaleByWidthHeightOnly_ReturnWidthHeightScaleMatrix()
    {
        var width = 10;
        var height = 10;

        var correctMatrix = Matrix4.CreateScale(width, height, 1.0f);

        Assert.That(transform.ScaleFromNDC(width, height), Is.EqualTo(correctMatrix));
    }

    [Test]
    public void ScaleFromNDC_ScaleWithCustomValues_ReturnCustomScaleMatrix()
    {
        var width = 10;
        var height = 10;
        transform.scale = new Vector2(2, 2);

        var correctMatrix = Matrix4.CreateScale(width, height, 1.0f) * Matrix4.CreateScale((float)transform.scale.X, (float)transform.scale.Y, 1.0f);

        Assert.That(transform.ScaleFromNDC(width, height), Is.EqualTo(correctMatrix));
    }

    [Test]
    public void Translate_SetPosition_ReturnTransformToPositionMatrix()
    {
        var position = new Vector3(10, 10, 0);
        transform.position = position;
        var correctMatrix = Matrix4.CreateTranslation((float)position.X, (float)position.Y, (float)position.Z);

        Assert.That(transform.Translate(), Is.EqualTo(correctMatrix));
    }

    [Test]
    public void GetRenderedTransform_SetPositionOnly_OnlyTranslate()
    {
        var position = new Vector3(10, 10, 0);
        transform.position = position;
        var correctMatrix = Matrix4.CreateTranslation((float)position.X, (float)position.Y, (float)position.Z);

        Assert.That(transform.GetRenderedTransform(1, 1), Is.EqualTo(correctMatrix));
    }

    [Test]
    public void GetRenderedTransform_SetPositionRotation_RotateThenTranslate()
    {
        var position = new Vector3(10, 10, 0);
        transform.position = position;
        transform.rotation = 90;

        // 90 degree rotation around Z axis matrix
        // [0 -1 0 0]
        // [1  0 0 0]
        // [0  0 1 0]
        // [0  0 0 1]
        // i-hat (x base vector) transforms to (0, 1) and j-hat (y base vector) transforms to (-1, 0)
        // ...hence 90 deg rotation around z-axis

        // translation matrix
        // [1 0 0 10]
        // [0 1 0 10]
        // [0 0 1  0]
        // [0 0 0  1]

        var correctMatrix = new Matrix4(
            0, -1, 0, 10,
            1, 0, 0, 10,
            0, 0, 1, 0,
            0, 0, 0, 1
        );

        // need to transpose since openTK uses row major
        AssertMatrixEqual(transform.GetRenderedTransform(1, 1), correctMatrix.Transposed());
    }

    [Test]
    public void GetRenderedTransform_SetPositionScale_ScaleThenTranslate()
    {
        var position = new Vector3(10, 10, 0);
        var scale = new Vector2(2, 2);
        transform.position = position;
        transform.scale = scale;

        var scaleMatrix = Matrix4.CreateScale((float)scale.X, (float)scale.Y, 1.0f);
        var translateMatrix = Matrix4.CreateTranslation((float)position.X, (float)position.Y, (float)position.Z);

        // actual order of transformation with column major convention
        var correctMatrix = translateMatrix.Transposed() * scaleMatrix.Transposed();

        // need to transpose back since openTK uses row major
        AssertMatrixEqual(transform.GetRenderedTransform(1, 1), correctMatrix.Transposed());
    }
}
