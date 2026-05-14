using rasdaq.Core.ECS;
using rasdaq.Inputs;
using rasdaq.Logging;

namespace pong;

internal class Soldier : Component
{
    public override void Start()
    {
        base.Start();

        Input.OnKeyDownEvent.Add(Keys.W, () =>
        {
            Log.Info("User pressed W based on an event");
        });

        Input.OnKeyUpEvent.Add(Keys.B, () =>
        {
            Log.Info("Hello");
        });
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
