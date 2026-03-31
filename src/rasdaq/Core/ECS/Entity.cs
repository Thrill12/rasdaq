namespace rasdaq.Core.ECS;

/// <summary>
/// Base rasdaq game object class
/// </summary>
public class Entity
{
    private readonly string _id;
    public string ID => _id;

    public Entity()
    {
        _id = Guid.NewGuid().ToString("N");
    }

    private List<Component> _components = new();

    public void AddComponent(Component c)
    {
        c.Entity = this;
        _components.Add(c);
    }

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

public abstract class Component
{
    public Entity? Entity { get; internal set; }
    internal virtual void Init() { }
    public virtual void Start() { }
    public virtual void Update(double deltaTime) { }
    public virtual void FrameUpdate(double deltaTime) { }
    public virtual void LateUpdate(double deltaTime) { }
}