using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using rasdaq.Inputs;
using Keys = rasdaq.Inputs.Keys;
using MouseButton = rasdaq.Inputs.MouseButton;
using OTKKeys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;
using OTKMouseButton = OpenTK.Windowing.GraphicsLibraryFramework.MouseButton;

internal class GameWindowWrapper : IGameWindow
{
    private readonly GameWindow gameWindow;

    internal GameWindowWrapper(GameWindow gameWindow)
    {
        this.gameWindow = gameWindow;
    }

    public CursorState CursorState
    {
        get => gameWindow.CursorState;
        set => gameWindow.CursorState = value;
    }
    public Vector2 MousePosition
    {
        get => gameWindow.MousePosition;
        set => gameWindow.MousePosition = value;
    }
    public Vector2? WindowSize
    {
        get => gameWindow.Size;
        set => gameWindow.Size = (Vector2i)value;
    }

    public bool IsKeyDown(Keys key)
    {
        return gameWindow.IsKeyDown((OTKKeys)key);
    }

    public bool IsKeyPressed(Keys key)
    {
        return gameWindow.IsKeyPressed((OTKKeys)key);
    }

    public bool IsKeyReleased(Keys key)
    {
        return gameWindow.IsKeyReleased((OTKKeys)key);
    }

    public bool IsMouseButtonDown(MouseButton button)
    {
        return gameWindow.IsMouseButtonDown((OTKMouseButton)button);
    }

    public bool IsMouseButtonPressed(MouseButton button)
    {
        return gameWindow.IsMouseButtonPressed((OTKMouseButton)button);
    }

    public bool IsMouseButtonReleased(MouseButton button)
    {
        return gameWindow.IsMouseButtonReleased((OTKMouseButton)button);
    }

    public event Action<KeyboardKeyEventArgs> KeyDown
    {
        add => gameWindow.KeyDown += value;
        remove => gameWindow.KeyDown -= value;
    }
    public event Action<KeyboardKeyEventArgs> KeyUp
    {
        add => gameWindow.KeyUp += value;
        remove => gameWindow.KeyUp -= value;
    }

    private readonly Dictionary<
        Action<MouseMoveEvent>,
        Action<MouseMoveEventArgs>
    > _mouseMoveWrappers = new();
    public event Action<MouseMoveEvent> MouseMove
    {
        add
        {
            Action<MouseMoveEventArgs> wrapper = args =>
                value(new MouseMoveEvent { dx = args.DeltaX, dy = args.DeltaY });

            _mouseMoveWrappers[value] = wrapper;
            gameWindow.MouseMove += wrapper;
        }
        remove
        {
            if (_mouseMoveWrappers.TryGetValue(value, out Action<MouseMoveEventArgs>? wrapper))
            {
                gameWindow.MouseMove -= wrapper;
                _mouseMoveWrappers.Remove(value);
            }
        }
    }

    public event Action<MouseButtonEventArgs> MouseDown
    {
        add => gameWindow.MouseDown += value;
        remove => gameWindow.MouseDown -= value;
    }
    public event Action<MouseButtonEventArgs> MouseUp
    {
        add => gameWindow.MouseUp += value;
        remove => gameWindow.MouseUp -= value;
    }
}
