using OpenTK.Audio.OpenAL;

using rasdaq.Core.ECS;
using rasdaq.Logging;

namespace rasdaq.Audio;

public class AudioSource : Component, IDisposable
{
    internal int? Handle = null;

    internal override void Init()
    {
        Handle = AL.GenSource();
        // Check for errors
        ALError error = AL.GetError();
        if (error != ALError.NoError)
        {
            string err = $"OpenAL error while trying to create audio source: {error}";

            Log.Error(err);
            throw new Exception("OpenAL error while trying to create audio source: " + error);
        }
    }

    public void Play()
    {
        if (!Handle.HasValue)
        {
            Log.Error("Audio source handle is null. Cannot play audio.");
            return;
        }
        AL.SourcePlay(Handle.Value);
        AudioManager.CheckALError();
    }

    public void Stop()
    {
        if (!Handle.HasValue)
        {
            Log.Error("Audio source handle is null. Cannot play audio.");
            return;
        }
        AL.SourceStop(Handle.Value);
        AudioManager.CheckALError();
    }

    public void Dispose()
    {
        if (Handle.HasValue)
        {
            AL.DeleteSource(Handle.Value);
        }
    }
}
