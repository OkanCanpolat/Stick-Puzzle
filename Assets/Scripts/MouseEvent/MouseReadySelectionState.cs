using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MouseReadySelectionState : IMouseEventState
{
    private Vector3 offset;
    private ShapeController shapeController;
    private Vector3 orijinalScale = Vector3.one;
    [Inject] private GridManager gridManager;
    [Inject] private SignalBus signalBus;
    private ShapePlacedSignal placedSignal;
    private List<Line> tempHighligtedLines;
    [Inject] private ShapeObjectPool objectPool;

    public Vector3 OrijinalScale => orijinalScale;

    public MouseReadySelectionState(ShapeController shapeController)
    {
        this.shapeController = shapeController;
        placedSignal = new ShapePlacedSignal(shapeController);
        tempHighligtedLines = new List<Line>();
    }

    public void OnDrag()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        targetPos.z = 0;
        shapeController.transform.position = targetPos;

        if (ControlRaycasts())
        {
            foreach (Shape shape in shapeController.ChildShapes)
            {
                RaycastHit2D hit = Physics2D.Raycast(shape.RaycastPosition.position, Vector2.zero, 100f, shape.Mask);
                Line line = hit.collider.gameObject.GetComponent<Line>();

                if (shape.lastCastedLine == null)
                {
                    shape.lastCastedLine = line;
                    line.Highligh(shape.Color);
                    tempHighligtedLines.Add(line);
                }

                if (shape.lastCastedLine != line)
                {
                    Line previousLine = shape.lastCastedLine;
                    shape.lastCastedLine = line;
                    if (!shapeController.IsSharedLine(previousLine)) previousLine.BackToInitialColor(shape.Color);
                    line.Highligh(shape.Color);
                    tempHighligtedLines.Add(line);
                    gridManager.RevertDummyBlast();
                }
            }

            ControlDummyBlast();
        }
        else
        {
            foreach (Shape shape in shapeController.ChildShapes)
            {
                if (shape.lastCastedLine != null)
                {
                    shape.lastCastedLine.BackToInitialColor(shape.Color);
                    shape.lastCastedLine = null;
                }
            }

            gridManager.RevertDummyBlast();
        }
    }

    public void OnPointerDown()
    {
        offset = shapeController.transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        shapeController.transform.localScale = orijinalScale;
        shapeController.ChangeShapeRendererLayer(1);
    }

    public void OnPointerUp()
    {
        if (ControlRaycasts())
        {
            foreach (var shape in shapeController.ChildShapes)
            {
                shape.lastCastedLine.OnPlacement(shape.Color);
                shape.lastCastedLine.IsEmpty = false;
            }

            gridManager.RevertDummyBlast();
            gridManager.ControlCellPaint(shapeController.ChildShapes[0].Color);
            gridManager.ControlBlast();
            objectPool.ReturnToPool(shapeController);
            signalBus.TryFire(placedSignal);
        }

        else
        {
            shapeController.transform.localScale = shapeController.MinimizedScale;
            shapeController.transform.position = shapeController.InitialPosition;
            shapeController.ChangeShapeRendererLayer(-1);
        }
    }

    public bool ControlRaycasts()
    {
        bool value = true;

        foreach (Shape shape in shapeController.ChildShapes)
        {
            RaycastHit2D hit = Physics2D.Raycast(shape.RaycastPosition.position, Vector2.zero, 100f, shape.Mask);
            Line line = null;

            if (hit)
            {
                line = hit.collider.GetComponent<Line>();
            }

            if (!hit || !line.IsEmpty)
            {
                return false;
            }
        }

        return value;
    }
    public void ControlDummyBlast()
    {
        if (tempHighligtedLines.Count <= 0) return;

        foreach (var line in tempHighligtedLines)
        {
            line.IsEmpty = false;
        }
        gridManager.ControlCellPaintDummy();
        gridManager.ControlBlastDummy();

        foreach (var line in tempHighligtedLines)
        {
            line.IsEmpty = true;
        }

        gridManager.RevertDummyPaint();

        tempHighligtedLines.Clear();
    }

}
