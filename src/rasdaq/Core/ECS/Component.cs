namespace rasdaq.Core.ECS;

/// <summary>
/// Component that can be added to entities. 
/// You may create subclasses in order to attach to entities.
/// </summary>
public abstract class Component
{
    /// <summary>
    /// Associated entity.
    /// </summary>
    public Entity? Entity { get; internal set; }
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
}