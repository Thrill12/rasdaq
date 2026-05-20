using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Logging;
using rasdaq.Resources;

namespace pong;

public class Game : Application
{
    public Texture andrei;

    public override void Init()
    {
        base.Init();
        andrei = ResourceManager.Load<Texture>("assets/andrei.png");
    }

    public override void Start()
    {
        base.Start();

        World world = new();
        Entity player = new();

        Sprite spr = new(1, 1, andrei);
        player.AddComponent(spr);

        Soldier sold = new();
        player.AddComponent(sold);

        world.AddEntity(player);
    }
}
