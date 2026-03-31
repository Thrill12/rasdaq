using rasdaq.Core.ECS;

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

    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("Soldier FPS " + Math.Round(1f / deltaTime));
    }
}
