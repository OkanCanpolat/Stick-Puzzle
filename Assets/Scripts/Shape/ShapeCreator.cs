using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ShapeCreator : MonoBehaviour
{
    [SerializeField] private List<Transform> shapeLocations;
    private int initialShapeCount;
    [Inject] private ShapeCreatorConfig config;
    [Inject] private SignalBus signalBus;
    [Inject] private LevelData levelData;
    [Inject] private ShapeObjectPool objectPool;
    [Inject] private IShapeCreator shapeCreator;
    private List<ShapeController> createdShapes;
    private WaitForSeconds waitForSeconds;

    public List<Transform> ShapeLocations => shapeLocations;
    public List<ShapeController> CreatedShapes => createdShapes;
    public int InitialShapeCount => initialShapeCount;
    private void Awake()
    {
        initialShapeCount = shapeLocations.Count;
        createdShapes = new List<ShapeController>();
        waitForSeconds = new WaitForSeconds(config.ShapeSlideSpeed);
        objectPool.SetupPool();
        signalBus.Subscribe<GridCreatedSignal>(() => shapeCreator.CreateShapes());
    }

    public void CreateShapes()
    {
        for (int i = 0; i < initialShapeCount; i++)
        {
            int index = Random.Range(0, config.Shapes.Count);
            Vector3 postion = shapeLocations[i].position;
            ShapeController shapeController = objectPool.Get(config.Shapes[index].Type);
            shapeController.InitialPosition = postion;
            shapeController.SetColor(levelData.ShapeColor);
            shapeController.transform.position = postion + config.InstantiateOffset;
            createdShapes.Add(shapeController);
        }
        StartCoroutine(CoSlideShapes());
    }

    public void OnShapePlaced(ShapePlacedSignal signal)
    {
        createdShapes.Remove(signal.Shape);

        if (createdShapes.Count <= 0)
        {
            CreateShapes();
        }
    }

    public IEnumerator CoSlideShapes()
    {
        for (int i = 0; i < createdShapes.Count; i++)
        {
            iTween.MoveTo(createdShapes[i].gameObject, iTween.Hash("position", shapeLocations[i].position, "time", config.ShapeSlideSpeed, "easetype", iTween.EaseType.linear));
        }

        yield return waitForSeconds;

        signalBus.TryFire<ShapesCreatedSignal>();
    }
}


public class Factory : PlaceholderFactory<ShapeController, Vector3, Quaternion, Transform, ShapeController> { }
public class CustomFactory : IFactory<ShapeController, Vector3, Quaternion, Transform, ShapeController>
{
    readonly private DiContainer container;
    public CustomFactory(DiContainer container)
    {
        this.container = container;
    }
    public ShapeController Create(ShapeController prefab, Vector3 pos, Quaternion rotation, Transform parent)
    {
        return container.InstantiatePrefabForComponent<ShapeController>(prefab, pos, rotation, parent);
    }
}
