using Zenject;

public class L_UL : IShapePlacementController
{
    [Inject] private GridManager gridManager;

    public bool CanBePlaced()
    {
        for (int column = 0; column < gridManager.VerticalLines.GetLength(0); column++)
        {
            for (int row = 0; row < gridManager.VerticalLines.GetLength(1); row++)
            {
                Line verticalLine = gridManager.GetVerticalLine(column, row);
                Line horizontalLine = gridManager.GetHorizontalLine(column - 1, row + 1);

                if (verticalLine == null || horizontalLine == null) continue;

                if (verticalLine.IsEmpty && horizontalLine.IsEmpty) return true;
            }
        }

        return false;
    }
}
