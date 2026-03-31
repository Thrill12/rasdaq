using pong;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
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

            World world = new();
            Entity soldier = new();
            soldier.AddComponent(new Soldier());

            Sprite spr = new(1.1f, 1.1f, tex);
            soldier.AddComponent(spr);

            world.AddEntity(soldier);

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}
