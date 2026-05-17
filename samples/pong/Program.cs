using pong;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
using rasdaq.Transformations;
using Application = rasdaq.Application;
using Keys = rasdaq.Inputs.Keys;
using MouseButton = rasdaq.Inputs.MouseButton;
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
            Entity soldier = new(new Vector3(400, 0, 100));
            Entity enemy = new(new Vector3(0, 0, 0));

            // TODO remove soldier from constructor, this was for testing purposes only
            using Application app = new(800, 600, "rasdaq");

            Texture tex = ResourceManager.Load<Texture>("assets/andrei.png");
            Texture evilTex = ResourceManager.Load<Texture>("assets/evil_enemy.jpg");

            World world = new();

            soldier.AddComponent(new Soldier());
            enemy.AddComponent(new Enemy());

            Sprite spr = new(400, 400, tex);
            Sprite evilSpr = new(400, 400, evilTex);

            soldier.AddComponent(spr);
            enemy.AddComponent(evilSpr);

            world.AddEntity(soldier);
            world.AddEntity(enemy);

            soldier.AddComponent(new PhysicsBody());

            // Input.OnKeyDownEvent.Add(Keys.B, () => TestKey(app));
            Input.OnKeyDownEvent.Add(Keys.A, () =>
                {
                    Log.Info("X: " + Input.GetMousePosition().X);
                    Log.Info("Y: " + Input.GetMousePosition().Y);
                });

            Input.OnMouseMoveEvent = (e) =>
            {
                if (logMouseDelta)
                {
                    PrintMouseDelta(e.dx, e.dy);
                }
            };

            Input.OnMouseButtonDownEvent.Add(MouseButton.Button1, TestMButton1Down);
            Input.OnMouseButtonUpEvent.Add(MouseButton.Button1, TestMButton1Up);

            string save1 = ResourceManager.Load<string>("assets/save.txt");
            string save2 = ResourceManager.Load<string>("assets/save.txt");
            Log.Info(save1);
            Log.Info(save2);

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
            throw new Exception(ex.Message + "\n Check 'rasdaq.log' for more details.");
        }
    }
}
