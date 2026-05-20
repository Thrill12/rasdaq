namespace rasdaq.Interfaces;

internal class FlushEnumerable<T>
{
    private List<T> _objects = new();
    private List<T> _pendingAdd = new();
    private List<T> _pendingRemove = new();

    public List<T> Objects
    {
        get
        {
            // Not sure if this is a good idea to FlushPending but i can't think of any good reasons
            // we would want to manually flush in other places. Maybe in Update methods wherever these get used instead?
            FlushPending();
            return _objects;
        }
    }

    /// <summary>
    /// Add the specified entity to the world entities.
    /// </summary>
    /// <param name="e"></param>
    internal void Add(T obj)
    {
        _pendingAdd.Add(obj);
    }

    /// <summary>
    /// Remove the specified entity from the world entities.
    /// </summary>
    /// <param name="e"></param>
    internal void Remove(T obj)
    {
        _pendingRemove.Add(obj);
    }

    /// <summary>
    /// Processes entities queued for addition/removal. Used
    /// to prevent adding or removing entities at runtime
    /// causing entities to be skipped, or updated twice.
    /// </summary>
    internal void FlushPending()
    {
        for (int i = 0; i < _pendingAdd.Count; i++)
        {
            T obj = _pendingAdd[i];
            _objects.Add(obj);
        }
        _pendingAdd.Clear();

        for (int i = 0; i < _pendingRemove.Count; i++)
        {
            T obj = _pendingRemove[i];
            _objects.Remove(obj);
        }
        _pendingRemove.Clear();
    }
}
