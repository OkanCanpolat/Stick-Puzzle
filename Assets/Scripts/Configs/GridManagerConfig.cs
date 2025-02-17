using UnityEngine;

[CreateAssetMenu(fileName = "GridManagerConfig", menuName = "Configurations / Grid Manager Config")]

public class GridManagerConfig : ScriptableObject
{
    public Dot DotPrefab;
    public Cell CellPrefab;
    public Line HorizontalLinePrefab;
    public Line VerticalLinePrefab;
    public readonly float DistanceBetweenDots = 1f;
}
