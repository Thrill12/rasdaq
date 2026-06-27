using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Resources;
using rasdaq.Transformations;
using System.Drawing;

namespace pong;

public class Game : Application
{
    public Texture carTexture;
    public Texture background;

    public override void Init()
    {
        carTexture = ResourceManager.Load<Texture>("assets/sport_red.png");
        background = ResourceManager.Load<Texture>("assets/rug.png");
    }

    public override void Start()
    {
        World world = new();
        Entity player = new(new Vector3(500, 500, 1));
        Entity enemy = new(new Vector3(300, 300, 1));

        world.AddEntity(player);
        world.AddEntity(enemy);

        Entity background = new(new Vector3(500, 200, 0));
        world.AddEntity(background);
        Sprite bgSprite = new(8000, 4000, this.background);
        background.AddComponent(bgSprite);

        Sprite spr = new(50, 25, carTexture);
        player.AddComponent(spr);
        PhysicsBody body = new() { ApplyGravity = false };
        player.AddComponent(body);
        Car car = new();
        player.AddComponent(car);
        car.speed = 1000;
        car.rotationSpeed = 250;
        car.maxSpeed = 5000;
    }
}
