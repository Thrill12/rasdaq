using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Resources;
using rasdaq.Transformations;
using System.Drawing;

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
        Renderer.Instance.Camera.SetPosition(0, 0);
        Entity player = new(new Vector3(500, 500, 1));
        Entity enemy = new(new Vector3(300, 300, 1));

        world.AddEntity(player);
        world.AddEntity(enemy);

        Sprite spr = new(25, 25, Color.WhiteSmoke);
        player.AddComponent(spr);
        PhysicsBody body = new() { ApplyGravity = false };
        player.AddComponent(body);
        BoxCollider collider = new(25, 25);
        player.AddComponent(collider);
        Soldier soldier = new();
        player.AddComponent(soldier);
        soldier.moveSpeed = 5;

        Sprite enemySprite = new(25, 25, Color.White);
        enemy.AddComponent(enemySprite);
        PhysicsBody enemyBody = new() { ApplyGravity = false };
        enemy.AddComponent(enemyBody);
        BoxCollider enemyCollider = new(25, 25);
        enemy.AddComponent(enemyCollider);
    }
}
