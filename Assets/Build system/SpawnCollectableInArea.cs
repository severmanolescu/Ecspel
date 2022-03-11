using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCollectableInArea : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();

    [SerializeField, Range(1, 10)] private int daysToSpawn = 1;

    [SerializeField, Range(1, 20)] private int minObjsToSpawn = 1;
    [SerializeField, Range(1, 20)] private int maxObjsToSpawn = 1;

    private LocationGridSave gridSave;

    private void Awake()
    {
        gridSave = GetComponentInParent<LocationGridSave>();
    }

    private void GetObjectScale(out bool dataFound, out int startScaleX, out int startScaleY, out int scaleX, out int scaleY, GameObject spawnObject)
    {
        dataFound = true;
        startScaleX = 0;
        startScaleY = 0;
        scaleX = 0;
        scaleY = 0;
    }

    private bool VerifyGridPosition(Vector3 position, GameObject spawnObject)
    {
        GetObjectScale(out bool dataFound, out int startScaleX, out int startScaleY, out int scaleX, out int scaleY, spawnObject);

        Grid<GridNode> grid = gridSave.Grid;

        if (dataFound == true && grid != null)
        {
            GridNode gridNode = gridSave.Grid.GetGridObject(position);

            if (gridNode != null &&
                gridNode.x + startScaleX < grid.gridArray.GetLength(0) && gridNode.x + scaleX < grid.gridArray.GetLength(0) &&
                gridNode.y + startScaleY < grid.gridArray.GetLength(1) && gridNode.y + scaleY < grid.gridArray.GetLength(1))
            {
                for (int i = gridNode.x + startScaleX; i <= gridNode.x + scaleX; i++)
                {
                    for (int j = gridNode.y + startScaleY; j <= gridNode.y + scaleY; j++)
                    {
                        if (i < gridSave.Grid.gridArray.GetLength(0) && j < gridSave.Grid.gridArray.GetLength(1))
                        {
                            if (gridSave.Grid.gridArray[i, j] != null)
                            {
                                if (gridSave.Grid.gridArray[i, j].canPlace == false ||
                                    gridSave.Grid.gridArray[i, j].isWalkable == false)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public IEnumerator DayChange(int day)
    {
        Rigidbody2D[] objectsNo = GetComponentsInChildren<Rigidbody2D>();

        foreach(Rigidbody2D obj in objectsNo)
        {
            Destroy(obj);
        }

        int noItemsToSpawn = Random.Range(minObjsToSpawn, maxObjsToSpawn);

        if (day % daysToSpawn == 0)
        {
            for (int noItem = 0; noItem < noItemsToSpawn; noItem++)
            {
                int spawnItemNo = Random.Range(0, spawnObjects.Count);

                Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                                                    transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2),
                                                    0);

                if (VerifyGridPosition(spawnPosition, spawnObjects[spawnItemNo]) == true)
                {
                    GameObject spawnObject = Instantiate(spawnObjects[spawnItemNo]);

                    spawnObject.GetComponent<PositionInGridHarvest>().LocationGrid = gridSave; 

                    spawnObject.transform.position = spawnPosition;
                    spawnObject.transform.SetParent(transform);
                }

                yield return new WaitForSeconds(.05f);
            }
        }
    }

    public void SpawnObjectAtLoad()
    {
        StartCoroutine(DayChange(daysToSpawn));
    }
}
