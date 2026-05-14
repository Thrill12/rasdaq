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

    /// <summary>
    /// Creates an <c>Entity</c> instance. An <c>ID</c> is generated automatically once the <c>Entity</c> is created.
    /// </summary>
    public Entity()
    {
        _id = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Adds a <c>Component</c> to the entity
    /// </summary>
    /// <param name="comp"></param>
    public void AddComponent(Component comp)
    {
        comp.Entity = this;
        _components.Add(comp);
    }

    /// <summary>
    /// Get the first <c>Component</c> with the given type attached to this entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>The first component of type T on the entity</returns>
    public T? GetComponent<T>() where T : Component
    {
        return _components.OfType<T>().FirstOrDefault();
    }

    /// <summary>
    /// Get all <c>Components</c> of a certain type attached to this entity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public IEnumerable<T>? GetComponents<T>() where T : Component
    {
        return _components.OfType<T>();
    }

    internal void Start()
    {
        _components.ForEach(c =>
        {
            c.Init();
            c.Start();
        });
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