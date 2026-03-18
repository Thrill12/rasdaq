using rasdaq.Graphics;
using System.Drawing;
using Application = rasdaq.Application;

namespace Pong;

internal class Program
{
    static void Main()
    {
        try
        {
            using Application app = new(800, 600, "Pong");

            Texture tex = new("assets/andrei.png");
            Sprite spr = new(2f, 1f, Color.Red);
            Application.SetBackgroundColor(Color.CornflowerBlue);

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}
