using OpenTK.Audio.OpenAL;

using rasdaq.Logging;

namespace rasdaq.Audio;

public class AudioManager
{
    internal static void CheckALCError(ALDevice device)
    {
        AlcError error = ALC.GetError(device);
        if (error != AlcError.NoError)
        {
            string err = $"OpenAL error: {error}";
            Log.Error(err);
            throw new Exception(err);
        }
    }

    internal static void CheckALError()
    {
        ALError error = AL.GetError();
        if (error != ALError.NoError)
        {
            string err = $"OpenAL error: {error}";
            Log.Error(err);
            throw new Exception(err);
        }
    }

    /// <summary>
    /// Initialize Audio for the Application, using the default audio device.
    /// </summary>
    /// <exception cref="Exception"></exception>
    internal static void Initialize()
    {
        // Open the default audio device.
        ALDevice device = ALC.OpenDevice(null);
        if (device == ALDevice.Null)
        {
            string err = "Failed to open the default audio device.";
            Log.Error(err);
            throw new Exception(err);
        }

        // Create a context for the device.
        ALContext context = ALC.CreateContext(device, new ALContextAttributes(null, null, null, null, null));
        if (context == ALContext.Null)
        {
            ALC.CloseDevice(device);
            string err = "Failed to create an OpenAL context.";
            Log.Error(err);
            throw new Exception(err);
        }

        // Check if we have any errors.
        AlcError error = ALC.GetError(device);
        CheckALCError(device);
        // If no errors, make the context current.
        ALC.MakeContextCurrent(context);
    }
}
