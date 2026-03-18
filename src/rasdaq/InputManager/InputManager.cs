using OpenTK.Graphics.ES20;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace rasdaq.InputManager;

public class InputManager
{
    private GameWindow _gameWindow;
    private Dictionary<char, Action> upCallbacks = [];
    private Dictionary<char, Action> downCallbacks = [];

    public InputManager(GameWindow gameWindow)
    {
        _gameWindow = gameWindow;
    }

    public void AddKeyUpCallback(char key, Action inputCallback)
    {
        upCallbacks.Add(key, inputCallback);
    }

    public void AddKeyDownCallback(char key, Action inputCallback)
    {
        downCallbacks.Add(key, inputCallback);
    }
    
    internal void SetEventListeners()
    {
        _gameWindow.KeyUp += (e) =>
        {
            if (upCallbacks.ContainsKey((char)e.Key))
            {
                upCallbacks[(char)e.Key]();
            }
        };

        _gameWindow.KeyDown += (e) =>
        {
            if (downCallbacks.ContainsKey((char)e.Key))
            {
                downCallbacks[(char)e.Key]();
            }
        };
    }

}
