using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class NextButton : MonoBehaviour
{
    [Inject] private SoundConfig soundConfig;
    [Inject] private IAudioService audioService;

 
    public void OnClick()
    {
        audioService.PlaySoundOnce(soundConfig.ButtonSound, soundConfig.ButtonVolume);
        int maxLevel = SceneManager.sceneCountInBuildSettings;
        int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

        if(nextLevel < maxLevel)
        {
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
