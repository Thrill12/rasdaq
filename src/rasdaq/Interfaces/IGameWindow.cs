using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using rasdaq.Inputs;

internal interface IGameWindow
{
    event Action<KeyboardKeyEventArgs> KeyDown;
    event Action<KeyboardKeyEventArgs> KeyUp;
    event Action<MouseMoveEvent> MouseMove;
    event Action<MouseButtonEventArgs> MouseDown;
    event Action<MouseButtonEventArgs> MouseUp;

    public CursorState CursorState { get; set; }

    public Vector2 MousePosition { get; set; }
    public bool IsKeyDown(Keys key);
    public bool IsKeyPressed(Keys key);
    public bool IsKeyReleased(Keys key);

    public bool IsMouseButtonDown(MouseButton button);
    public bool IsMouseButtonPressed(MouseButton button);
    public bool IsMouseButtonReleased(MouseButton button);
}
