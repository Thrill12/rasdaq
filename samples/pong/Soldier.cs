using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
using rasdaq.Transformations;

using rasdaq.Resources;
namespace pong;

internal class Soldier : Component
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);
    }

    private List<Sprite> sprites = new();
    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);

        if (Input.IsKeyDown(Keys.V))
        {
            Texture tex = ResourceManager.Load<Texture>("assets/andrei.png");
            Random rand = new();
            float randomSize = (float)(rand.Next(1, 101) / 100f);
            Sprite spr = new(randomSize, randomSize, tex);
            sprites.Add(spr);

            Entity newEnt = new();
            Entity.AddComponent(spr);
            World.AddEntity(Entity);
        }

        if (Input.IsKeyDown(Keys.Backspace))
        {
            if (sprites.Count > 0)
            {
                Sprite sprite = sprites[sprites.Count - 1];

                Entity.RemoveComponent(sprite);
                sprites.RemoveAt(sprites.Count - 1);

                Log.Info("Worlds: " + Application.Instance.Worlds.Count.ToString());
                Log.Info("Components: " + Entity.GetComponents().Count().ToString());
                Log.Info("Sprites: " + Entity.GetComponents<Sprite>().Count().ToString());
            }
        }

        PhysicsBody? body = Entity?.GetComponent<PhysicsBody>();

        if (Input.IsKeyDown(Keys.W))
        {
            body?.MoveOnce(new Vector2(0, 100));
        }

        if (Input.IsKeyDown(Keys.S))
        {
            body?.MoveOnce(new Vector2(0, -100));
        }
        if (Input.IsKeyDown(Keys.A))
        {
            body?.MoveOnce(new Vector2(-100, 0));
        }
        if (Input.IsKeyDown(Keys.D))
        {
            body?.MoveOnce(new Vector2(100, 0));
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
