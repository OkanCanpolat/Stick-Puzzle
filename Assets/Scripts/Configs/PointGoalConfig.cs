using UnityEngine;

[CreateAssetMenu (fileName = "PointGoalConfig", menuName = "Goal Configs/ Point Goal Config")]
public class PointGoalConfig : ScriptableObject
{
    public int TargetPoint;
    public int PointPerCellBlast;
}
