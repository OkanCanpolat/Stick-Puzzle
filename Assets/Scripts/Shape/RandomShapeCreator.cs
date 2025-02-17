using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RandomShapeCreator : IShapeCreator
{
    private ShapeCreator shapeCreator;
    private SignalBus signalBus;
    [Inject] private ShapeCreatorConfig config;
    [Inject] private LevelData levelData;
    [Inject] private ShapeObjectPool objectPool;
    private System.Random rnd;
    public RandomShapeCreator(ShapeCreator shapeCreator, SignalBus signalBus)
    {
        this.signalBus = signalBus;
        this.shapeCreator = shapeCreator;
        signalBus.Subscribe<ShapePlacedSignal>(OnShapePlaced);
        rnd = new System.Random();
    }
    public bool CreateOnePossibleShape()
    {
        config.Shapes.Shuffle(rnd);

        foreach (var shape in config.Shapes)
        {
            ShapeController shapeController = objectPool.Get(shape.Type);

            if (shapeController.ShapePlacementController.CanBePlaced())
            {
                Vector3 postion = shapeCreator.ShapeLocations[0].position;
                shapeController.InitialPosition = postion;
                shapeController.SetColor(levelData.ShapeColor);
                shapeController.transform.position = postion + config.InstantiateOffset;
                shapeCreator.CreatedShapes.Add(shapeController);
                return true;
            }
            else
            {
                objectPool.ReturnToPool(shapeController);
            }
        }

        return false;
    }

    public void CreateShapes()
    {
        if (!CreateOnePossibleShape())
        {
            Debug.LogError("Can Not Find Possible Shape to Create!");
            return;
        }



        for (int i = 1; i < shapeCreator.InitialShapeCount; i++)
        {
            int probability = Random.Range(0, 5);
            List<ShapeController> targetList = probability <= 2 ? config.HighProbabilityShapes : config.LowProbabilityShapes;
            int index = UnityEngine.Random.Range(0, targetList.Count);
            Vector3 postion = shapeCreator.ShapeLocations[i].position;
            ShapeController shapeController = objectPool.Get(targetList[index].Type);
            shapeController.InitialPosition = postion;
            shapeController.SetColor(levelData.ShapeColor);
            shapeController.transform.position = postion + config.InstantiateOffset;
            shapeCreator.CreatedShapes.Add(shapeController);
        }

        shapeCreator.StartCoroutine(shapeCreator.CoSlideShapes());
    }

    public void OnShapePlaced(ShapePlacedSignal signal)
    {
        shapeCreator.CreatedShapes.Remove(signal.Shape);

        if (shapeCreator.CreatedShapes.Count <= 0)
        {
            CreateShapes();
        }
    }
}
