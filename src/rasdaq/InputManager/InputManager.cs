using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq.Input;

public class InputManager(IGameWindow gameWindow)
{
    private IGameWindow gameWindow = gameWindow;
    private Dictionary<Keys, Action> _upCallbacks = [];
    public Dictionary<Keys, Action> UpCallbacks => _upCallbacks;

    private Dictionary<Keys, Action> _downCallbacks = [];
    public Dictionary<Keys, Action> DownCallbacks => _downCallbacks;

    private Dictionary<MouseButton, Action> _mButtonUpCallbacks = [];
    public Dictionary<MouseButton, Action> MButtonUpCallbacks => _mButtonUpCallbacks;

    private Dictionary<MouseButton, Action> _mButtonDownCallbacks = [];
    public Dictionary<MouseButton, Action> MButtonDownCallbacks => _mButtonDownCallbacks;

    public Action<MouseMoveEventArgs> mouseMoveAction = e => { };

    static public bool logMouseDelta = false;

    public void AddKeyUpCallback(Keys key, Action inputCallback)
    {
        UpCallbacks.Add(key, inputCallback);
    }

    public void AddKeyDownCallback(Keys key, Action inputCallback)
    {
        DownCallbacks.Add(key, inputCallback);
    }

    public void AddMouseButtonDownCallback(MouseButton mouseButton, Action inputCallback)
    {
        MButtonDownCallbacks.Add(mouseButton, inputCallback);
    }

    public void AddMouseButtonUpCallback(MouseButton mouseButton, Action inputCallback)
    {
        MButtonUpCallbacks.Add(mouseButton, inputCallback);
    }

    internal void SetEventListeners()
    {
        Console.WriteLine("Setting listeners");
        gameWindow.KeyUp += (e) =>
        {
            if (UpCallbacks.ContainsKey(e.Key))
            {
                UpCallbacks[e.Key]();
            }
        };

        gameWindow.KeyDown += (e) =>
        {
            if (DownCallbacks.ContainsKey(e.Key))
            {
                DownCallbacks[e.Key]();
            }
        };

        gameWindow.MouseMove += mouseMoveAction;

        gameWindow.MouseDown += (e) =>
        {
            if (MButtonDownCallbacks.ContainsKey(e.Button))
            {
                MButtonDownCallbacks[e.Button]();
            }
        };

        gameWindow.MouseUp += (e) =>
        {
            if (MButtonUpCallbacks.ContainsKey(e.Button))
            {
                MButtonUpCallbacks[e.Button]();
            }
        };
    }

    public CursorState LockMouse()
    {
        gameWindow.CursorState = CursorState.Grabbed;

        return gameWindow.CursorState;
    }

    public CursorState UnlockMouse()
    {
        gameWindow.CursorState = CursorState.Normal;

        return gameWindow.CursorState;
    }

    public Vector2 GetMousePosition()
    {
        return gameWindow.MousePosition;
    }
}
