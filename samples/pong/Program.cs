using rasdaq.Graphics;
using System.Drawing;
using Application = rasdaq.Application;

using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Pong;

internal class Program
{
    static void TestKey()
    {
        Console.WriteLine("testing keys");
    }
    static void Main()
    {
        try
        {
            using Application app = new(800, 600, "Pong");

            Texture tex = new("assets/andrei.png");
            Sprite spr = new(1f, 1.1f, tex);
            Application.SetBackgroundColor(Color.CornflowerBlue);
            app.InputManager.AddKeyDownCallback(Keys.B, TestKey);

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}
