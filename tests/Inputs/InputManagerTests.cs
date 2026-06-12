using NUnit.Framework;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq.Inputs;
using Keys = rasdaq.Inputs.Keys;
using MouseButton = rasdaq.Inputs.MouseButton;

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
        {
            return false;
        }

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

    public event Action<MouseMoveEvent>? MouseMove;
    public event Action<MouseButtonEventArgs>? MouseDown;
    public event Action<MouseButtonEventArgs>? MouseUp;

    public List<Keys> currentKeysBeingPressed = new();
    public List<MouseButton> currentMouseButtonsBeingPressed = new();

    public void SetPressedKeys(Keys newKeys)
    {
        currentKeysBeingPressed.Add(newKeys);
    }

    public void SetMouseButtonPressed(MouseButton newButton)
    {
        currentMouseButtonsBeingPressed.Add(newButton);
    }

    public bool IsKeyDown(Keys key)
    {
        return currentKeysBeingPressed.Contains(key);
    }

    public bool IsKeyPressed(Keys key)
    {
        return currentKeysBeingPressed.Contains(key);
    }

    public bool IsKeyReleased(Keys key)
    {
        return !currentKeysBeingPressed.Contains(key);
    }

    public bool IsMouseButtonDown(MouseButton button)
    {
        return currentMouseButtonsBeingPressed.Contains(button);
    }

    public bool IsMouseButtonPressed(MouseButton button)
    {
        return currentMouseButtonsBeingPressed.Contains(button);
    }

    public bool IsMouseButtonReleased(MouseButton button)
    {
        return !currentMouseButtonsBeingPressed.Contains(button);
    }

    public void TriggerKeyDown(Keys key)
    {
        KeyDown?.Invoke(new KeyboardKeyEventArgs((OpenTK.Windowing.GraphicsLibraryFramework.Keys)key, 0, 0, false));
    }

    public void TriggerKeyUp(Keys key)
    {
        KeyUp?.Invoke(new KeyboardKeyEventArgs((OpenTK.Windowing.GraphicsLibraryFramework.Keys)key, 0, 0, false));
    }

    public void TriggerMouseButtonDown(MouseButton mouseButton)
    {
        MouseDown?.Invoke(new MouseButtonEventArgs((OpenTK.Windowing.GraphicsLibraryFramework.MouseButton)mouseButton, InputAction.Press, 0));
    }

    public void TriggerMouseButtonUp(MouseButton mouseButton)
    {
        MouseUp?.Invoke(new MouseButtonEventArgs((OpenTK.Windowing.GraphicsLibraryFramework.MouseButton)mouseButton, InputAction.Release, 0));
    }

    public void TriggerMouseMove(MouseMoveEvent mouseMoveData)
    {
        MouseMove?.Invoke(mouseMoveData);
    }
}

[TestFixture]
public class InputManagerTests
{
    private MockApplication mockApplication = new();

    [SetUp]
    public void Init()
    {
        mockApplication.currentKeysBeingPressed.Clear();
        mockApplication.currentMouseButtonsBeingPressed.Clear();
    }

    [Test]
    public void SetEventListeners_InvokeKeyDown_InvokeKeyDownCallback()
    {
        bool isKeyDown = false;

        InputManager inputManager = new(mockApplication);
        inputManager.KeyDownCallbacks.Add(Keys.A, () => isKeyDown = true);
        inputManager.SetEventListeners();

        mockApplication.TriggerKeyDown(Keys.A);

        Assert.That(isKeyDown, Is.True);
    }

    [Test]
    public void SetEventListeners_InvokeKeyUp_InvokeKeyUpCallback()
    {
        bool isKeyUp = false;

        InputManager inputManager = new(mockApplication);
        inputManager.KeyUpCallbacks.Add(Keys.A, () => isKeyUp = true);
        inputManager.SetEventListeners();

        mockApplication.TriggerKeyUp(Keys.A);

        Assert.That(isKeyUp, Is.True);
    }

    [Test]
    public void SetEventListeners_InvokeMouseDown_InvokeMouseDownCallback()
    {
        bool isMouseDown = false;

        InputManager inputManager = new(mockApplication);
        inputManager.MouseButtonDownCallbacks.Add(MouseButton.Button1, () => isMouseDown = true);
        inputManager.SetEventListeners();

        mockApplication.TriggerMouseButtonDown(MouseButton.Button1);

        Assert.That(isMouseDown, Is.True);
    }

    [Test]
    public void SetEventListeners_InvokeMouseUp_InvokeMouseUpCallback()
    {
        bool isMouseUp = false;

        InputManager inputManager = new(mockApplication);
        inputManager.MouseButtonUpCallbacks.Add(MouseButton.Button1, () => isMouseUp = true);
        inputManager.SetEventListeners();

        mockApplication.TriggerMouseButtonUp(MouseButton.Button1);

        Assert.That(isMouseUp, Is.True);
    }

    [Test]
    public void SetEventListeners_InvokeMouseMove_InvokeMouseMoveCallback()
    {
        MouseMoveEvent inputMouseMoveData = new() { dx = 0, dy = 0 };
        MouseMoveEvent verifiedMouseMoveData = new() { dx = 70, dy = 30 };

        InputManager inputManager = new(mockApplication)
        {
            mouseMoveAction = (e) =>
            {
                inputMouseMoveData.dx = e.dx;
                inputMouseMoveData.dy = e.dy;
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
    public void InputManager_IsKeyDown()
    {
        InputManager inputManager = new(mockApplication);

        mockApplication.currentKeysBeingPressed.Add(Keys.A);

        Assert.That(inputManager.IsKeyDown(Keys.A), Is.True);
        Assert.That(inputManager.IsKeyDown(Keys.B), Is.False);
    }

    [Test]
    public void InputManager_IsMouseButtonDown()
    {
        InputManager inputManager = new(mockApplication);

        mockApplication.currentMouseButtonsBeingPressed.Add(MouseButton.Left);

        Assert.That(inputManager.IsMouseButtonDown(MouseButton.Left), Is.True);
        Assert.That(inputManager.IsMouseButtonDown(MouseButton.Right), Is.False);
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
