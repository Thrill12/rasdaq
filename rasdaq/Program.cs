internal class Program
{
    static void Main(string[] args)
    {
        using (Application app = new Application(800, 600, "Rasdaq"))
        {
            app.Run();
        }
    }
}