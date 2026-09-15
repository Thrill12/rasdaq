using OpenTK.Audio.OpenAL;
using rasdaq.Logging;
using rasdaq.Resources;

namespace rasdaq.Audio;

public class AudioLoader : IResourceLoader
{
    public object Load(string path)
    {
        return AudioManager.Instance.LoadAudio(path);
    }
}
