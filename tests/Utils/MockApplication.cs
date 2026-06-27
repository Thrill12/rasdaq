using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using rasdaq.Inputs;
using Keys = rasdaq.Inputs.Keys;
using MouseButton = rasdaq.Inputs.MouseButton;

namespace tests;

internal class MockApplication : IGameWindow
{
    public CursorState CursorState { get; set; } = CursorState.Normal;

    public Vector2 MousePosition { get; set; }

    private Vector2? _windowSize = new(800, 600);
    public Vector2? WindowSize
    {
        get => _windowSize;
        set => _windowSize = value;
    }

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
        KeyDown?.Invoke(
            new KeyboardKeyEventArgs(
                (OpenTK.Windowing.GraphicsLibraryFramework.Keys)key,
                0,
                0,
                false
            )
        );
    }

    public void TriggerKeyUp(Keys key)
    {
        KeyUp?.Invoke(
            new KeyboardKeyEventArgs(
                (OpenTK.Windowing.GraphicsLibraryFramework.Keys)key,
                0,
                0,
                false
            )
        );
    }

    public void TriggerMouseButtonDown(MouseButton mouseButton)
    {
        MouseDown?.Invoke(
            new MouseButtonEventArgs(
                (OpenTK.Windowing.GraphicsLibraryFramework.MouseButton)mouseButton,
                InputAction.Press,
                0
            )
        );
    }

    public void TriggerMouseButtonUp(MouseButton mouseButton)
    {
        MouseUp?.Invoke(
            new MouseButtonEventArgs(
                (OpenTK.Windowing.GraphicsLibraryFramework.MouseButton)mouseButton,
                InputAction.Release,
                0
            )
        );
    }

    public void TriggerMouseMove(MouseMoveEvent mouseMoveData)
    {
        MouseMove?.Invoke(mouseMoveData);
    }
}
