using System.Collections;
using System.Collections.Generic;
using Zenject;

public class I_Hor_2X : IShapePlacementController
{
    [Inject] private GridManager gridManager;

    public bool CanBePlaced()
    {
        for (int column = 0; column < gridManager.HorizontalLines.GetLength(0); column++)
        {
            for (int row = 0; row < gridManager.HorizontalLines.GetLength(1); row++)
            {
                Line horizontalLine = gridManager.GetHorizontalLine(column, row);
                Line horizontalLine2 = gridManager.GetHorizontalLine(column + 1, row);

                if (horizontalLine == null || horizontalLine2 == null) continue;
                if (horizontalLine.IsEmpty && horizontalLine2.IsEmpty) return true;
            }
        }

        return false;
    }

    
}
