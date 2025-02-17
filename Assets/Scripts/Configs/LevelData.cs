using UnityEngine;

[CreateAssetMenu (fileName = "LevelData", menuName = "Level Data")]
public class LevelData : ScriptableObject
{
    public int Column;
    public int Row;
    public GoalType GoalType;
    public PointGoalConfig PointGoalConfig;
    public Color ShapeColor;
}
