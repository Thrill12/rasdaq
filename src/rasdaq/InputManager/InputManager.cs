using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq.Input;

public class InputManager
{
    private readonly IGameWindow gameWindow;

    /// <summary>
    /// Callback to run upon releasing specified keyboard key
    /// </summary>
    public Dictionary<Keys, Action> KeyUpCallbacks { get; } = [];
    /// <summary>
    /// Callback to run upon pressing down specified keyboard key
    /// </summary>
    public Dictionary<Keys, Action> KeyDownCallbacks { get; } = [];
    /// <summary>
    /// Callback to run upon releasing specified mouse button
    /// </summary>
    public Dictionary<MouseButton, Action> MouseButtonUpCallbacks { get; } = [];
    /// <summary>
    /// Callback to run upon pressing down specified mouse button
    /// </summary>
    public Dictionary<MouseButton, Action> MouseButtonDownCallbacks { get; } = [];
    /// <summary>
    /// Callback to run when mouse moves
    /// </summary>
    public Action<MouseMoveEventArgs> mouseMoveAction = e => { };


    internal InputManager(IGameWindow gameWindow)
    {
        this.gameWindow = gameWindow;
    }

    /// <summary>
    /// Sets defined callbacks to event listeners for keys/mouse
    /// </summary>
    internal void SetEventListeners()
    {
        // Console.WriteLine("Setting listeners");
        gameWindow.KeyUp += (e) =>
        {
            if (KeyUpCallbacks.ContainsKey(e.Key))
            {
                KeyUpCallbacks[e.Key]();
            }
        };

        gameWindow.KeyDown += (e) =>
        {
            if (KeyDownCallbacks.ContainsKey(e.Key))
            {
                KeyDownCallbacks[e.Key]();
            }
        };

        gameWindow.MouseMove += mouseMoveAction;

        gameWindow.MouseDown += (e) =>
        {
            if (MouseButtonDownCallbacks.ContainsKey(e.Button))
            {
                MouseButtonDownCallbacks[e.Button]();
            }
        };

        gameWindow.MouseUp += (e) =>
        {
            if (MouseButtonUpCallbacks.ContainsKey(e.Button))
            {
                MouseButtonUpCallbacks[e.Button]();
            }
        };
    }

    /// <summary>
    /// Locks mouse cursor to center of screen
    /// </summary>
    /// <returns>OpenTK CursorState</returns>
    public CursorState LockMouse()
    {
        gameWindow.CursorState = CursorState.Grabbed;

        return gameWindow.CursorState;
    }

    /// <summary>
    /// Unlocks mouse cursor from center of screen, if locked
    /// </summary>
    /// <returns>OpenTK CursorState</returns>
    public CursorState UnlockMouse()
    {
        gameWindow.CursorState = CursorState.Normal;

        return gameWindow.CursorState;
    }

    /// <summary>
    /// Gets the position of the mouse relative to the content area of this window
    /// </summary>
    /// <returns>Vector2 representing mouse coordinates</returns>
    public Vector2 GetMousePosition()
    {
        return gameWindow.MousePosition;
    }
}