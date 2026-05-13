namespace rasdaq.Resources.Loaders;

internal class TextLoader : IResourceLoader
{
    public object Load(string path)
    {
        return File.ReadAllText(path);
    }
}