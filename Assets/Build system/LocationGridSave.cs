using System.Collections;
using UnityEngine;

public class LocationGridSave : MonoBehaviour
{
    [SerializeField] private int height;
    [SerializeField] private int weight;
    [SerializeField] private float cellSize;
    [SerializeField] private Vector3 position = new Vector3();

    [SerializeField] private bool canPlantToGrid;

    public bool test = false;

    private SpawnEnemyInArea[] spawnLocations;

    private DayTimerHandler dayTimer;

    public Grid grid;

    public Grid Grid { get { return grid; } }

    public bool CanPlantToGrid { get => canPlantToGrid; set => canPlantToGrid = value; }

    private void Awake()
    {
        ReinitializeGrid();

        dayTimer = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();

        spawnLocations = gameObject.GetComponentsInChildren<SpawnEnemyInArea>();

        position = transform.position;
    }

    public void ReinitializeGrid()
    {
        grid = new Grid(height, weight, cellSize, position, (Grid g, int x, int y) => new GridNode(g, x, y));
    }

    public Vector3 GetNeighborNode(Vector3 position, int x, int y)
    {
        GridNode gridNode = grid.GetGridObject(position);

        if (gridNode != null)
        {
            return grid.GetWorldPosition(gridNode.x + x, gridNode.y + y);
        }

        return DefaulData.nullVector;
    }

    private IEnumerator WaitToCheck(NewGameLoadingHandler newGameLoading)
    {
        for (int indexCellX = 0; indexCellX < grid.gridArray.GetLength(0); indexCellX++)
        {
            for (int indexCellY = 0; indexCellY < grid.gridArray.GetLength(1); indexCellY++)
            {
                GameObject testObjectInGridCell = new GameObject("TestGridCells");

                Vector3 position = grid.GetWorldPosition(grid.gridArray[indexCellX, indexCellY].x, grid.gridArray[indexCellX, indexCellY].y);

                position.x += grid.CellSize / 2f;
                position.y += grid.CellSize / 2f;

                testObjectInGridCell.transform.position = position;

                testObjectInGridCell.AddComponent<ChangeGridCellValuesByObjects>().Grid = Grid;

                testObjectInGridCell.GetComponent<ChangeGridCellValuesByObjects>().SetComponents();
            }

            yield return new WaitForSeconds(0);
        }

        if (newGameLoading != null)
        {
            newGameLoading.IncreseGridCheck();
        }
    }

    public void CheckGridForObjects(NewGameLoadingHandler newGameLoading)
    {
        StartCoroutine(WaitToCheck(newGameLoading));
    }

    public void SpawnEnemy()
    {
        if (dayTimer.CanSpawnEnemy() == true)
        {
            foreach (SpawnEnemyInArea spawnLocation in spawnLocations)
            {
                spawnLocation.SpawnEnemy();
            }
        }
    }

    public void ChangeLocation()
    {
        AIPathFinding[] enemy = gameObject.GetComponentsInChildren<AIPathFinding>();

        foreach (AIPathFinding aIPathFinding in enemy)
        {
            Destroy(aIPathFinding.gameObject);
        }

        gameObject.SetActive(false);
    }

    private void Start()
    {
        if (test)
        {
            StartCoroutine(WaitToCheck(null));
        }
    }
}
