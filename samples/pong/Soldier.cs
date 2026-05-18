using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
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
    }
}
