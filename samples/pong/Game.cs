using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Resources;

namespace pong;

public class Game : Application
{
    public Texture andrei;
    public Texture evilDiv;

    public override void Init()
    {
        base.Init();
        andrei = ResourceManager.Load<Texture>("assets/andrei.png");
        evilDiv = ResourceManager.Load<Texture>("assets/evil_enemy.jpg");
    }

    public override void Start()
    {
        base.Start();

        World world = new();
        Entity player = new(new Vector3(400, 0, 1));
        Entity enemy = new();

        Sprite spr = new(456, 456, andrei);
        player.AddComponent(spr);

        Sprite enemySprite = new(456, 456, evilDiv);
        enemy.AddComponent(enemySprite);

        Soldier sold = new();
        player.AddComponent(sold);

        world.AddEntity(player);
        world.AddEntity(enemy);
    }
}
