using pong;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
using Application = rasdaq.Application;
using Keys = rasdaq.Inputs.Keys;
using MouseButton = rasdaq.Inputs.MouseButton;

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
            Input.LockMouse();
        }
        else
        {
            Input.UnlockMouse();
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

            Input.OnKeyDown.Add(Keys.B, () => TestKey(app));
            Input.OnKeyDown.Add(Keys.A, () =>
                {
                    Log.Info("X: " + Input.GetMousePosition().X);
                    Log.Info("Y: " + Input.GetMousePosition().Y);
                });

            Input.OnMouseMove = (e) =>
            {
                if (logMouseDelta)
                {
                    PrintMouseDelta(e.dx, e.dy);
                }
            };

            Input.OnMouseButtonDown.Add(MouseButton.Button1, TestMButton1Down);
            Input.OnMouseButtonUp.Add(MouseButton.Button1, TestMButton1Up);

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}