using NUnit.Framework;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq;
using rasdaq.Input;

namespace tests;

class MockCallbacks
{
    public static void MockCallback()
    {
        
    }
}

[TestFixture]
public class InputManagerTests
{
    [SetUp]
    public void Init()
    {

    }

    [Test]
    public void AddKeyUpCallback_PassValidPair_AddToDict()
    {
        Dictionary<Keys, Action> upCallbacks = [];
        upCallbacks.Add(Keys.A, MockCallbacks.MockCallback);

        var inputManager = new InputManager();
        inputManager.AddKeyUpCallback(Keys.A, MockCallbacks.MockCallback);

        Assert.That(inputManager.UpCallbacks, Is.EqualTo(upCallbacks));
    }

    [Test]
    public void AddKeyDownCallback_PassValidPair_AddToDict()
    {
        Dictionary<Keys, Action> downCallbacks = [];
        downCallbacks.Add(Keys.A, MockCallbacks.MockCallback);

        var inputManager = new InputManager();
        inputManager.AddKeyDownCallback(Keys.A, MockCallbacks.MockCallback);

        Assert.That(inputManager.DownCallbacks, Is.EqualTo(downCallbacks));
    }

    [Test]
    public void AddMouseButtonDownCallback_PassValidPair_AddToDict()
    {
        Dictionary<MouseButton, Action> mButtonDownCallbacks = [];
        mButtonDownCallbacks.Add(MouseButton.Button1, MockCallbacks.MockCallback);

        var inputManager = new InputManager();
        inputManager.AddMouseButtonDownCallback(MouseButton.Button1, MockCallbacks.MockCallback);

        Assert.That(inputManager.MButtonDownCallbacks, Is.EqualTo(mButtonDownCallbacks));
    }

    [Test]
    public void AddMouseButtonUpCallback_PassValidPair_AddToDict()
    {
        Dictionary<MouseButton, Action> mButtonUpCallbacks = [];
        mButtonUpCallbacks.Add(MouseButton.Button1, MockCallbacks.MockCallback);

        var inputManager = new InputManager();
        inputManager.AddMouseButtonUpCallback(MouseButton.Button1, MockCallbacks.MockCallback);

        Assert.That(inputManager.MButtonUpCallbacks, Is.EqualTo(mButtonUpCallbacks));
    }
}
