using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode
{
    private Grid<GridNode> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public bool isWalkable;
    public bool canPlant;
    public bool canPlace;
    public bool cropPlaced;

    public SpriteRenderer currentObject;

    public GridNode cameFromNode;

    public GameObject objectInSpace;
    public GameObject crop;

    public GridNode(Grid<GridNode> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        isWalkable = true;
        canPlace = true;
        canPlant = false;
        cropPlaced = false;
        objectInSpace = null;
        crop = null;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + "," + y;
    }
}
