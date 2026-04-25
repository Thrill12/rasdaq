using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using rasdaq.Logging;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("tests")]

namespace rasdaq.Inputs;

internal class InputManager
{
    private readonly IGameWindow gameWindow;

    /// <summary>
    /// Callback to run upon releasing specified keyboard key
    /// </summary>
    public Dictionary<Keys, Action> KeyUpCallbacks { get; set; } = [];
    /// <summary>
    /// Callback to run upon pressing down specified keyboard key
    /// </summary>
    public Dictionary<Keys, Action> KeyDownCallbacks { get; set; } = [];
    /// <summary>
    /// Callback to run upon releasing specified mouse button
    /// </summary>
    public Dictionary<MouseButton, Action> MouseButtonUpCallbacks { get; set; } = [];
    /// <summary>
    /// Callback to run upon pressing down specified mouse button
    /// </summary>
    public Dictionary<MouseButton, Action> MouseButtonDownCallbacks { get; set; } = [];
    /// <summary>
    /// Callback to run when mouse moves
    /// </summary>
    public Action<MouseMoveEvent> mouseMoveAction { get; set; } = e => { };

    internal InputManager(IGameWindow gameWindow)
    {
        this.gameWindow = gameWindow;
    }

    /// <summary>
    /// Sets defined callbacks to event listeners for keys/mouse
    /// </summary>
    internal void SetEventListeners()
    {
        Log.Trace("Setting listeners");
        gameWindow.KeyUp += (e) =>
        {
            if (KeyUpCallbacks.ContainsKey((Keys)e.Key))
            {
                KeyUpCallbacks[(Keys)e.Key]();
            }
        };

        gameWindow.KeyDown += (e) =>
        {
            if (KeyDownCallbacks.ContainsKey((Keys)e.Key))
            {
                KeyDownCallbacks[(Keys)e.Key]();
            }
        };

        gameWindow.MouseMove += mouseMoveAction;

        gameWindow.MouseDown += (e) =>
        {
            if (MouseButtonDownCallbacks.ContainsKey((MouseButton)e.Button))
            {
                MouseButtonDownCallbacks[(MouseButton)e.Button]();
            }
        };

        gameWindow.MouseUp += (e) =>
        {
            if (MouseButtonUpCallbacks.ContainsKey((MouseButton)e.Button))
            {
                MouseButtonUpCallbacks[(MouseButton)e.Button]();
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

    /// <summary>
    /// Returns whether a specific key or set of keys is pressed in the current frame.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public bool IsKeyDown(Keys key)
    {
        return gameWindow.IsKeyDown(key);
    }

    /// <summary>
    /// Returns whether a specific key or set of keys is pressed in the current frame and released in the previous.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public bool IsKeyPressed(Keys key)
    {
        return gameWindow.IsKeyPressed(key);
    }

    /// <summary>
    /// Returns whether a specific key or set of keys is released in the current frame and pressed in the previous.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public bool IsKeyReleased(Keys key)
    {
        return gameWindow.IsKeyReleased(key);
    }

    /// <summary>
    /// Returns whether a specific mouse button is pressed in the current frame.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public bool IsMouseButtonDown(MouseButton button)
    {
        return gameWindow.IsMouseButtonDown(button);
    }

    /// <summary>
    /// Returns whether a specific mouse button is pressed in the current frame and released in the previous.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public bool IsMouseButtonPressed(MouseButton button)
    {
        return gameWindow.IsMouseButtonPressed(button);
    }

    /// <summary>
    /// Returns whether a specific mouse button is released in the current frame and pressed in the previous.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public bool IsMouseButtonReleased(MouseButton button)
    {
        return gameWindow.IsMouseButtonReleased(button);
    }
}