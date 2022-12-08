using System;
using UnityEngine;

public class Grid<TGridObject>
{
    private int height;
    private int width;
    private float cellSize;

    private Vector3 originPosition;

    public TGridObject[,] gridArray;

    public int Height { get { return height; } }
    public int Width { get { return width; } }
    public float CellSize { get { return cellSize; } }

    public Grid(int height, int width, float cellSize, Vector3 originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = true;
        if (showDebug)
        {

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public Vector3 GetWorldPosition(GridNode gridNode)
    {
        return GetWorldPosition(gridNode.x, gridNode.y);
    }

    public bool VerifyDistanceBetweenTwoNodes(GridNode node1, GridNode node2, int maxDistance)
    {
        if (Math.Abs(node1.x - node2.x) > maxDistance) return false;
        if (Math.Abs(node1.y - node2.y) > maxDistance) return false;

        return true;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public TGridObject GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition)
    {
        int x;
        int y;

        GetXY(worldPosition, out x, out y);

        return GetGridObject(x, y);
    }


}
