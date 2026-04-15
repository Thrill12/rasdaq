using OpenTK.Windowing.GraphicsLibraryFramework;
using pong;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using Application = rasdaq.Application;

namespace Pong;

internal class Program
{
    static bool isMouseLocked = false;
    static bool logMouseDelta = false;

    static void TestKey(Application application)
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

        Console.WriteLine("testing keys: B");
    }

    static void TestMButton1Down()
    {
        Console.WriteLine("MB1 clicked!");

        logMouseDelta = true;
    }

    static void TestMButton1Up()
    {
        Console.WriteLine("MB1 released!");
        logMouseDelta = false;
    }

    static void PrintMouseDelta(float deltaX, float deltaY)
    {
        Console.WriteLine("Delta X: " + deltaX);
        Console.WriteLine("Delta Y: " + deltaY);
    }

    static void Main()
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
            // app.InputManager.KeyDownCallbacks.Add(Keys.A, () =>
            //     {
            //         Console.WriteLine("X: " + app.InputManager.GetMousePosition().X);
            //         Console.WriteLine("Y: " + app.InputManager.GetMousePosition().Y);
            //     });
            app.InputManager.KeyDownCallbacks.Add(Keys.W, () =>
            {
                soldier.Transform.Move(0, 0.1f);
                System.Console.WriteLine("yo just got");
            });
            app.InputManager.KeyDownCallbacks.Add(Keys.S, () =>
            {
                soldier.Transform.Move(0, -0.1f);
            });
            app.InputManager.KeyDownCallbacks.Add(Keys.A, () =>
            {
                soldier.Transform.Move(-0.1f, 0);
            });
            app.InputManager.KeyDownCallbacks.Add(Keys.D, () =>
            {
                soldier.Transform.Move(0.1f, 0);
            });

            app.InputManager.mouseMoveAction = (e) =>
            {
                if (logMouseDelta)
                {
                    PrintMouseDelta(e.DeltaX, e.DeltaY);
                }
            };

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