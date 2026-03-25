using rasdaq.Graphics;
using System.Drawing;
using Application = rasdaq.Application;

using OpenTK.Windowing.GraphicsLibraryFramework;

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

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}
