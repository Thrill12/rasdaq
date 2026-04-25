using OpenTK.Mathematics;
using OpenTK.Windowing.Common;

namespace rasdaq.Inputs;

/// <summary>
/// Static Input class for easy access to inputs.
/// </summary>
public static class Input
{
    public static void OnKeyDown(Keys key, Action callback)
    {
        Application.Instance?.InputManager.KeyDownCallbacks[key] = callback;
    }

    public static void OnKeyUp(Keys key, Action callback)
    {
        Application.Instance?.InputManager.KeyUpCallbacks[key] = callback;
    }

    public static void OnMouseButtonDown(MouseButton button, Action callback)
    {
        Application.Instance?.InputManager.MouseButtonDownCallbacks[button] = callback;
    }

    public static void OnMouseButtonUp(MouseButton button, Action callback)
    {
        Application.Instance?.InputManager.MouseButtonUpCallbacks[button] = callback;
    }

    public static void OnMouseMove(Action<(float, float)> callback)
    {
        Application.Instance?.InputManager.mouseMoveAction = callback;
    }

    // And corresponding Remove methods:
    public static void RemoveKeyDown(Keys key)
    {
        Application.Instance?.InputManager.KeyDownCallbacks.Remove(key);
    }

    public static void RemoveKeyUp(Keys key)
    {
        Application.Instance?.InputManager.KeyUpCallbacks.Remove(key);
    }

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
    /// Returns whether a specific key or set of keys is pressed in the current frame and released in the previous.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public static bool IsKeyPressed(Keys key)
    {
        return Application.Instance?.InputManager.IsKeyPressed(key) ?? false;
    }

    /// <summary>
    /// Returns whether a specific key or set of keys is released in the current frame and pressed in the previous.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public static bool IsKeyReleased(Keys key)
    {
        return Application.Instance?.InputManager.IsKeyReleased(key) ?? false;
    }

    /// <summary>
    /// Returns whether a specific mouse button is pressed in the current frame.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public static bool IsMouseButtonDown(MouseButton button)
    {
        return Application.Instance?.InputManager.IsMouseButtonDown(button) ?? false;
    }

    /// <summary>
    /// Returns whether a specific mouse button is pressed in the current frame and released in the previous.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public static bool IsMouseButtonPressed(MouseButton button)
    {
        return Application.Instance?.InputManager.IsMouseButtonPressed(button) ?? false;
    }

    /// <summary>
    /// Returns whether a specific mouse button is released in the current frame and pressed in the previous.
    /// </summary>
    /// <param name="key">Key to check for</param>
    /// <returns></returns>
    public static bool IsMouseButtonReleased(MouseButton button)
    {
        return Application.Instance?.InputManager.IsMouseButtonReleased(button) ?? false;
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