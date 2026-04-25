using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using rasdaq.Inputs;
using OTKKeys = OpenTK.Windowing.GraphicsLibraryFramework.Keys;

internal class GameWindowWrapper : IGameWindow
{
    private readonly GameWindow gameWindow;
    internal GameWindowWrapper(GameWindow gameWindow)
    {
        this.gameWindow = gameWindow;
    }

    public CursorState CursorState { get => gameWindow.CursorState; set => gameWindow.CursorState = value; }
    public Vector2 MousePosition { get => gameWindow.MousePosition; set => gameWindow.MousePosition = value; }

    public bool IsKeyDown(Keys key)
    {
        return gameWindow.IsKeyDown((OTKKeys)key);
    }

    public event Action<KeyboardKeyEventArgs> KeyDown
    {
        add => gameWindow.KeyDown += value; remove => gameWindow.KeyDown -= value;
    }
    public event Action<KeyboardKeyEventArgs> KeyUp
    {
        add => gameWindow.KeyUp += value; remove => gameWindow.KeyUp -= value;
    }

    public event Action<MouseMoveEventArgs> MouseMove
    {
        add => gameWindow.MouseMove += value; remove => gameWindow.MouseMove -= value;
    }

    public event Action<MouseButtonEventArgs> MouseDown
    {
        add => gameWindow.MouseDown += value; remove => gameWindow.MouseDown -= value;
    }
    public event Action<MouseButtonEventArgs> MouseUp
    {
        add => gameWindow.MouseUp += value; remove => gameWindow.MouseUp -= value;
    }
}