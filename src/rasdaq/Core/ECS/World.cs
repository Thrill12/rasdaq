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

    public World(Application app)
    {
        _id = Guid.NewGuid().ToString("N");
        _gameLoop = new(this);
        app.worlds.Add(this);
    }

    private HashSet<Entity> _entities = new();
    public HashSet<Entity> Entities => _entities;

    public void AddEntity(Entity entity)
    {
        _entities.Add(entity);
    }

    internal void Start()
    {
        for (int i = 0; i < _entities.Count; i++)
        {
            Entity e = _entities.ElementAt(i);
            e.Start();
        }
    }

    internal void Update(double deltaTime)
    {
        for (int i = 0; i < _entities.Count; i++)
        {
            Entity e = _entities.ElementAt(i);
            e.Update(deltaTime);
        }
    }

    internal void FrameUpdate(double deltaTime)
    {
        for (int i = 0; i < _entities.Count; i++)
        {
            Entity e = _entities.ElementAt(i);
            e.FrameUpdate(deltaTime);
        }
    }

    internal void LateUpdate(double deltaTime)
    {
        for (int i = 0; i < _entities.Count; i++)
        {
            Entity e = _entities.ElementAt(i);
            e.LateUpdate(deltaTime);
        }
    }
}
