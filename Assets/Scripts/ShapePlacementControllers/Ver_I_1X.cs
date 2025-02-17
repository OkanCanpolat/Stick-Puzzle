using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Ver_I_1X : IShapePlacementController
{
    [Inject] private GridManager gridManager;

    public bool CanBePlaced()
    {
        foreach (var line in gridManager.VerticalLines)
        {
            if (line.IsEmpty) return true;
        }

        return false;
    }

    
}
