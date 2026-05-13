using OpenTK.Audio.OpenAL;

using rasdaq.Logging;

namespace rasdaq.Audio;

public class AudioManager
{
    private static void CheckALCError(ALDevice device)
    {
        AlcError error = ALC.GetError(device);
        if (error != AlcError.NoError)
        {
            string err = $"OpenAL error: {error}";
            Log.Error(err)
            throw new Exception(err);
        }
    }

    private static void CheckALError()
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
    public AudioManager()
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

    /// <summary>
    /// Load a WAV file that contains PCM data only.
    /// </summary>
    /// <param name="path">Path to WAV file</param>
    /// <returns>A integer handle referring to the loaded audio</returns>
    /// <exception cref="Exception"></exception>
    public Audio LoadAudio(string path)
    {
        // Create an OpenAL buffer for the data to go in.
        Audio audio = new Audio();

        // Load the WAV file
        WAVLoader.WAVData wavData = WAVLoader.LoadWav(path);

        // Discern the format of the audio data based on number of channels and bits per sample.
        ALFormat format;
        if (wavData.Format.NumChannels == 1)
        {
            if (wavData.Format.BitsPerSample == 8)
            {
                format = ALFormat.Mono8;
            }
            else if (wavData.Format.BitsPerSample == 16)
            {
                format = ALFormat.Mono16;
            }
            else
            {
                string err = $"Unsupported bits per sample: {wavData.Format.BitsPerSample}";
                Log.Error(err);
                throw new Exception(err);
            }
        }
        else if (wavData.Format.NumChannels == 2)
        {
            if (wavData.Format.BitsPerSample == 8)
            {
                format = ALFormat.Stereo8;
            }
            else if (wavData.Format.BitsPerSample == 16)
            {
                format = ALFormat.Stereo16;
            }
            else
            {
                string err = $"Unsupported bits per sample: {wavData.Format.BitsPerSample}";
                Log.Error(err);
                throw new Exception(err);
            }
        }
        else
        {
            string err = $"Unsupported number of channels: {wavData.Format.NumChannels}";
            Log.Error(err);
            throw new Exception(err);
        }

        // Load the audio data into the OpenAL buffer.
        AL.BufferData(audio.Handle, format, wavData.Data.Data, (int)wavData.Format.SampleRate);

        // Check if we have any errors from trying to load the audio data.
        CheckALError();

        return audio;
    }

    /// <summary>
    /// Add a loaded audio file to an audio source, so that it can be played.
    /// </summary>
    /// <param name="audio">Audio file</param>
    /// <param name="audioSource">Audio source</param>
    public void AttachAudioToSource(Audio audio, AudioSource audioSource)
    {
        AL.Source(audioSource.Handle, ALSourcei.Buffer, audio.Handle);
        CheckALError();
    }

    /// <summary>
    /// Play back an audio source that has audio attached to it.
    /// </summary>
    /// <param name="source"></param>
    public void PlaySource(AudioSource audioSource)
    {
        AL.SourcePlay(audioSource.Handle);
        CheckALError();
    }

    /// <summary>
    /// Stop playback of an audio source.
    /// </summary>
    /// <param name="source"></param>
    public void StopSource(AudioSource audioSource)
    {
        AL.SourceStop(audioSource.Handle);
        CheckALError();
    }
}