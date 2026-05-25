using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
using rasdaq.Resources;
using rasdaq.Transformations;
using System.Drawing;

namespace pong;

internal class Soldier : Component
{
    public BoxCollider? collider;
    public Sprite? sprite;

    public override void Start()
    {
        base.Start();
        sprite = Entity?.GetComponent<Sprite>();
    }

    public override void Update(double deltaTime)
    {
        base.Update(deltaTime);

        if (collider == null)
        {
            collider = Entity?.GetComponent<BoxCollider>();
        }

        if (collider != null)
        {
            sprite?.Color = collider.Collisions.Count > 0 ? Color.Red : Color.WhiteSmoke;
        }
    }

    public float moveSpeed = 100;

    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);

        PhysicsBody? body = Entity?.GetComponent<PhysicsBody>();

        if (Input.IsKeyDown(Keys.W))
        {
            body?.MoveOnce(new Vector2(0, moveSpeed));
        }
        if (Input.IsKeyDown(Keys.S))
        {
            body?.MoveOnce(new Vector2(0, -moveSpeed));
        }
        if (Input.IsKeyDown(Keys.A))
        {
            body?.MoveOnce(new Vector2(-moveSpeed, 0));
        }
        if (Input.IsKeyDown(Keys.D))
        {
            body?.MoveOnce(new Vector2(moveSpeed, 0));
        }
    }
}
