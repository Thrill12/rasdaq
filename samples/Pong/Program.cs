using Application = rasdaq.Application;

namespace Pong;

internal class Program
{
    static void Main(string[] args)
    {
        using (Application app = new Application(800, 600, "Pong"))
        {
            app.Run();
        }
    }
}
