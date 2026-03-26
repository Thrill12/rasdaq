using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace rasdaq.Input;

public class InputManager
{
    private GameWindow _gameWindow;
    private Dictionary<Keys, Action> upCallbacks = [];
    private Dictionary<Keys, Action> downCallbacks = [];
    private Dictionary<MouseButton, Action> mButtonUpCallbacks = [];
    private Dictionary<MouseButton, Action> mButtonDownCallbacks = [];
    static public bool logMouseDelta = false;

    public InputManager(GameWindow gameWindow)
    {
        _gameWindow = gameWindow;
    }

    public void AddKeyUpCallback(Keys key, Action inputCallback)
    {
        upCallbacks.Add(key, inputCallback);
    }

    public void AddKeyDownCallback(Keys key, Action inputCallback)
    {
        downCallbacks.Add(key, inputCallback);
    }

    public void AddMouseButtonDownCallback(MouseButton mouseButton, Action inputCallback)
    {
        mButtonDownCallbacks.Add(mouseButton, inputCallback);
    }

    public void AddMouseButtonUpCallback(MouseButton mouseButton, Action inputCallback)
    {
        mButtonUpCallbacks.Add(mouseButton, inputCallback);
    }


    internal void SetEventListeners()
    {
        Console.WriteLine("Setting listeners");
        _gameWindow.KeyUp += (e) =>
        {
            if (upCallbacks.ContainsKey(e.Key))
            {
                upCallbacks[e.Key]();
            }
        };

        _gameWindow.KeyDown += (e) =>
        {
            if (downCallbacks.ContainsKey(e.Key))
            {
                downCallbacks[e.Key]();
            }
        };

        // placeholder function, what are we supposed to do with mouse movements?
        _gameWindow.MouseMove += GetMouseDelta;

        _gameWindow.MouseDown += (e) =>
        {
            if (mButtonDownCallbacks.ContainsKey(e.Button))
            {
                mButtonDownCallbacks[e.Button]();
            }    
        };
        
        _gameWindow.MouseUp += (e) =>
        {
            if (mButtonUpCallbacks.ContainsKey(e.Button))
            {
                mButtonUpCallbacks[e.Button]();
            }
        };
    }

    public void LockMouse()
    {
        _gameWindow.CursorState = OpenTK.Windowing.Common.CursorState.Grabbed;
    }

    public void UnlockMouse()
    {
        _gameWindow.CursorState = OpenTK.Windowing.Common.CursorState.Normal;
    }

    public void GetMouseDelta(OpenTK.Windowing.Common.MouseMoveEventArgs e)
    {
        if (logMouseDelta) 
        {
            Console.WriteLine("x: " + e.DeltaX);
            Console.WriteLine("y: " + e.DeltaY);
        }
    }

}
