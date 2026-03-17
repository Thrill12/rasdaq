using rasdaq.Graphics;
using System.Drawing;
using Application = rasdaq.Application;

namespace Pong;

internal class Program
{
    static void Main()
    {
        using Application app = new(800, 600, "Pong");

        Texture tex = new("samples/pong/assets/andrei.png");
        Sprite spr = new(2f, 1f, tex);
        Application.SetBackgroundColor(Color.CornflowerBlue);

            app.Run();
        }
    }
}
