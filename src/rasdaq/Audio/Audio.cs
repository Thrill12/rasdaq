using OpenTK.Audio.OpenAL;

using rasdaq.Logging;

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
            string err = $"OpenAL error while trying to create audio buffer: {error}";

            Log.Error(err);
            throw new Exception(err);
        }
    }

    public void Dispose()
    {
        AL.DeleteBuffer(Handle);
    }
}
