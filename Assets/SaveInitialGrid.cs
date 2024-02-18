using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveInitialGrid : MonoBehaviour
{
    [SerializeField] private bool checkTheGrid = false;

    public List<MyTuple> newNotWalkableNodes = new List<MyTuple>();
    public List<MyTuple> oldNotWalkableNodes = new List<MyTuple>();
    public List<MyTuple> excludedNodes = new List<MyTuple>();

    private LocationGridSave locationGrid;

    private Grid grid { get =>  locationGrid.Grid; }

    private void Awake()
    {
        locationGrid = GetComponent<LocationGridSave>();
    }

    private void Start()
    {
        if(newNotWalkableNodes.Count != 0)
        {
            AddNewNodesData();
        }
        if(checkTheGrid)
        {
            StartCoroutine(WaitToCheck());
        }
    }

    public void AddNode(int x, int y)
    {
        oldNotWalkableNodes.Add(new MyTuple((ushort)x, (ushort)y));
    }

    private IEnumerator WaitToCheck()
    {
        for (int indexCellX = 0; indexCellX < grid.gridArray.GetLength(0); indexCellX++)
        {
            for (int indexCellY = 0; indexCellY < grid.gridArray.GetLength(1); indexCellY++)
            {
                GameObject testObjectInGridCell = new GameObject("TestGridCells");

                Vector3 position = grid.GetWorldPositionCenter(grid.gridArray[indexCellX, indexCellY].x, grid.gridArray[indexCellX, indexCellY].y);

                testObjectInGridCell.transform.position = position;

                ChangeGridCellValuesByObjects changeGrid = testObjectInGridCell.AddComponent<ChangeGridCellValuesByObjects>();

                changeGrid.saveTheData = true;

                changeGrid.Grid = grid;

                changeGrid.SetComponents();
            }

            yield return new WaitForSeconds(0);
        }
    }

    private void AddNewNodesData()
    {
        foreach(MyTuple node in newNotWalkableNodes)
        {
            grid.GetGridObject(node.Item1, node.Item2).isWalkable = false;
        }

        foreach(MyTuple node in excludedNodes)
        {
            grid.GetGridObject(node.Item1, node.Item2).isWalkable = true;
        }
    }

}
