using System.Collections;
using UnityEngine;
using Zenject;

public class LoseScreenUIController : MonoBehaviour
{
    [SerializeField] private GameObject losePanel;
    [Inject] private SignalBus signalBus;
    [Inject] private LoseScreenConfig loseScreenConfig;
    [Inject] private IAudioService audioService;
    [Inject] private SoundConfig soundConfig;
    private void Awake()
    {
        signalBus.Subscribe<LevelLoseSignal>(OnLoseGame);
    }

    public void OnLoseGame()
    {
        StartCoroutine(CoOnLoseGame());
    }
    private IEnumerator CoOnLoseGame()
    {
        yield return new WaitForSeconds(loseScreenConfig.OutMoveSoundDelay);
        audioService.PlaySoundOnce(soundConfig.OutMovesSound, soundConfig.OutMovesVolume);
        yield return new WaitForSeconds(loseScreenConfig.LoseSCreenOpenDelay);
        losePanel.SetActive(true);
        audioService.PlaySoundOnce(soundConfig.LoseSound, soundConfig.LoseVolume);
    }
}
