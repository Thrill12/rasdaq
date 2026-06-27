using rasdaq.Core.ECS;

namespace rasdaq.Core;

/// <summary>
/// Handles the engine game loop, which is responsible for updating the world and processing input.
/// </summary>
/// <param name="world"></param>
internal class GameLoop(World world)
{
    private double _lag = 0.0f;
    private double _maxUpdates = 10;
    private World _world = world;

    /// <summary>
    /// Maximum amount of updates before the game loop gives up and moves on to the next frame.
    /// This is a safety measure to prevent the game from freezing if the update logic takes too long.
    /// </summary>
    internal double MaxUpdates => _maxUpdates;

    /// <summary>
    /// Ticks the game loop by a certain amount of time.
    /// </summary>
    /// <param name="deltaTime"></param>
    public void Tick(double deltaTime)
    {
        _lag += deltaTime;

        // PROCESS INPUT HERE

        _maxUpdates = 10;

        while (_lag >= _world.Physics.FixedUpdateTime && _maxUpdates-- > 0)
        {
            _world.Update(_world.Physics.FixedUpdateTime);

            _lag -= _world.Physics.FixedUpdateTime;
        }

        _world.FrameUpdate(deltaTime);

        _world.LateUpdate(deltaTime);
    }
}
