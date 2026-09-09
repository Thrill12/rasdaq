using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Resources;
using rasdaq.Transformations;

namespace pong;

public class Game : Application
{
    public Texture andrei;
    public Texture evilDiv;

    public override void Init()
    {
        andrei = ResourceManager.Load<Texture>("assets/andrei.png");
        evilDiv = ResourceManager.Load<Texture>("assets/evil_enemy.jpg");
    }

    public override void Start()
    {
        World world = new();
        Entity player = new(new Vector3(400, 200, 1));
        Entity enemy = new();

        Sprite spr = new(456, 456, andrei);
        player.AddComponent(spr);
        player.AddComponent(new PhysicsBody());

        Sprite enemySprite = new(456, 456, evilDiv);
        enemy.AddComponent(enemySprite);

        Soldier sold = new();
        player.AddComponent(sold);

        Enemy enem = new();
        enemy.AddComponent(enem);

        world.AddEntity(player);
        world.AddEntity(enemy);
    }
}
