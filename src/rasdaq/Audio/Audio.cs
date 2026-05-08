using OpenTK.Audio.OpenAL;

namespace rasdaq.Audio;

public class Audio : IDisposable
{
    public int Handle;
    public Audio()
    {
        Handle = AL.GenBuffer();
        // Check for errors
        ALError error = AL.GetError();
        if (error != ALError.NoError)
        {
            throw new Exception("OpenAL error while trying to create audio buffer: " + error);
        }
    }

    public void Dispose()
    {
        AL.DeleteBuffer(Handle);
    }
}
