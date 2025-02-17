using Zenject;

public class U_Down : IShapePlacementController
{
    [Inject] private GridManager gridManager;

    public bool CanBePlaced()
    {
        for (int column = 0; column < gridManager.HorizontalLines.GetLength(0); column++)
        {
            for (int row = 0; row < gridManager.HorizontalLines.GetLength(1); row++)
            {
                Line horizontalLine = gridManager.GetHorizontalLine(column, row);
                Line verticalLine = gridManager.GetVerticalLine(column, row);
                Line verticalLine2 = gridManager.GetVerticalLine(column + 1, row);

                if (verticalLine == null || horizontalLine == null || verticalLine2 == null) continue;

                if (verticalLine.IsEmpty && horizontalLine.IsEmpty && verticalLine2.IsEmpty) return true;
            }
        }

        return false;
    }
}
