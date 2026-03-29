using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace rasdaq.Input;

public class InputManager(GameWindow gameWindow)
{
    private GameWindow _gameWindow = gameWindow;

    private Dictionary<Keys, Action> _upCallbacks = [];
    public Dictionary<Keys, Action> UpCallbacks => _upCallbacks;
    
    private Dictionary<Keys, Action> _downCallbacks = [];
    public Dictionary<Keys, Action> DownCallbacks => _downCallbacks;

    private Dictionary<MouseButton, Action> _mButtonUpCallbacks = [];
    public Dictionary<MouseButton, Action> MButtonUpCallbacks => _mButtonUpCallbacks;
    
    private Dictionary<MouseButton, Action> _mButtonDownCallbacks = [];
    public Dictionary<MouseButton, Action> MButtonDownCallbacks => _mButtonDownCallbacks;
    
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
        _gameWindow.KeyUp += (e) =>
        {
            if (UpCallbacks.ContainsKey(e.Key))
            {
                UpCallbacks[e.Key]();
            }
        };

        _gameWindow.KeyDown += (e) =>
        {
            if (DownCallbacks.ContainsKey(e.Key))
            {
                DownCallbacks[e.Key]();
            }
        };

        // placeholder function, what are we supposed to do with mouse movements?
        _gameWindow.MouseMove += GetMouseDelta;

        _gameWindow.MouseDown += (e) =>
        {
            if (MButtonDownCallbacks.ContainsKey(e.Button))
            {
                MButtonDownCallbacks[e.Button]();
            }    
        };
        
        _gameWindow.MouseUp += (e) =>
        {
            if (MButtonUpCallbacks.ContainsKey(e.Button))
            {
                MButtonUpCallbacks[e.Button]();
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
