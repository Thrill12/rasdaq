using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

public interface IGameWindow
{
    event Action<KeyboardKeyEventArgs> KeyDown;
    event Action<KeyboardKeyEventArgs> KeyUp;
    event Action<MouseMoveEventArgs> MouseMove;
    event Action<MouseButtonEventArgs> MouseDown;
    event Action<MouseButtonEventArgs> MouseUp;

    public CursorState CursorState { get; set; }

    public MouseState MouseState { get; }

}