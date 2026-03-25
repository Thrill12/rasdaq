using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace rasdaq.Input;

public class InputManager
{
    private GameWindow _gameWindow;
    private Dictionary<Keys, Action> upCallbacks = [];
    private Dictionary<Keys, Action> downCallbacks = [];

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
    }

    public void LockMouse()
    {
        _gameWindow.CursorState = OpenTK.Windowing.Common.CursorState.Grabbed;
    }

    public void UnlockMouse()
    {
        _gameWindow.CursorState = OpenTK.Windowing.Common.CursorState.Normal;
    }
}
