using System.Collections;
using UnityEngine;
using Zenject;

public class WinScreenUIController : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [Inject] private SignalBus signalBus;
    [Inject] private WinScreenConfig winScreenConfig;
    [Inject] private IAudioService audioService;
    [Inject] private SoundConfig soundConfig;
    private void Awake()
    {
        signalBus.Subscribe<LevelWinSignal>(OnWinLevel);
    }

    public void OnWinLevel()
    {
        StartCoroutine(CoOnWinLevel());
    }
    public IEnumerator CoOnWinLevel()
    {
        yield return new WaitForSeconds(winScreenConfig.WinScreenOpenDelay);
        winScreen.SetActive(true);
        audioService.PlaySoundOnce(soundConfig.WinSound, soundConfig.WinVolume);
    }
}
