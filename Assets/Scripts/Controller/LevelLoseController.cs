using UnityEngine;
using Zenject;

public class LevelLoseController : MonoBehaviour
{
    [Inject] private ShapeCreator shapeCreator;
    [Inject] private SignalBus signalBus;
   
    private void Start()
    {
        signalBus.Subscribe<ShapePlacedSignal>(ControlPlacement);
    }

    private void ControlPlacement()
    {
        bool canPlaced = false;
        foreach (var shape in shapeCreator.CreatedShapes)
        {
            if (shape.ShapePlacementController.CanBePlaced())
            {
                canPlaced = true;
                break;
            }
        }

        if (!canPlaced)
        {
            signalBus.TryFire<LevelLoseSignal>();
        }
    }
}
