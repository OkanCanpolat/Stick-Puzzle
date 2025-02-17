using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridManager : MonoBehaviour
{
    private Dot[,] dots;
    private Cell[,] cells;
    private Line[,] horizontalLines;
    private Line[,] verticalLines;
   
    List<Cell> tempBlastList = new List<Cell>();
    List<Cell> blastList = new List<Cell>();
    [Inject] private SignalBus signalBus;
    [Inject] private SoundConfig soundConfig;
    [Inject] private IAudioService audioService;
    [Inject] private GridManagerConfig managerConfig;
    [Inject] private LevelData levelData;

    public Line[,] HorizontalLines => horizontalLines;
    public Line[,] VerticalLines => verticalLines;
    public int Column => levelData.Column;
    public int Row => levelData.Row;
    private void Awake()
    {
        Application.targetFrameRate = 60;

        dots = new Dot[levelData.Column, levelData.Row];
        cells = new Cell[levelData.Column - 1, levelData.Row - 1];
        horizontalLines = new Line[levelData.Column - 1, levelData.Row];
        verticalLines = new Line[levelData.Column, levelData.Row - 1];
    }

    private void Start()
    {
        CreateDots();
        ConnectDots();
        CreateCells();
        signalBus.TryFire<GridCreatedSignal>();
    }
    public void CreateDots()
    {
        for (int i = 0; i < levelData.Column; i++)
        {
            for (int j = 0; j < levelData.Row; j++)
            {
                Vector2 pos = new Vector2(i, j);
                Dot dot = Instantiate(managerConfig.DotPrefab, pos, Quaternion.identity);
                dots[i, j] = dot;
            }
        }
    }

    public void ConnectDots()
    {
        for (int i = 0; i < levelData.Row; i++)
        {
            for (int j = 0; j < levelData.Column - 1; j++)
            {
                Dot dot1 = dots[j, i];
                Dot dot2 = dots[j + 1, i];
                Vector2 linePos = (dot1.transform.position + dot2.transform.position) / 2;
                Line line = Instantiate(managerConfig.HorizontalLinePrefab, linePos, Quaternion.identity);
                line.ConnectedDots[0] = dot1;
                line.ConnectedDots[1] = dot2;
                horizontalLines[j, i] = line;
            }
        }

        for (int i = 0; i < levelData.Column; i++)
        {
            for (int j = 0; j < levelData.Row - 1; j++)
            {
                Dot dot1 = dots[i, j];
                Dot dot2 = dots[i, j + 1];
                Vector2 linePos = (dot1.transform.position + dot2.transform.position) / 2;
                Line line = Instantiate(managerConfig.VerticalLinePrefab, linePos, Quaternion.identity);
                line.ConnectedDots[0] = dot1;
                line.ConnectedDots[1] = dot2;
                verticalLines[i, j] = line;
            }
        }
    }

    public void CreateCells()
    {
        for (int i = 0; i < levelData.Column - 1; i++)
        {
            for (int j = 0; j < levelData.Row - 1; j++)
            {
                Vector2 pos = new Vector2(i + managerConfig.DistanceBetweenDots / 2, j + managerConfig.DistanceBetweenDots / 2);
                Cell cell = Instantiate(managerConfig.CellPrefab, pos, Quaternion.identity);
                cell.Column = i;
                cell.Row = j;
                Line line1 = horizontalLines[i, j];
                Line line2 = horizontalLines[i, j + 1];
                Line line3 = verticalLines[i, j];
                Line line4 = verticalLines[i + 1, j];
                cell.RegisterLines(line1, line2, line3, line4);
                cells[i, j] = cell;
            }
        }
    }

    public Line GetHorizontalLine(int column, int row)
    {
        if(column < 0 || column >= horizontalLines.GetLength(0) || row < 0 || row >= horizontalLines.GetLength(1))
        {
            return null;
        }

        return horizontalLines[column, row];
    }
    public Line GetVerticalLine(int column, int row)
    {
        if (column < 0 || column >= verticalLines.GetLength(0) || row < 0 || row >= verticalLines.GetLength(1))
        {
            return null;
        }

        return verticalLines[column, row];
    }
    public void ControlCellPaint(Color color)
    {
        foreach (Cell cell in cells)
        {
            if (cell.IsPainted) continue;
            cell.ControlPaint(color);
        }
    }
    public void ControlCellPaintDummy()
    {
        foreach (Cell cell in cells)
        {
            if (cell.IsPainted) continue;
            cell.ControlPaintDummy();
        }
    }
    public void RevertDummyPaint()
    {
        foreach (Cell cell in cells)
        {
            cell.RevertDummyPaint();
        }
    }
    public void ControlBlast()
    {
        SetBlastCells();

        if (blastList.Count <= 0) return;

        audioService.PlaySoundOnce(soundConfig.BlastSound, soundConfig.BlastVolume);

        foreach (var item in blastList)
        {
            item.Blast();
            signalBus.TryFire<BlastSignal>();
        }
    }
    public void ControlBlastDummy()
    {
        SetBlastCells();

        foreach (var cell in blastList)
        {
            cell.Shine(true);
        }
    }
    public void RevertDummyBlast()
    {
        foreach (var cell in blastList)
        {
            if (cell.IsShining)
            {
                cell.Shine(false);
            }
        }
    }
    
    private void SetBlastCells()
    {
        bool verticalBlast;
        bool horizontalBlast;
        blastList.Clear();

        for (int i = 0; i < levelData.Column - 1; i++)
        {
            verticalBlast = true;

            for (int j = 0; j < levelData.Row - 1; j++)
            {
                Cell cell = cells[i, j];

                if (!cell.IsPainted)
                {
                    verticalBlast = false;
                    break;
                }
                tempBlastList.Add(cell);
            }

            if (verticalBlast)
            {
                blastList.MergeUniques(tempBlastList);
            }

            tempBlastList.Clear();
        }

        for (int i = 0; i < levelData.Row - 1; i++)
        {
            horizontalBlast = true;

            for (int j = 0; j < levelData.Column - 1; j++)
            {
                Cell cell = cells[j, i];

                if (!cell.IsPainted)
                {
                    horizontalBlast = false;
                    break;
                }
                tempBlastList.Add(cell);
            }

            if (horizontalBlast)
            {
                blastList.MergeUniques(tempBlastList);
            }

            tempBlastList.Clear();
        }
    }
}
