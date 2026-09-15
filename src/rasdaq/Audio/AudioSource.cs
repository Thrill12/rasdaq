using OpenTK.Audio.OpenAL;
using rasdaq.Core.ECS;
using rasdaq.Logging;

namespace rasdaq.Audio;

public class AudioSource : Component, IDisposable
{
    internal int Handle;
    public Audio AudioClip;

    public AudioSource(Audio? audioClip = null)
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

        if (audioClip != null)
        {
            SetAudio(audioClip);
        }
    }

    public void Dispose()
    {
        AL.DeleteSource(Handle);
    }

    /// <summary>
    /// Plays the loaded audio clip.
    /// </summary>
    public void Play()
    {
        AudioManager.Instance.PlaySource(this);
    }

    /// <summary>
    /// Stops playing the audio source.
    /// </summary>
    public void Stop()
    {
        AudioManager.Instance.StopSource(this);
    }

    /// <summary>
    /// Sets the audio clip of the audio source.
    /// </summary>
    /// <param name="audioClip"></param>
    public void SetAudio(Audio audioClip)
    {
        AudioManager.Instance.AttachAudioToSource(audioClip, this);
        this.AudioClip = audioClip;
    }
}
