using rasdaq.Core.ECS;
using rasdaq.Inputs;
using rasdaq.Logging;

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

        if (Input.IsKeyDown(Keys.V))
        {
            Log.Info("V pressed");
        }
    }

    public override void FrameUpdate(double deltaTime)
    {
        base.Update(deltaTime);
    }
}