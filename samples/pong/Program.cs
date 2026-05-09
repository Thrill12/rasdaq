using OpenTK.Windowing.GraphicsLibraryFramework;
using pong;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Logging;
using Application = rasdaq.Application;
using ResourceManager = rasdaq.Resources.ResourceManager;

namespace Pong;

internal class Program
{
    private static bool isMouseLocked = false;
    private static bool logMouseDelta = false;

    private static void TestKey(Application application)
    {
        isMouseLocked = !isMouseLocked;

        if (isMouseLocked)
        {
            application.InputManager.LockMouse();
        }
        else
        {
            application.InputManager.UnlockMouse();
        }

        Log.Info("testing keys: B");
    }

    private static void TestMButton1Down()
    {
        Log.Info("MB1 clicked!");

        logMouseDelta = true;
    }

    private static void TestMButton1Up()
    {
        Log.Info("MB1 released!");
        logMouseDelta = false;
    }

    private static void PrintMouseDelta(float deltaX, float deltaY)
    {
        Log.Info("Delta X: " + deltaX);
        Log.Info("Delta Y: " + deltaY);
    }

    private static void Main()
    {
        try
        {

            using Application app = new(800, 600, "rasdaq");

            Texture tex = new("assets/andrei.png");

            World world = new();
            Entity soldier = new();
            soldier.AddComponent(new Soldier());

            Sprite spr = new(1.1f, 1.1f, tex);
            soldier.AddComponent(spr);

            world.AddEntity(soldier);

            app.InputManager.KeyDownCallbacks.Add(Keys.B, () => TestKey(app));
            app.InputManager.KeyDownCallbacks.Add(Keys.A, () =>
                {
                    Log.Info("X: " + app.InputManager.GetMousePosition().X);
                    Log.Info("Y: " + app.InputManager.GetMousePosition().Y);
                });

            app.InputManager.mouseMoveAction = (e) =>
            {
                if (logMouseDelta)
                {
                    PrintMouseDelta(e.DeltaX, e.DeltaY);
                }
            };

            Log.SetLogLevel(RasdaqLogLevel.Trace);

            app.InputManager.MouseButtonDownCallbacks.Add(MouseButton.Button1, TestMButton1Down);
            app.InputManager.MouseButtonUpCallbacks.Add(MouseButton.Button1, TestMButton1Up);

            string he = ResourceManager.Load<string>("assets/save.txt");
            string he2 = ResourceManager.Load<string>("assets/save.txt");
            Log.Info(he);
            Log.Info(he2);

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
            throw new Exception(ex.Message + "\n Check 'rasdaq.log' for more details.");
        }
    }
}