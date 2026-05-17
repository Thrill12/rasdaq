using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
using rasdaq.Transformations;

namespace pong;

internal class Soldier : Component
{
    public override void Start()
    {
        base.Start();

        // Input.OnKeyDownEvent.Add(Keys.W, () =>
        // {
        //     Log.Info("User pressed W based on an event");
        // });

        // Input.OnKeyUpEvent.Add(Keys.B, () =>
        // {
        //     Log.Info("Hello");
        // });
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
    }

    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);

        Entity?.Transform.rotatedDegrees = 25f;

        // Console.SetCursorPosition(0, 0);
        // Log.Info("Soldier FPS " + Math.Round(1f / deltaTime));

        if (Input.IsKeyPressed(Keys.V))
        {
            Log.Info("V pressed");
        }
        if (Input.IsKeyDown(Keys.W))
        {
            Entity?.GetComponent<PhysicsBody>()?.MoveOnce(new Vector2(0, 100));
        }

        if (Input.IsKeyDown(Keys.S))
        {
            Entity?.GetComponent<PhysicsBody>()?.MoveOnce(new Vector2(0, -100));
        }
        if (Input.IsKeyDown(Keys.A))
        {
            Entity?.GetComponent<PhysicsBody>()?.MoveOnce(new Vector2(-100, 0));
        }
        if (Input.IsKeyDown(Keys.D))
        {
            Entity?.GetComponent<PhysicsBody>()?.MoveOnce(new Vector2(100, 0));
        }
        if (Input.IsKeyDown(Keys.H))
        {
            // track camera to entity
            var x = Entity?.Transform.position.X - (Application.WindowSize?.X / 2) ?? 0;
            var y = Entity?.Transform.position.Y - (Application.WindowSize?.Y / 2) ?? 0;
            Renderer.Instance.Camera.SetPosition(x, y);
        }
    }
}

internal class Enemy : Component
{
    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);

        if (Input.IsKeyDown(Keys.I))
        {
            // track camera to entity
            var x = Entity?.Transform.position.X - (Application.WindowSize?.X / 2) ?? 0;
            var y = Entity?.Transform.position.Y - (Application.WindowSize?.Y / 2) ?? 0;
            Renderer.Instance.Camera.SetPosition(x, y);
        }

    }
}
