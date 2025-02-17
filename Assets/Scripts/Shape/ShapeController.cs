using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class ShapeController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public ShapeType Type;
    public Vector3 MinimizedScale;
    [Inject] public MouseEventStateMachine StateMachine;
    [Inject] private IShapePlacementController shapePlacementController;
    [Inject] private ShapeCreator shapeCreator;
    private List<Shape> childShapes;
    private List<RaycastHit2D> raycastHits;
    private Vector3 initialPosition;

    [Inject(Id = "MouseLockState")] public MouseLockState MouseLockState;
    [Inject(Id = "ReadySelectionState")] public MouseReadySelectionState ReadySelectionState;

    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public List<Shape> ChildShapes => childShapes;
    public IShapePlacementController ShapePlacementController => shapePlacementController;
    private void Awake()
    {
        transform.localScale = MinimizedScale;
        raycastHits = new List<RaycastHit2D>();
        childShapes = GetComponentsInChildren<Shape>().ToList();
    }
    private void Start()
    {
        AdjustColliderScale();
    }
    public void SetColor(Color color)
    {
        foreach(Shape shape in childShapes)
        {
            shape.SetColor(color);
        }
    }
   
    public bool IsSharedLine(Line line)
    {
        bool value = false;

        foreach (var item in childShapes)
        {
            if(item.lastCastedLine == line)
            {
                return true;
            }
        }

        return value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StateMachine.CurrentState.OnPointerDown();
    }

    public void OnDrag(PointerEventData eventData)
    {
        StateMachine.CurrentState.OnDrag();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StateMachine.CurrentState.OnPointerUp();

    }
    public void ChangeShapeRendererLayer(int value)
    {
        foreach (var shape in childShapes)
        {
            shape.SpriteRenderer.sortingOrder += value;
        }
    }

    private void AdjustColliderScale()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Vector3 currentScale = transform.localScale;
        Vector3 containerScale = shapeCreator.transform.localScale;
        float x = (containerScale.x / shapeCreator.InitialShapeCount) / currentScale.x;
        float y = containerScale.y / currentScale.y;

        if(transform.eulerAngles.z == 90f || transform.eulerAngles.z == 270f)
        {
            col.size = new Vector2(y, x);
        }
        else
        {
            col.size = new Vector2(x, y);
        }
    }
}
