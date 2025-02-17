using UnityEngine;
using Zenject;

public class I_Ver_2X : IShapePlacementController
{
    [Inject] private GridManager gridManager;

    public bool CanBePlaced()
    {
        for (int column = 0; column < gridManager.VerticalLines.GetLength(0); column++)
        {
            for (int row = 0; row < gridManager.VerticalLines.GetLength(1); row++)
            {
                Line verticalLine = gridManager.GetVerticalLine(column, row);
                Line verticalLine2 = gridManager.GetVerticalLine(column, row + 1);

                if (verticalLine == null || verticalLine2 == null) continue;
                if (verticalLine.IsEmpty && verticalLine2.IsEmpty) return true;

            }
        }

        return false;
    }
}
