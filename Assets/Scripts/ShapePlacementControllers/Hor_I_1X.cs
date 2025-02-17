using Zenject;

public class Hor_I_1X : IShapePlacementController
{
    [Inject] private GridManager gridManager;
    public bool CanBePlaced()
    {
        foreach (var line in gridManager.HorizontalLines)
        {
            if (line.IsEmpty) return true;
        }

        return false;
    }
}
