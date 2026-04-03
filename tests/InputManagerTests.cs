using NUnit.Framework;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq;
using rasdaq.Input;

namespace tests;

internal class MockCallbacks
{
    public static void MockCallback()
    {

    }
}

internal class MouseMoveData(float X, float Y, float DeltaX, float DeltaY)
{
    public float X = X;
    public float Y = Y;
    public float deltaX = DeltaX;
    public float deltaY = DeltaY;

    public override bool Equals(object? obj)
    {
        if (obj is not MouseMoveData)
            return false;
        MouseMoveData other = (MouseMoveData)obj;

        return X == other.X &&
            Y == other.Y &&
            deltaX == other.deltaX &&
            deltaY == other.deltaY;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, deltaX, deltaY);
    }
}


internal class MockApplication : IGameWindow
{
    public CursorState CursorState { get; set; } = CursorState.Normal;

    public Vector2 MousePosition { get; set; }

    public event Action<KeyboardKeyEventArgs>? KeyDown;

    public event Action<KeyboardKeyEventArgs>? KeyUp;

    public event Action<MouseMoveEventArgs>? MouseMove;
    public event Action<MouseButtonEventArgs>? MouseDown;
    public event Action<MouseButtonEventArgs>? MouseUp;


    public void TriggerKeyDown(Keys key)
    {
        KeyDown?.Invoke(new KeyboardKeyEventArgs(key, 0, 0, false));
    }

    public void TriggerKeyUp(Keys key)
    {
        KeyUp?.Invoke(new KeyboardKeyEventArgs(key, 0, 0, false));
    }

    public void TriggerMouseButtonDown(MouseButton mouseButton)
    {
        MouseDown?.Invoke(new MouseButtonEventArgs(mouseButton, InputAction.Press, 0));
    }

    public void TriggerMouseButtonUp(MouseButton mouseButton)
    {
        MouseUp?.Invoke(new MouseButtonEventArgs(mouseButton, InputAction.Release, 0));
    }

    public void TriggerMouseMove(MouseMoveData mouseMoveData)
    {
        MouseMove?.Invoke(
            new MouseMoveEventArgs(
                mouseMoveData.X,
                mouseMoveData.Y,
                mouseMoveData.deltaX,
                mouseMoveData.deltaY
            )
        );
    }
}

[TestFixture]
public class InputManagerTests
{
    MockApplication mockApplication = new();

    [SetUp]
    public void Init()
    {

    }

    [Test]
    public void AddKeyUpCallback_PassValidPair_AddToDict()
    {
        Dictionary<Keys, Action> upCallbacks = [];
        upCallbacks.Add(Keys.A, MockCallbacks.MockCallback);

        var inputManager = new InputManager(mockApplication);
        inputManager.AddKeyUpCallback(Keys.A, MockCallbacks.MockCallback);

        Assert.That(inputManager.UpCallbacks, Is.EqualTo(upCallbacks));
    }

    [Test]
    public void AddKeyDownCallback_PassValidPair_AddToDict()
    {
        Dictionary<Keys, Action> downCallbacks = [];
        downCallbacks.Add(Keys.A, MockCallbacks.MockCallback);

        var inputManager = new InputManager(mockApplication);
        inputManager.AddKeyDownCallback(Keys.A, MockCallbacks.MockCallback);

        Assert.That(inputManager.DownCallbacks, Is.EqualTo(downCallbacks));
    }

    [Test]
    public void AddMouseButtonDownCallback_PassValidPair_AddToDict()
    {
        Dictionary<MouseButton, Action> mButtonDownCallbacks = [];
        mButtonDownCallbacks.Add(MouseButton.Button1, MockCallbacks.MockCallback);

        var inputManager = new InputManager(mockApplication);
        inputManager.AddMouseButtonDownCallback(MouseButton.Button1, MockCallbacks.MockCallback);

        Assert.That(inputManager.MButtonDownCallbacks, Is.EqualTo(mButtonDownCallbacks));
    }

