using System.Drawing;
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
            float[] verts1 = [-0.5f, -0.5f, 0.0f, 0.5f, -0.5f, 0.0f, 0.0f, 0.5f, 0.0f];
            float[] verts2 = [-0.8f, -0.3f, 0.0f, -0.6f, -0.3f, 0.0f, -0.7f, 0.3f, 0.0f];
            float[] verts3 = [0.8f, -0.3f, 0.0f, 0.6f, -0.3f, 0.0f, 0.7f, 0.3f, 0.0f];

            Sprite middle = new(verts1, Color.Yellow);
            Sprite left = new(verts2, Color.Blue);
            Sprite right = new(verts3, Color.Red);

            app.Run();
        }
    }
}
