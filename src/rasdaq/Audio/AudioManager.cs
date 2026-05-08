using OpenTK.Audio.OpenAL;

namespace rasdaq.Audio;

public class AudioManager
{
    private static void CheckALCError(ALDevice device)
    {
        AlcError error = ALC.GetError(device);
        if (error != AlcError.NoError)
        {
            throw new Exception("OpenAL error: " + error);
        }
    }

    private static void CheckALError()
    {
        ALError error = AL.GetError();
        if (error != ALError.NoError)
        {
            throw new Exception("OpenAL error: " + error);
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
            throw new Exception("Failed to open the default audio device.");
        }

        // Create a context for the device.
        ALContext context = ALC.CreateContext(device, new ALContextAttributes(null, null, null, null, null));
        if (context == ALContext.Null)
        {
            ALC.CloseDevice(device);
            throw new Exception("Failed to create an OpenAL context.");
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
                throw new Exception("Unsupported bits per sample: " + wavData.Format.BitsPerSample);
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
                throw new Exception("Unsupported bits per sample: " + wavData.Format.BitsPerSample);
            }
        }
        else
        {
            throw new Exception("Unsupported number of channels: " + wavData.Format.NumChannels);
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
    /// <param name="audioHandle">Integer handle of loaded audio file</param>
    /// <param name="sourceHandle">Integer handle of audio source</param>
    public void AttachAudioToSource(Audio audio, AudioSource audioSource)
    {
        AL.Source(audio.Handle, ALSourcei.Buffer, audioSource.Handle);
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