    [Test]
    public void AddMouseButtonUpCallback_PassValidPair_AddToDict()
    {
        Dictionary<MouseButton, Action> mButtonUpCallbacks = [];
        mButtonUpCallbacks.Add(MouseButton.Button1, MockCallbacks.MockCallback);

        var inputManager = new InputManager(mockApplication);
        inputManager.AddMouseButtonUpCallback(MouseButton.Button1, MockCallbacks.MockCallback);

        Assert.That(inputManager.MButtonUpCallbacks, Is.EqualTo(mButtonUpCallbacks));
    }

    [Test]
    public void SetEventListeners_InvokeKeyDown_InvokeKeyDownCallback()
    {
        bool isKeyDown = false;

        var inputManager = new InputManager(mockApplication);
        inputManager.AddKeyDownCallback(Keys.A, () => isKeyDown = true);
        inputManager.SetEventListeners();

        mockApplication.TriggerKeyDown(Keys.A);

        Assert.That(isKeyDown, Is.True);
    }

    [Test]
    public void SetEventListeners_InvokeKeyUp_InvokeKeyUpCallback()
    {
        bool isKeyUp = false;

        var inputManager = new InputManager(mockApplication);
        inputManager.AddKeyUpCallback(Keys.A, () => isKeyUp = true);
        inputManager.SetEventListeners();

        mockApplication.TriggerKeyUp(Keys.A);

        Assert.That(isKeyUp, Is.True);
    }

    [Test]
    public void SetEventListeners_InvokeMouseDown_InvokeMouseDownCallback()
    {
        bool isMouseDown = false;

        var inputManager = new InputManager(mockApplication);
        inputManager.AddMouseButtonDownCallback(MouseButton.Button1, () => isMouseDown = true);
        inputManager.SetEventListeners();

        mockApplication.TriggerMouseButtonDown(MouseButton.Button1);

        Assert.That(isMouseDown, Is.True);
    }

    [Test]
    public void SetEventListeners_InvokeMouseUp_InvokeMouseUpCallback()
    {
        bool isMouseUp = false;

        var inputManager = new InputManager(mockApplication);
        inputManager.AddMouseButtonUpCallback(MouseButton.Button1, () => isMouseUp = true);
        inputManager.SetEventListeners();

        mockApplication.TriggerMouseButtonUp(MouseButton.Button1);

        Assert.That(isMouseUp, Is.True);
    }

    [Test]
    public void SetEventListeners_InvokeMouseMove_InvokeMouseMoveCallback()
    {
        MouseMoveData inputMouseMoveData = new(0, 0, 0, 0);
        MouseMoveData verifiedMouseMoveData = new(30, 70, 30, 70);

        var inputManager = new InputManager(mockApplication)
        {
            mouseMoveAction = (e) =>
            {
                inputMouseMoveData.X = e.X;
                inputMouseMoveData.Y = e.Y;
                inputMouseMoveData.deltaX = e.DeltaX;
                inputMouseMoveData.deltaY = e.DeltaY;
            }
        };

        inputManager.SetEventListeners();

        mockApplication.TriggerMouseMove(verifiedMouseMoveData);

        Assert.That(inputMouseMoveData, Is.EqualTo(verifiedMouseMoveData));
    }

    [Test]
    public void LockMouse_Call_SetGrabbedCursorState()
    {
        InputManager inputManager = new(mockApplication);

        Assert.That(inputManager.LockMouse(), Is.EqualTo(CursorState.Grabbed));
    }

    [Test]
    public void UnlockMouse_Call_SetNormalCursorState()
    {
        InputManager inputManager = new(mockApplication);

        Assert.That(inputManager.UnlockMouse(), Is.EqualTo(CursorState.Normal));
    }

    [Test]
    public void GetMousePosition_Call_GetVector2MousePosition()
    {
        InputManager inputManager = new(mockApplication);
        Vector2 verifiedVector = new(30, 60);
        mockApplication.MousePosition = verifiedVector;

        Assert.That(inputManager.GetMousePosition(), Is.EqualTo(verifiedVector));
    }

}
