using rasdaq.Graphics;
using System.Drawing;
using Application = rasdaq.Application;

using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL;
using rasdaq.Input;

namespace Pong;

internal class Program
{
    static void TestKey(Application application, bool isMouseLocked)
    {
        if (isMouseLocked)
        {
            application.InputManager.LockMouse();
        } else
        {
            application.InputManager.UnlockMouse();
        }
        Console.WriteLine("testing keys: B");
    }

    static void TestMButton1()
    {
        Console.WriteLine("MB1 clicked!");

        InputManager.logMouseDelta = true;
    }

    static void TestMButton1Up()
    {
        Console.WriteLine("MB1 released!");
        InputManager.logMouseDelta = false;
    }

    static void Main()
    {
        try
        {
            using Application app = new(800, 600, "Pong");
            bool lockedMouse = false;

            Texture tex = new("assets/andrei.png");
            Sprite spr = new(1f, 1.1f, tex);
            Application.SetBackgroundColor(Color.CornflowerBlue);
            
            app.InputManager.AddKeyDownCallback(Keys.B, () =>
            {
                TestKey(app, lockedMouse); lockedMouse = !lockedMouse;
            });

            app.InputManager.AddMouseButtonDownCallback(MouseButton.Button1, TestMButton1);
            app.InputManager.AddMouseButtonUpCallback(MouseButton.Button1, TestMButton1Up);

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}
