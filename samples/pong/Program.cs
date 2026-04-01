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
            Application.Initialize(780, 680, "rasdaq");
            Texture tex = new("assets/andrei.png");

            World world = new();
            Entity soldier = new();
            soldier.AddComponent(new Soldier());

            Sprite spr = new(1.1f, 1.1f, tex);
            soldier.AddComponent(spr);

            world.AddEntity(soldier);

            Application.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}