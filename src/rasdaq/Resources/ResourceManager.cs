using rasdaq.Logging;
using rasdaq.Resources.Loaders;

namespace rasdaq.Resources;

public static class ResourceManager
{
    private static Dictionary<string, object> _cache = new();
    private static Dictionary<string, IResourceLoader> _loaders = new();

    static ResourceManager()
    {
        // Match file extensions with specific loaders here.
        _loaders["txt"] = new TextLoader();
        _loaders["png"] = new TextureLoader();
        _loaders["jpg"] = new TextureLoader();
    }

    /// <summary>
    /// Loads a file from a path and returns a type, if rasdaq supports it.
    /// If the same file has been loaded already, it returns the cached reference instead of
    /// loading the file again.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public static T Load<T>(string path)
    {
        if (_cache.ContainsKey(path))
        {
            Log.Trace($"Cache hit for {path}");
            return (T)_cache[path];
        }

        string ext = Path.GetExtension(path).TrimStart('.');

        if (!_loaders.ContainsKey(ext))
        {
            Log.Error("rasdaq does not currently support this file.");
            throw new Exception("rasdaq does not currently support this file.");
        }

        IResourceLoader loader = _loaders[ext];

        T value = (T)loader.Load(path);

        _cache[path] = value;

        return value;
    }
}
