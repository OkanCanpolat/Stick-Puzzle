using System;
using UnityEngine;
using Zenject;

public class PointGoal : ILevelGoal
{
    public Action<int> OnPointChanged;
    private SignalBus signalBus;
    private LevelData levelData;
    private PointGoalUIController uIController;
    private int currentPoint = 0;
    private PointGoalConfig goalConfig;
    private bool levelFinished = false;
    public PointGoalConfig GoalConfig => goalConfig;

    public PointGoal(SignalBus signalBus, LevelData levelData, PointGoalUIController uIController)
    {
        this.signalBus = signalBus;
        this.levelData = levelData;
        this.uIController = uIController;
        goalConfig = levelData.PointGoalConfig;
        uIController.gameObject.SetActive(true);
        uIController.Init(this);
    }
    public void CheckGoal()
    {
        if (levelFinished) return;

        currentPoint += goalConfig.PointPerCellBlast;
        currentPoint = Mathf.Clamp(currentPoint, 0, goalConfig.TargetPoint);
        OnPointChanged?.Invoke(currentPoint);

        if(currentPoint >= goalConfig.TargetPoint)
        {
            signalBus.TryFire<LevelWinSignal>();
            levelFinished = true;
        }
    }
}
