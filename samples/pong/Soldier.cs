using rasdaq;
using rasdaq.Core.ECS;
using rasdaq.Graphics;
using rasdaq.Inputs;
using rasdaq.Logging;
using rasdaq.Resources;
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

        if (Input.IsKeyPressed(Keys.V))
        {
            Log.Info("V pressed");
        }
    }
}
