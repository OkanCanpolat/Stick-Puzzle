using Zenject;

public class U_Left : IShapePlacementController
{
    [Inject] private GridManager gridManager;

    public bool CanBePlaced()
    {
        for (int column = 0; column < gridManager.VerticalLines.GetLength(0); column++)
        {
            for (int row = 0; row < gridManager.VerticalLines.GetLength(1); row++)
            {
                Line verticalLine = gridManager.GetVerticalLine(column, row);
                Line horizontalLine = gridManager.GetHorizontalLine(column, row);
                Line horizontalLine2 = gridManager.GetHorizontalLine(column, row + 1);

                if (verticalLine == null || horizontalLine == null || horizontalLine2 == null) continue;

                if (verticalLine.IsEmpty && horizontalLine.IsEmpty && horizontalLine2.IsEmpty) return true;
            }
        }

        return false;
    }
}
