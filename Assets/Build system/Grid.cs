using System;
using System.Collections.Generic;
using UnityEngine;

public class Grid
{
    private int height;
    private int width;
    private float cellSize;

    private Vector3 originPosition;

    public GridNode[,] gridArray;

    public int Height { get { return height; } }
    public int Width { get { return width; } }
    public float CellSize { get { return cellSize; } }

    public Grid(int height, int width, float cellSize, Vector3 originPosition, Func<Grid, int, int, GridNode> createGridObject)
    {
        this.height = height;
        this.width = width;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new GridNode[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = false;
        if (showDebug)
        {

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                Debug.DrawLine(GetWorldPosition(x, 0), GetWorldPosition(x, height), Color.white, 100f);

                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Debug.DrawLine(GetWorldPosition(0, y), GetWorldPosition(width, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        Vector3 position = new Vector3(x, y) * cellSize + originPosition;

        return position;
    }

    public Vector3 GetWorldPositionCenter(int x, int y)
    {
        Vector3 position = new Vector3(x, y) * cellSize + originPosition;

        position.x += cellSize / 2;
        position.y += cellSize / 2;

        return position;
    }

    public Vector3 GetWorldPosition(GridNode gridNode)
    {
        return GetWorldPosition(gridNode.x, gridNode.y);
    }

    public Vector3 GetWorldPositionFronGridObject(Vector3 position)
    {
        GetXY(position, out int x, out int y);

        return GetWorldPosition(x, y);
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

    public GridNode GetGridObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return null;
        }
    }

    public GridNode GetGridObject(Vector3 worldPosition)
    {
        GetXY(worldPosition, out int x, out int y);

        return GetGridObject(x, y);
    }

    public void ReinitializeGrid(Placeable placeable, Vector3 position)
    {
        GridNode changePosition = GetGridObject(position);

        if (changePosition != null)
        {
            for (int i = changePosition.x + placeable.StartX; i <= changePosition.x + placeable.SizeX; i++)
            {
                for (int j = changePosition.y + placeable.StartY; j <= changePosition.y + placeable.SizeY; j++)
                {
                    if (i >= 0 && i < width &&
                        j >= 0 && j < height)
                    {
                        ReinitializeGrid(gridArray[i, j]);
                    }
                }
            }
        } 
    }

    public void ReinitializeGrid(Vector3 position)
    {
        GridNode changePosition = GetGridObject(position);

        ReinitializeGrid(changePosition);
    }

    public void ReinitializeGrid(GridNode changePosition)
    {
        if (changePosition != null)
        {
            gridArray[changePosition.x, changePosition.y].canPlace = true;
            gridArray[changePosition.x, changePosition.y].canPlant = false;
            gridArray[changePosition.x, changePosition.y].isWalkable = true;
            gridArray[changePosition.x, changePosition.y].cropPlaced = false;
            gridArray[changePosition.x, changePosition.y].crop = null;
            gridArray[changePosition.x, changePosition.y].objectInSpace = null;
        }
    }

    public List<GameObject> PlaceObjectInGrid(Placeable placeable, GridNode position, GameObject newObject = null)
    {
        List<GameObject> objectsToDestroy = new List<GameObject>();

        if (position != null)
        {
            for (int i = position.x + placeable.StartX; i <= position.x + placeable.SizeX; i++)
            {
                for (int j = position.y + placeable.StartY; j <= position.y + placeable.SizeY; j++)
                {
                    if (i >= 0 && i < width &&
                        j >= 0 && j < height &&
                        gridArray[i, j] != null)
                    {
                        if (gridArray[i, j].objectInSpace != null &&
                            gridArray[i, j].objectInSpace.CompareTag("FarmPlot") &&
                            gridArray[i, j].cropPlaced == false &&
                            !objectsToDestroy.Contains(gridArray[i, j].objectInSpace))
                        {
                            objectsToDestroy.Add(gridArray[i, j].objectInSpace);
                        }

                        gridArray[i, j].canPlace = false;
                        gridArray[i, j].canPlant = false;
                        gridArray[i, j].isWalkable = false;
                        gridArray[i, j].objectInSpace = newObject;
                    }
                }
            }
        }

        return objectsToDestroy;
    }

    public bool CheckCanPlaceBuildSystem(Placeable placeable, GridNode gridNode)
    {
        if (gridNode != null)
        {
            if (gridNode.x + placeable.StartX >= 0 &&
                gridNode.x + placeable.StartY >= 0 &&
                gridNode.x + placeable.SizeX < width &&
                gridNode.y + placeable.SizeY < height)
            {
                for (int i = gridNode.x + placeable.StartX; i <= gridNode.x + placeable.SizeX; i++)
                {
                    for (int j = gridNode.y + placeable.StartY; j <= gridNode.y + placeable.SizeY; j++)
                    {
                        if (gridArray[i, j].canPlace == false || gridArray[i, j].isWalkable == false)
                        {
                            if (gridArray[i, j].objectInSpace != null)
                            {
                                if (gridArray[i, j].objectInSpace.gameObject.tag != "FarmPlot" ||
                                   (gridArray[i, j].objectInSpace.gameObject.tag == "FarmPlot" && gridArray[i, j].cropPlaced == true))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }

                return true;
            }
        }
        return false;
    }
}
