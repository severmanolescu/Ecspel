using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationGridSave : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int weight;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 position = new Vector3();

    [SerializeField] private bool canPlantToGrid;

    private Grid<GridNode> grid;

    public Grid<GridNode> Grid { get { return grid; } }

    public bool CanPlantToGrid { get => canPlantToGrid; set => canPlantToGrid = value; }

    private void Awake()
    {
        grid = new Grid<GridNode>(height, weight, cellSize, position, (Grid<GridNode> g, int x, int y) => new GridNode(g, x, y));
    }

    private void Start()
    {
        StartCoroutine(WaitToCheck());
    }

    IEnumerator WaitToCheck()
    {
        for (int indexCellX = 0; indexCellX < grid.gridArray.GetLength(0); indexCellX++)
        {
            for (int indexCellY = 0; indexCellY < grid.gridArray.GetLength(1); indexCellY++)
            {
                GameObject testObjectInGridCell = new GameObject("TestGridCells");

                testObjectInGridCell.AddComponent<ChangeGridCellValuesByObjects>().Grid = Grid;

                Vector3 position = grid.GetWorldPosition(grid.gridArray[indexCellX, indexCellY].x, grid.gridArray[indexCellX, indexCellY].y);

                position.x += grid.CellSize / 2f;
                position.y += grid.CellSize / 2f;

                testObjectInGridCell.transform.position = position;

                testObjectInGridCell.GetComponent<ChangeGridCellValuesByObjects>().SetComponents();
            }

            yield return new WaitForSeconds(0.05f);
        }

        GameObject loadScene = GameObject.FindGameObjectWithTag("LoadScene");

        if (loadScene != null)
        {
            loadScene.GetComponent<LoadSceneHandler>().FinishGridSearchProcess++;
        }
    }
}
