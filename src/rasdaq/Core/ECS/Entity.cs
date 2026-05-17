using rasdaq.Transformations;

namespace rasdaq.Core.ECS;

/// <summary>
/// Base rasdaq game object class
/// </summary>
/// <param name="spawnPosition">
/// The position where the object will be spawned. The z position determines how the sprites are layered on the screen.
/// A higher z value means the object will be rendered on top of objects with lower z values
/// The z value can be anywhere between 0 and 1000.
///
/// <para>The x and y coordinates is the position of the centre of the Entity</para>
/// </param>
public class Entity(Vector3 spawnPosition)
{
    private readonly string _id = Guid.NewGuid().ToString("N");
    private List<Component> _components = [];
    private Transform _transform = new(spawnPosition);

    /// <summary>
    /// Unique ID.
    /// </summary>
    public string ID => _id;
    public Transform Transform => _transform;

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

/// <summary>
/// Component that can be added to entities.
/// </summary>
public abstract class Component
{
    /// <summary>
    /// Associated entity.
    /// </summary>
    public Entity? Entity { get; internal set; }
    internal virtual void Init() { }
    public virtual void Start() { }
    public virtual void Update(double deltaTime) { }
    public virtual void FrameUpdate(double deltaTime) { }
    public virtual void LateUpdate(double deltaTime) { }
}
