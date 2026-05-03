using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using pong;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Logging;
using rasdaq.Transformations;
using Application = rasdaq.Application;

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
            Entity soldier = new();
            Entity enemy = new();
            // TODO remove soldier from constructor, this was for testing purposes only
            using Application app = new(800, 600, "rasdaq", soldier);

            Texture tex = new("assets/andrei.png");
            Texture evilTex = new("assets/evil_enemy.jpg");

            World world = new();

            soldier.AddComponent(new Soldier());
            enemy.AddComponent(new Soldier());

            Sprite spr = new(456, 456, tex, 400, 300);
            Sprite evilSpr = new(304, 230, evilTex, 800, 600);

            soldier.AddComponent(spr);
            enemy.AddComponent(evilSpr);

            world.AddEntity(soldier);
            world.AddEntity(enemy);

            app.InputManager.KeyDownCallbacks.Add(Keys.B, () => TestKey(app));
            app.InputManager.KeyDownCallbacks.Add(Keys.A, () =>
                {
                    Log.Info("X: " + app.InputManager.GetMousePosition().X);
                    Log.Info("Y: " + app.InputManager.GetMousePosition().Y);
                });

            soldier.Transform.MoveVector(new Vector2(90, 0), 100);
            app.InputManager.mouseMoveAction = (e) =>
            {
                if (logMouseDelta)
                {
                    PrintMouseDelta(e.DeltaX, e.DeltaY);
                }
            };

            // set camera follow on spr sprite
            app.InputManager.KeyDownCallbacks.Add(Keys.C, () =>
            { Camera.SpriteToFollow = spr; });
            app.InputManager.KeyUpCallbacks.Add(Keys.C, () =>
            { Camera.SpriteToFollow = null; });

            app.InputManager.MouseButtonDownCallbacks.Add(MouseButton.Button1, TestMButton1Down);
            app.InputManager.MouseButtonUpCallbacks.Add(MouseButton.Button1, TestMButton1Up);

            app.Run();
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
        }
    }
}