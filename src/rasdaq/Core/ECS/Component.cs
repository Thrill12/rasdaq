namespace rasdaq.Core.ECS;

/// <summary>
/// Component that can be added to entities.
/// You may create subclasses in order to attach to entities.
/// </summary>
public abstract class Component
{
    /// <summary>
    /// Get the associated entity.
    /// </summary>
    public Entity Entity => _entity == null ? throw new InvalidOperationException($"{GetType().Name} is not attached to an entity.") : _entity;

    /// <summary>
    /// Get the world of the associated entity.
    /// </summary>
    public World World => _entity == null
                ? throw new InvalidOperationException($"{GetType().Name} is not attached to an entity.")
                : _entity.world == null
                ? throw new InvalidOperationException($"{GetType().Name} Entity is not attached to a a world.")
                : _entity.world;

    /// <summary>
    /// Determines if component has performed its start method.
    /// </summary>
    public bool Started { get; set; }

    private Entity? _entity;

    /// <summary>
    /// Create a new component.
    /// </summary>
    public Component()
    {
        Init();
    }

    internal void Attach(Entity entity)
    {
        _entity = entity;
    }

    internal void Detach()
    {
        _entity = null;
    }

    internal virtual void Init() { }

    /// <summary>
    /// Called at the start of a <c>Component</c>'s life.
    /// </summary>
    public virtual void Start() { }
    /// <summary>
    /// Called on every physics update.
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void Update(double deltaTime) { }
    /// <summary>
    /// Called on every frame.
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void FrameUpdate(double deltaTime) { }
    /// <summary>
    /// Called on every frame, but guaranteed to be after <c>FrameUpdate</c>.
    /// </summary>
    /// <param name="deltaTime"></param>
    public virtual void LateUpdate(double deltaTime) { }
    /// <summary>
    /// Called before the component is destroyed.
    /// </summary>
    public virtual void Destroy() { }
}
