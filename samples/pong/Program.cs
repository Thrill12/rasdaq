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
            Sprite spr = new(1f, 1f, tex);
            Application.SetBackgroundColor(Color.CornflowerBlue);

            app.Update += (deltaTime) =>
            {
                Console.WriteLine($"Update {deltaTime}");
                spr.SetWidthHeight(1, ((float)deltaTime * 100) + 1f);
            };

            app.FrameUpdate += (deltaTime) =>
            {
                Console.WriteLine($"FrameUpdate {deltaTime}");
            };

            app.LateUpdate += (deltaTime) =>
            {
                Console.WriteLine($"LateUpdate {deltaTime}");
            };

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}
