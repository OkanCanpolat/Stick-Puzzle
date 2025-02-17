using UnityEngine;
using Zenject;

public class LevelWinController : MonoBehaviour
{
    [Inject] private ILevelGoal levelGoal;
    [Inject] private SignalBus signalBus;

    private void Awake()
    {
        signalBus.Subscribe<BlastSignal>(CheckGoal);
    }

    public void CheckGoal()
    {
        levelGoal.CheckGoal();
    }
}
