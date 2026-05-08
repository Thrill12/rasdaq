using OpenTK.Audio.OpenAL;

namespace rasdaq.Audio;

public class AudioSource : IDisposable
{
    public int Handle;
    public AudioSource()
    {
        Handle = AL.GenSource();
        // Check for errors
        ALError error = AL.GetError();
        if (error != ALError.NoError)
        {
            throw new Exception("OpenAL error while trying to create audio source: " + error);
        }
    }

    public void Dispose()
    {
        AL.DeleteSource(Handle);
    }
}
