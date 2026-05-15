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
            Log.Info(randomSize.ToString());
            Sprite spr = new(randomSize, randomSize, tex);
            Log.Info("Added sprite");
            sprites.Add(spr);
            Entity.AddComponent(spr);

            Log.Info(sprites.Count.ToString());
        }

        if (Input.IsKeyDown(Keys.Backspace))
        {
            if (sprites.Count > 0)
            {
                Sprite sprite = sprites[sprites.Count - 1];

                Entity.RemoveComponent(sprite);
                sprites.RemoveAt(sprites.Count - 1);

                Log.Info(sprites.Count.ToString());
            }
        }
    }
}
