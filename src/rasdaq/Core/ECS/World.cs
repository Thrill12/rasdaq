namespace rasdaq.Core.ECS;

/// <summary>
/// A world is a container for entities and their components. 
/// It is responsible for the entity lifecycle and management.
/// </summary>
public class World
{
    private readonly string _id;
    private GameLoop _gameLoop;

    private List<Entity> _entities = new();
    private List<Entity> _pendingAdd = new();
    private List<Entity> _pendingRemove = new();

    internal GameLoop GameLoop => _gameLoop;

    /// <summary>
    /// Unique ID representing the world.
    /// </summary>
    public string ID => _id;
    /// <summary>
    /// Gets the collection of entities managed by this world.
    /// </summary>
    public List<Entity> Entities => _entities;

    /// <summary>
    /// Creates an instance of a <c>World</c>. An <c>ID</c> is generated automatically once the <c>World</c> is created.
    /// </summary>
    public World()
    {
        _id = Guid.NewGuid().ToString("N");
        _gameLoop = new(this);

        if (Application.Instance != null)
        {
            Application.Instance.RegisterWorld(this);
        }
    }

    /// <summary>
    /// Adds an <c>Entity</c> to the <c>World</c>.
    /// </summary>
    /// <param name="e"></param>
    public void AddEntity(Entity e)
    {
        _pendingAdd.Add(e);
    }

    /// <summary>
    /// Removes an <c>Entity</c> from the <c>World</c>.
    /// </summary>
    /// <param name="e"></param>
    public void RemoveEntity(Entity e)
    {
        _pendingRemove.Add(e);
    }

    /// <summary>
    /// Processes entities queued for addition/removal. Used
    /// to prevent adding or removing entities at runtime
    /// causing entities to be skipped, or updated twice.
    /// </summary>
    private void FlushPendingEntities()
    {
        foreach (var e in _pendingAdd)
        {
            _entities.Add(e);
        }

        _pendingAdd.Clear();
        foreach (var e in _pendingRemove)
        {
            _entities.Remove(e);
        }

        _pendingRemove.Clear();
    }

    internal void Start()
    {
        FlushPendingEntities();
        foreach (var e in _entities)
        {
            e.Start();
        }
    }

    internal void Update(double deltaTime)
    {
        FlushPendingEntities();
        foreach (var e in _entities)
        {
            e.Update(deltaTime);
        }
    }

    internal void FrameUpdate(double deltaTime)
    {
        FlushPendingEntities();
        foreach (var e in _entities)
        {
            e.FrameUpdate(deltaTime);
        }
    }

    internal void LateUpdate(double deltaTime)
    {
        FlushPendingEntities();
        foreach (var e in _entities)
        {
            e.LateUpdate(deltaTime);
        }
    }
}