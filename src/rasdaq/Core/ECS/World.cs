using rasdaq.Interfaces;
using rasdaq.Logging;

namespace rasdaq.Core.ECS;

/// <summary>
/// A world is a container for entities. 
/// It is responsible for their lifecycle and management.
/// </summary>
public class World
{
    /// <summary>
    /// Unique ID representing the world.
    /// </summary>
    public string ID => _id;

    private readonly string _id;
    private GameLoop _gameLoop;
    private FlushEnumerable<Entity> _entities = new();

    /// <summary>
    /// Determines if world has performed its start method.
    /// </summary>
    internal bool Started { get; set; }
    internal GameLoop GameLoop => _gameLoop;

    /// <summary>
    /// Get the collection of entities managed by this world.
    /// </summary>
    public List<Entity> Entities => _entities.Objects;

    /// <summary>
    /// Create a new world. A unique ID is generated automatically.
    /// </summary>
    public World()
    {
        _id = Guid.NewGuid().ToString("N");
        _gameLoop = new(this);

        try
        {
            Application.Instance.AddWorld(this);
        }
        catch (Exception e)
        {
            Log.Error("Application has not been initialized. Call Run() first.");
        }
    }

    /// <summary>
    /// Add the specified entity to the world entities.
    /// </summary>
    /// <param name="e"></param>
    public void AddEntity(Entity e)
    {
        _entities.Add(e);
    }

    /// <summary>
    /// Remove the specified entity from the world entities.
    /// </summary>
    /// <param name="e"></param>
    public void RemoveEntity(Entity e)
    {
        _entities.Remove(e);
    }

    internal void Start()
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entity e = Entities[i];
            e.Start();
            e.Started = true;
        }
    }

    internal void Update(double deltaTime)
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entity e = Entities[i];
            Utils.EnsureStartableStart(e);
            e.Update(deltaTime);
        }
    }

    internal void FrameUpdate(double deltaTime)
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entity e = Entities[i];
            Utils.EnsureStartableStart(e);
            e.FrameUpdate(deltaTime);
        }
    }

    internal void LateUpdate(double deltaTime)
    {
        for (int i = 0; i < Entities.Count; i++)
        {
            Entity e = Entities[i];
            Utils.EnsureStartableStart(e);
            e.LateUpdate(deltaTime);
        }
    }
}
