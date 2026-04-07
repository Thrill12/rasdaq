using rasdaq.Core.ECS;
using rasdaq.Graphics;

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

    int counter = 0;
    float move = -0.0001f;

    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);
        Sprite? spr = Entity?.GetComponent<Sprite>();
        if (counter % 2000 == 0)
        {
            move *= -1;
        }
        counter++;

        // spr?.Transform.Move(0, move);
        spr?.Transform.RotateY(0.05f);
        // Console.SetCursorPosition(0, 0);
        // Console.WriteLine("Soldier FPS " + Math.Round(1f / deltaTime));
    }
}