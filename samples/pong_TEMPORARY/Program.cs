using System.Drawing;
using rasdaq.Graphics;
using Application = rasdaq.Application;

namespace Pong;

internal class Program
{
    static void Main()
    {
        using Application app = new(800, 600, "Pong");

        Sprite spr = new(2f, 1f, "samples/pong/assets/div.jpeg");
        Application.SetBackgroundColor(Color.CornflowerBlue);

        app.Run();
    }
}
