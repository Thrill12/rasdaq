using pong;

namespace Pong;

class Program
{
    private static void Main()
    {
        try
        {
            using Game game = new();

            game.Run(800, 600, "rasdaq");
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
            throw new Exception(ex.Message + "\n Check 'rasdaq.log' for more details. \n" + ex.StackTrace);
        }
    }
}
