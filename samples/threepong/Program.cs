using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Logging;
using rasdaq.Resources;
using rasdaq.Transformations;
namespace threepong;

class Program
{
    private static void Main()
    {
        try
        {
            using Game game = new();

            game.Run(800, 600, "ThreePong");
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
            throw new Exception(ex.Message + "\n Check 'rasdaq.log' for more details. \n" + ex.StackTrace);
        }
    }
}
