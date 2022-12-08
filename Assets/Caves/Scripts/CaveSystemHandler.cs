using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveSystemHandler : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;
    [SerializeField] private Transform spawnLocationOutside;

    [SerializeField] private List<GameObject> cavesPrefabs;

    [SerializeField] private GameObject newCamera;

    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;

    [SerializeField] private LocationGridSave newGrid;

    private Transform playerTransform;

    private GameObject lastCave = null;

    private LocationGridSave locationGrid;

    private CaveIndexSelect caveIndexSelect;

    private int caveIndex;
    private int maxCaveIndex;

    private GameObject caveArea;

    public LocationGridSave LocationGrid { get { return caveArea.GetComponent<LocationGridSave>(); } }

    public int MaxCaveIndex { get => maxCaveIndex; set => maxCaveIndex = value; }

    private void Awake()
    {
        locationGrid = GetComponent<LocationGridSave>();

        caveIndex = 0;

        playerTransform = GameObject.Find("Global/Player").transform;
        caveIndexSelect = GameObject.Find("Global/Player/Canvas/CaveSelect").GetComponent<CaveIndexSelect>();

        caveArea = GameObject.Find("CaveArea");
    }

    IEnumerator WaitToCheck()
    {
        for (int indexCellX = 0; indexCellX < locationGrid.Grid.gridArray.GetLength(0); indexCellX++)
        {
            for (int indexCellY = 0; indexCellY < locationGrid.Grid.gridArray.GetLength(1); indexCellY++)
            {
                GameObject testObjectInGridCell = new GameObject("TestGridCells");

                testObjectInGridCell.AddComponent<ChangeGridCellValuesByObjects>().Grid = locationGrid.Grid;

                Vector3 position = locationGrid.Grid.GetWorldPosition(locationGrid.Grid.gridArray[indexCellX, indexCellY].x, locationGrid.Grid.gridArray[indexCellX, indexCellY].y);

                position.x += locationGrid.Grid.CellSize / 2f;
                position.y += locationGrid.Grid.CellSize / 2f;

                testObjectInGridCell.transform.position = position;

                testObjectInGridCell.GetComponent<ChangeGridCellValuesByObjects>().SetComponents();
            }

            yield return new WaitForSeconds(0.05f);
        }
    }

    private void ReinitializeGrid()
    {
        if (locationGrid != null)
        {
            StopAllCoroutines();

            StartCoroutine(WaitToCheck());
        }
    }

    public void TeleportToCaves()
    {
        if (maxCaveIndex >= 4)
        {
            caveIndexSelect.SpawnButtons(maxCaveIndex, this);

            caveIndexSelect.gameObject.SetActive(true);
        }
        else
        {
            caveIndex = 0;

            for (int indexOfX = 0; indexOfX < locationGrid.Grid.gridArray.GetLength(0); indexOfX++)
            {
                for (int indexOfY = 0; indexOfY < locationGrid.Grid.gridArray.GetLength(1); indexOfY++)
                {
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].isWalkable = true;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].canPlace = true;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].canPlant = false;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].cropPlaced = false;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].objectInSpace = null;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].crop = null;
                }
            }

            Teleport();

            ReinitializeGrid();

            caveArea.SetActive(false);

            if (lastCave != null)
            {
                Destroy(lastCave);

                AIPathFinding[] enemys = GetComponentsInChildren<AIPathFinding>();

                foreach (AIPathFinding enemy in enemys)
                {
                    Destroy(enemy.gameObject);
                }
            }

            if (caveIndex < cavesPrefabs.Count)
            {
                lastCave = Instantiate(cavesPrefabs[caveIndex]);

                lastCave.transform.SetParent(transform);

                lastCave.transform.position = spawnLocation.position;

                lastCave.GetComponent<SpawnEnemys>().SpawnEnemy(locationGrid);

                playerTransform.position = lastCave.GetComponent<SpawnEnemys>().SpawnLocation.position;

                if (maxCaveIndex > caveIndex)
                {
                    Debug.Log("Dasdasd");

                    lastCave.GetComponent<DeleteCaveChestItems>().DeleteAllStorage();
                }
            }
        }
    }

    public void Teleport()
    {
        newCamera.SetActive(true);

        foreach (GameObject gameObject in objectsToSetActiveToFalse)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in objectsToSetActiveToTrue)
        {
            gameObject.SetActive(true);
        }

        GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().LocationGrid = newGrid;
        GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>().StopSoundEffects();
    }

    public void TeleportToNextCave()
    {
        caveIndex++;

        for (int indexOfX = 0; indexOfX < locationGrid.Grid.gridArray.GetLength(0); indexOfX++)
        {
            for (int indexOfY = 0; indexOfY < locationGrid.Grid.gridArray.GetLength(1); indexOfY++)
            {
                locationGrid.Grid.gridArray[indexOfX, indexOfY].isWalkable = true;
                locationGrid.Grid.gridArray[indexOfX, indexOfY].canPlace = true;
                locationGrid.Grid.gridArray[indexOfX, indexOfY].canPlant = false;
                locationGrid.Grid.gridArray[indexOfX, indexOfY].cropPlaced = false;
                locationGrid.Grid.gridArray[indexOfX, indexOfY].objectInSpace = null;
                locationGrid.Grid.gridArray[indexOfX, indexOfY].crop = null;
            }
        }

        ReinitializeGrid();

        if (lastCave != null)
        {
            Destroy(lastCave);

            AIPathFinding[] enemys = GetComponentsInChildren<AIPathFinding>();

            foreach (AIPathFinding enemy in enemys)
            {
                Destroy(enemy.gameObject);
            }
        }

        if (caveIndex < cavesPrefabs.Count)
        {
            lastCave = Instantiate(cavesPrefabs[caveIndex]);

            lastCave.transform.SetParent(transform);

            lastCave.transform.position = spawnLocation.position;

            lastCave.GetComponent<SpawnEnemys>().SpawnEnemy(locationGrid);

            playerTransform.position = lastCave.GetComponent<SpawnEnemys>().SpawnLocation.position;

            if (maxCaveIndex > caveIndex)
            {
                lastCave.GetComponent<DeleteCaveChestItems>().DeleteAllStorage();
            }
        }

        if (caveIndex >= maxCaveIndex)
        {
            maxCaveIndex = caveIndex;
        }
    }

    public void TeleportOutside()
    {
        playerTransform.position = spawnLocationOutside.position;

        if (lastCave != null)
        {
            Destroy(lastCave);
        }

        caveArea.SetActive(true);
    }

    public void TeleportToCaveWithIndex(int indexOfCave)
    {
        if (indexOfCave >= 0 && indexOfCave < cavesPrefabs.Count)
        {
            caveIndex = indexOfCave;

            for (int indexOfX = 0; indexOfX < locationGrid.Grid.gridArray.GetLength(0); indexOfX++)
            {
                for (int indexOfY = 0; indexOfY < locationGrid.Grid.gridArray.GetLength(1); indexOfY++)
                {
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].isWalkable = true;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].canPlace = true;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].canPlant = false;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].cropPlaced = false;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].objectInSpace = null;
                    locationGrid.Grid.gridArray[indexOfX, indexOfY].crop = null;
                }
            }

            Teleport();

            ReinitializeGrid();

            caveArea.SetActive(false);

            if (lastCave != null)
            {
                Destroy(lastCave);

                AIPathFinding[] enemys = GetComponentsInChildren<AIPathFinding>();

                foreach (AIPathFinding enemy in enemys)
                {
                    Destroy(enemy.gameObject);
                }
            }

            if (caveIndex < cavesPrefabs.Count)
            {
                lastCave = Instantiate(cavesPrefabs[caveIndex]);

                lastCave.transform.SetParent(transform);

                lastCave.transform.position = spawnLocation.position;

                lastCave.GetComponent<SpawnEnemys>().SpawnEnemy(locationGrid);

                playerTransform.position = lastCave.GetComponent<SpawnEnemys>().SpawnLocation.position;

                if (maxCaveIndex >= caveIndex)
                {
                    lastCave.GetComponent<DeleteCaveChestItems>().DeleteAllStorage();
                }
            }
        }
    }

    public bool HaveNextLevel()
    {
        if (caveIndex < cavesPrefabs.Count - 1)
        {
            return true;
        }

        return false;
    }
}
