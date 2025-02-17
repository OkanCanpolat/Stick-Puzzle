using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class RestartButton : MonoBehaviour
{
    [Inject] private SoundConfig soundConfig;
    [Inject] private IAudioService audioService;
    public void OnClick()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        audioService.PlaySoundOnce(soundConfig.ButtonSound, soundConfig.ButtonVolume);
        SceneManager.LoadScene(index);
    }
}
