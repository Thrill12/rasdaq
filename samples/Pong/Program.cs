using rasdaq.Graphics;
using rasdaq.Graphics.Shaders;
using Application = rasdaq.Application;

namespace Pong;

internal class Program
{
    static void Main(string[] args)
    {
        using (Application app = new Application(800, 600, "Pong"))
        {
            Sprite sprite = new([-0.5f, -0.5f, 0.0f, 0.5f, -0.5f, 0.0f, 0.0f, 0.5f, 0.0f]);
            app.Run();
            sprite.Dispose();
        }
    }
}
