using pong;
using rasdaq.Core.ECS;
using Application = rasdaq.Application;

namespace Pong;

internal class Program
{
    static void Main()
    {
        try
        {
            using Application app = new(800, 600, "Pong");

            World world = new(app);
            Entity soldier = new();
            soldier.AddComponent(new Soldier());

            world.AddEntity(soldier);

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}
