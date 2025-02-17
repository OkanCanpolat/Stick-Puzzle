using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioService
{
    private class MusicChannel
    {
        public int[] MusicSourceIndices;

        public int ActiveMusicSourceIndex;
    }

    [SerializeField] private int musicChannelCount = 2;

    [SerializeField] private float transitionTime = 2f;

    private const int musicSourcePerChannel = 2;

    private AudioSource sfxSource;

    private AudioSource[] musicSources;

    private List<MusicChannel> musicChannels = new List<MusicChannel>();

    private Coroutine[] sourceFadeCoroutines;
    private int MusicSourceCount => musicSourcePerChannel * musicChannelCount;

    private void Start()
    {
        Init();
    }
    private void Init()
    {
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSources = new AudioSource[MusicSourceCount];
        sourceFadeCoroutines = new Coroutine[MusicSourceCount];

        for (int i = 0; i < MusicSourceCount; i++)
        {
            musicSources[i] = gameObject.AddComponent<AudioSource>();
            musicSources[i].loop = true;
            musicSources[i].volume = 0f;
        }

        int num = 0;

        for (int j = 0; j < musicChannelCount; j++)
        {
            MusicChannel musicChannel = new MusicChannel();
            musicChannel.MusicSourceIndices = new int[musicSourcePerChannel];
            musicChannel.MusicSourceIndices[0] = num;
            musicChannel.MusicSourceIndices[1] = num + 1;
            musicChannel.ActiveMusicSourceIndex = num;
            musicChannels.Add(musicChannel);
            num += musicSourcePerChannel;
        }
    }
    public void FadeIn(int channel, AudioClip music, bool restartMusic, float time = -1)
    {
        if (music == null) return;

        MusicChannel musicChannel = musicChannels[channel];
        AudioSource audioSource = musicSources[musicChannel.ActiveMusicSourceIndex];

        if (restartMusic || audioSource.clip != music)
        {
            Coroutine coroutine = sourceFadeCoroutines[musicChannel.ActiveMusicSourceIndex];

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            sourceFadeCoroutines[musicChannel.ActiveMusicSourceIndex] = StartCoroutine(CoFadeOut(audioSource, time));

            musicChannel.ActiveMusicSourceIndex = ((musicChannel.ActiveMusicSourceIndex != musicChannel.MusicSourceIndices[0]) ? musicChannel.MusicSourceIndices[0] : musicChannel.MusicSourceIndices[1]);
            audioSource = musicSources[musicChannel.ActiveMusicSourceIndex];

            if (audioSource.clip != music)
            {
                audioSource.clip = music;
                audioSource.Play();
                audioSource.volume = 0f;
            }
            else if (restartMusic)
            {
                audioSource.Play();
            }

            coroutine = sourceFadeCoroutines[musicChannel.ActiveMusicSourceIndex];

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            sourceFadeCoroutines[musicChannel.ActiveMusicSourceIndex] = StartCoroutine(CoFadeIn(audioSource, time));
        }
    }

    public void FadeOut(int channel, float time = -1)
    {
        MusicChannel musicChannel = musicChannels[channel];
        AudioSource audioSource = musicSources[musicChannel.ActiveMusicSourceIndex];

        if (audioSource.clip != null)
        {
            Coroutine coroutine = sourceFadeCoroutines[musicChannel.ActiveMusicSourceIndex];

            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }

            sourceFadeCoroutines[musicChannel.ActiveMusicSourceIndex] = StartCoroutine(CoFadeOut(audioSource, time));
        }
    }

    public void PlaySoundOnce(AudioClip audioClip, float volume)
    {
        if (!(audioClip == null))
        {
            sfxSource.PlayOneShot(audioClip, volume);
        }
    }

    public void PlaySoundOnceDelayed(AudioClip audioClip, float volume, float delay)
    {
        if (!(audioClip == null))
        {
            StartCoroutine(CoDelayedOneShot(audioClip, volume, delay));
        }
    }
    private IEnumerator CoFadeIn(AudioSource audioSource, float time = -1)
    {
        float currentTransititonTime = time == -1f ? transitionTime : time;

        float targetVolume = 1;

        if (audioSource.volume == targetVolume) yield break;

        float t = 0;
        float currentVolume = audioSource.volume;

        while (t < 1)
        {
            audioSource.volume = Mathf.Lerp(currentVolume, targetVolume, t);
            t += Time.deltaTime / currentTransititonTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
    }
    private IEnumerator CoFadeOut(AudioSource audioSource, float time = -1f)
    {
        float currentTransititonTime = time == -1f ? transitionTime : time;

        float targetVolume = 0;

        if (audioSource.volume == targetVolume) yield break;

        float t = 0;
        float currentVolume = audioSource.volume;

        while (t < 1)
        {
            audioSource.volume = Mathf.Lerp(currentVolume, targetVolume, t);
            t += Time.deltaTime / currentTransititonTime;
            yield return null;
        }

        audioSource.volume = targetVolume;
        audioSource.clip = null;
    }

    private IEnumerator CoDelayedOneShot(AudioClip audioClip, float volume, float delay)
    {
        yield return new WaitForSeconds(delay);
        sfxSource.PlayOneShot(audioClip, volume);
    }
}
