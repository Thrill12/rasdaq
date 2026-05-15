namespace rasdaq.Core.ECS;

/// <summary>
/// Base rasdaq game object class
/// </summary>
public class Entity
{
    private readonly string _id;
    private List<Component> _components = new();

    /// <summary>
    /// Unique ID.
    /// </summary>
    public string ID => _id;

    public Entity()
    {
        _id = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Adds a component to the entity
    /// </summary>
    /// <param name="c"></param>
    public void AddComponent(Component c)
    {
        c.Entity = this;
        _components.Add(c);
    }

    /// <summary>
    /// Removes a component from the entity. Will call the <c>Destroy</c> function before removing the component.
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
        return _components.OfType<T>().FirstOrDefault();
    }

    internal void Start()
    {
        for (int i = 0; i < _components.Count; i++)
        {
            Component c = _components[i];
            c.Init();
            c.Start();
        }
    }

    internal void Update(double dt)
    {
        _components.ForEach(c => c.Update(dt));
    }

    internal void FrameUpdate(double dt)
    {
        _components.ForEach(c => c.FrameUpdate(dt));
    }

    internal void LateUpdate(double dt)
    {
        _components.ForEach(c => c.LateUpdate(dt));
    }
}
