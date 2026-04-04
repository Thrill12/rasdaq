using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

public interface IGameWindow
{
    event Action<KeyboardKeyEventArgs> KeyDown;
    event Action<KeyboardKeyEventArgs> KeyUp;
    event Action<MouseMoveEventArgs> MouseMove;
    event Action<MouseButtonEventArgs> MouseDown;
    event Action<MouseButtonEventArgs> MouseUp;

    public CursorState CursorState { get; set; }

    public Vector2 MousePosition { get; set; }
}