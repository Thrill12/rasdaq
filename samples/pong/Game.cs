using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Logging;
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

        world.AddEntity(player);

        Sprite spr = new(50, 25, carTexture);
        player.AddComponent(spr);
        PhysicsBody body = new() { ApplyGravity = false };
        player.AddComponent(body);
        BoxCollider carCollider = new(50, 25);
        player.AddComponent(carCollider);
        Car car = new();
        player.AddComponent(car);
        car.speed = 1000;
        car.rotationSpeed = 250;
        car.maxSpeed = 5000;

        Entity collisionBox = new(new Vector3(400, 400, 0));
        world.AddEntity(collisionBox);

        Sprite collisionBoxSprite = new(200, 200, background);
        collisionBox.AddComponent(collisionBoxSprite);

        PhysicsBody physicsBody = new() { ApplyGravity = false };
        collisionBox.AddComponent(physicsBody);

        BoxCollider boxCollider = new(200, 200);
        collisionBox.AddComponent(boxCollider);

        boxCollider.OnCollisionEnter += (other) =>
        {
            Log.Info($"Collision detected with {other.Entity}");
            collisionBoxSprite.Color = Color.Red;
        };

        boxCollider.OnCollisionExit += (other) =>
        {
            Log.Info($"Collision ended with {other.Entity}");
            collisionBoxSprite.Color = Color.White;
        };

        boxCollider.OnCollisionStay += (other) =>
        {
            Log.Info($"Collision ongoing with {other.Entity}");
        };
    }
}
