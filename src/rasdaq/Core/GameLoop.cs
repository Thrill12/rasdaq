using rasdaq.Core.ECS;

namespace rasdaq.Core;

internal class GameLoop(World world)
{
    private double _msPerUpdate = 0.01f;
    private double _lag = 0.0f;

    private World _world = world;

    /// <summary>
    /// Ticks the game loop by a certain amount of time.
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Tick(double deltaTime)
    {
        _lag += deltaTime;

        // PROCESS INPUT HERE

        double maxUpdates = 10;

        while (_lag >= _msPerUpdate && maxUpdates-- > 0)
        {
            _world.Update(_msPerUpdate);

            _lag -= _msPerUpdate;
        }

        _world.FrameUpdate(deltaTime);

        _world.LateUpdate(deltaTime);
    }
}
