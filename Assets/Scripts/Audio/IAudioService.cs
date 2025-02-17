using UnityEngine;

public interface IAudioService
{
    public void PlaySoundOnce(AudioClip audioClip, float volume);
    public void PlaySoundOnceDelayed(AudioClip audioClip, float volume, float delay);
    public void FadeIn(int channel, AudioClip music, bool restartMusic = false, float time = -1);
    public void FadeOut(int channel, float time = -1);
}
