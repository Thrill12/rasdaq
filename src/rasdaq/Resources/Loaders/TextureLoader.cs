using rasdaq.Graphics;

namespace rasdaq.Resources.Loaders;

internal class TextureLoader : IResourceLoader
{
    public object Load(string path)
    {
        return new Texture(path);
    }
}
