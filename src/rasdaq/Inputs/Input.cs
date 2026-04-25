using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace rasdaq.Inputs;

/// <summary>
/// Static Input class for easy access to inputs.
/// </summary>
public static class Input
{
    /// <summary>
    /// Returns whether a specific key or set of keys is pressed in the current frame.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public static bool IsKeyDown(Keys key)
    {
        return Application.Instance?.InputManager.IsKeyDown(key) ?? false;
    }

    /// <summary>
    /// Gets the position of the mouse relative to the content area of this window
    /// </summary>
    /// <returns>Vector2 representing mouse coordinates</returns>
    public static Vector2 GetMousePosition()
    {
        return Application.Instance?.InputManager.GetMousePosition() ?? Vector2.Zero;
    }

    /// <summary>
    /// Locks mouse cursor to center of screen
    /// </summary>
    /// <returns>OpenTK CursorState</returns>
    public static CursorState LockMouse()
    {
        return Application.Instance?.InputManager.LockMouse() ?? CursorState.Normal;
    }

    /// <summary>
    /// Unlocks mouse cursor from center of screen, if locked
    /// </summary>
    /// <returns>OpenTK CursorState</returns>
    public static CursorState UnlockMouse()
    {
        return Application.Instance?.InputManager.UnlockMouse() ?? CursorState.Normal;
    }
}