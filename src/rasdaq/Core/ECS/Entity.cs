using rasdaq.Interfaces;

namespace rasdaq.Core.ECS;

/// <summary>
/// Base rasdaq game object class.
/// </summary>
public class Entity
{
    /// <summary>
    /// Return list of components attached to the entity.
    /// </summary>
    public List<Component> Components => _components.Objects;

    private readonly string _id;
    private FlushEnumerable<Component> _components = new();

    /// <summary>
    /// Determines if entity has performed its start method.
    /// </summary>
    internal bool Started { get; set; }

    /// <summary>
    /// Unique ID.
    /// </summary>
    public string ID => _id;

    public Entity(World? world = null)
    {
        _id = Guid.NewGuid().ToString("N");

        if (world != null)
        {
            world.AddEntity(this);
        }
    }

    /// <summary>
    /// Add a component to the entity.
    /// </summary>
    /// <param name="c"></param>
    public void AddComponent(Component c)
    {
        c.Entity = this;
        _components.Add(c);
    }

    /// <summary>
    /// Remove a component from the entity. Will call the <c>Destroy</c> function on the component before removing.
    /// </summary>
    /// <param name="c"></param>
    public void RemoveComponent(Component c)
    {
        c.Destroy();
        _components.Remove(c);
    }

    /// <summary>
    /// Get the first component with the given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>The first component of type T on the entity</returns>
    public T? GetComponent<T>() where T : Component
    {
        return _components.Objects.OfType<T>().FirstOrDefault();
    }

    internal void Start()
    {
        _components.FlushPending();

        for (int i = 0; i < _components.Objects.Count; i++)
        {
            Component c = _components.Objects[i];
            c.Start();
            c.Started = true;
        }
    }

    internal void Update(double dt)
    {
        _components.FlushPending();

        for (int i = 0; i < _components.Objects.Count; i++)
        {
            Component c = _components.Objects[i];
            Utils.EnsureStartableStart(c);
            c.Update(dt);
        }
    }

    internal void FrameUpdate(double dt)
    {
        _components.FlushPending();

        for (int i = 0; i < _components.Objects.Count; i++)
        {
            Component c = _components.Objects[i];
            Utils.EnsureStartableStart(c);
            c.FrameUpdate(dt);
        }
    }

    internal void LateUpdate(double dt)
    {
        _components.FlushPending();

        for (int i = 0; i < _components.Objects.Count; i++)
        {
            Component c = _components.Objects[i];
            Utils.EnsureStartableStart(c);
            c.LateUpdate(dt);
        }
    }
}
