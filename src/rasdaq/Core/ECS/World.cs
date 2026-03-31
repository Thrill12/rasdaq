namespace rasdaq.Core.ECS;

/// <summary>
/// A world holds entities
/// </summary>
public class World
{
    private readonly string _id;
    public string ID => _id;

    private GameLoop _gameLoop;
    internal GameLoop GameLoop => _gameLoop;

    public World()
    {
        _id = Guid.NewGuid().ToString("N");
        _gameLoop = new(this);

        if (Application.Instance != null)
        {
            Application.Instance.worlds.Add(this);
        }
    }

    private List<Entity> _entities = new();
    public List<Entity> Entities => _entities;

    private List<Entity> _pendingAdd = new();
    private List<Entity> _pendingRemove = new();

    public void AddEntity(Entity e)
    {
        _pendingAdd.Add(e);
    }

    public void RemoveEntity(Entity e)
    {
        _pendingRemove.Add(e);
    }

    // This is used in case there is any change of entities at runtime
    // It prevents entities being skipped or updated twice
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
