using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ShapeObjectPool
{
    private Dictionary<ShapeType, Queue<ShapeController>> objects;
    private Factory factory;
    private ShapeCreatorConfig creatorConfig;
    private int initialCount = 3;
    public ShapeObjectPool(ShapeCreatorConfig creatorConfig, Factory factory)
    {
        this.creatorConfig = creatorConfig;
        this.factory = factory;
        objects = new Dictionary<ShapeType, Queue<ShapeController>>();
    }

    public ShapeController Get(ShapeType type)
    {
        if (!objects.ContainsKey(type)) return null;

        if (objects[type].Count <= 0)
        {
            InstantiatePoolObject(type);
        }

        ShapeController poolObject = objects[type].Dequeue();
        poolObject.gameObject.SetActive(true);
        return poolObject;
    }

    public void ReturnToPool(ShapeController poolObject)
    {
        if (!objects.ContainsKey(poolObject.Type)) return;

        poolObject.ChangeShapeRendererLayer(-1);
        poolObject.gameObject.SetActive(false);
        poolObject.transform.localScale = poolObject.MinimizedScale;

        foreach (Shape shape in poolObject.ChildShapes)
        {
            shape.lastCastedLine = null;
        }

        objects[poolObject.Type].Enqueue(poolObject);
    }
    public void SetupPool()
    {
        foreach (ShapeController shape in creatorConfig.Shapes)
        {
            ShapeType type = shape.Type;

            if (objects.ContainsKey(type)) continue;

            Queue<ShapeController> objectQueue = new Queue<ShapeController>();

            objects.Add(type, objectQueue);

            for (int i = 0; i < initialCount; i++)
            {
                ShapeController poolObject = factory.Create(shape, Vector3.right, shape.transform.rotation, null);
                poolObject.gameObject.SetActive(false);
                objectQueue.Enqueue(poolObject);
            }
        }
    }
    private void InstantiatePoolObject(ShapeType type)
    {
        if (!objects.ContainsKey(type)) return;

        foreach (ShapeController shape in creatorConfig.Shapes)
        {
            if (shape.Type == type)
            {
                ShapeController poolObject = factory.Create(shape, Vector3.right, shape.transform.rotation, null);
                poolObject.gameObject.SetActive(false);
                objects[type].Enqueue(poolObject);
                return;
            }
        }
    }
}
