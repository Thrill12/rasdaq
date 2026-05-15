namespace rasdaq.Core.ECS;

/// <summary>
/// Base rasdaq game object class.
/// </summary>
public class Entity
{
    private readonly string _id;
    private List<Component> _components = new();

    private List<Component> _pendingAdd = new();
    private List<Component> _pendingRemove = new();

    /// <summary>
    /// Determines if component has performed its start method.
    /// </summary>
    internal bool Started { get; set; }

    /// <summary>
    /// Unique ID.
    /// </summary>
    public string ID => _id;

    public Entity()
    {
        _id = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Add a component to the entity.
    /// </summary>
    /// <param name="c"></param>
    public void AddComponent(Component c)
    {
        c.Entity = this;
        _pendingAdd.Add(c);
    }

    /// <summary>
    /// Remove a component from the entity. Will call the <c>Destroy</c> function before removing the component.
    /// </summary>
    /// <param name="c"></param>
    public void RemoveComponent(Component c)
    {
        _pendingRemove.Add(c);
    }

    /// <summary>
    /// Processes components queued for addition/removal. Used
    /// to prevent adding or removing components at runtime
    /// causing components to be skipped, or updated twice.
    /// </summary>
    private void FlushPendingComponents()
    {
        for (int i = 0; i < _pendingAdd.Count; i++)
        {
            Component c = _pendingAdd[i];
            _components.Add(c);
        }
        _pendingAdd.Clear();

        for (int i = 0; i < _pendingRemove.Count; i++)
        {
            Component c = _pendingRemove[i];
            c.Destroy();
            _components.Remove(c);
        }
        _pendingRemove.Clear();
    }

    /// <summary>
    /// Get the first component with the given type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>The first component of type T on the entity</returns>
    public T? GetComponent<T>() where T : Component
    {
        return _components.OfType<T>().FirstOrDefault();
    }

    internal void Start()
    {
        FlushPendingComponents();

        for (int i = 0; i < _components.Count; i++)
        {
            Component c = _components[i];
            c.Start();
            c.Started = true;
        }
    }

    internal void Update(double dt)
    {
        FlushPendingComponents();

        for (int i = 0; i < _components.Count; i++)
        {
            Component c = _components[i];
            c.Update(dt);
            EnsureComponentStart(c);
        }
    }

    internal void FrameUpdate(double dt)
    {
        FlushPendingComponents();

        for (int i = 0; i < _components.Count; i++)
        {
            Component c = _components[i];
            c.FrameUpdate(dt);
            EnsureComponentStart(c);
        }
    }

    internal void LateUpdate(double dt)
    {
        FlushPendingComponents();

        for (int i = 0; i < _components.Count; i++)
        {
            Component c = _components[i];
            c.LateUpdate(dt);
            EnsureComponentStart(c);
        }
    }

    private static void EnsureComponentStart(Component c)
    {
        if (!c.Started)
        {
            c.Start();
            c.Started = true;
        }
    }
}
