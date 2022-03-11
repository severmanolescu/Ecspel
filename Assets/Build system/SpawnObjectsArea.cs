using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectsArea : MonoBehaviour
{
    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();

    [SerializeField] private int dayToSpawn;
    [SerializeField] private int noItemsToSpawn;
    [SerializeField] private int maxItems;

    private LocationGridSave gridSave;

    private void Awake()
    {
        gridSave = GetComponentInParent<LocationGridSave>();
    }

    private void GetObjectScale(out bool dataFound, out int startScaleX, out int startScaleY, out int scaleX, out int scaleY, GameObject spawnObject)
    {
        DamageTree damageTree = spawnObject.GetComponent<DamageTree>();

        if (damageTree != null)
        {
            damageTree.GetData(out startScaleX, out startScaleY, out scaleX, out scaleY);

            dataFound = true;

            return;
        }
        else
        {
            StoneDamage stoneDamage = spawnObject.GetComponent<StoneDamage>();

            if (stoneDamage != null)
            {
                stoneDamage.GetData(out startScaleX, out startScaleY, out scaleX, out scaleY);

                dataFound = true;

                return;
            }
        }

        dataFound = false;
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
        int objectsNo = GetComponentsInChildren<Rigidbody2D>().Length;

        if (day % dayToSpawn == 0 && objectsNo < maxItems)
        {
            for(int noItem = 0; noItem < noItemsToSpawn; noItem++)
            {
                if (objectsNo < maxItems)
                {
                    int spawnItemNo = Random.Range(0, spawnObjects.Count);

                    Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                                                        transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2),
                                                        0);

                    if (VerifyGridPosition(spawnPosition, spawnObjects[spawnItemNo]) == true)
                    {
                        GameObject spawnObject = Instantiate(spawnObjects[spawnItemNo]);

                        spawnObject.GetComponent<PositionInGrid>().LocationGrid = gridSave;

                        spawnObject.transform.position = spawnPosition;
                        spawnObject.transform.SetParent(transform);

                        objectsNo++;
                    }

                    yield return new WaitForSeconds(.05f);
                }
                else
                {
                    break;
                }
            }
        }
    }

    public IEnumerator SpawnObjectAtLoad()
    {
        for (int noItem = 0; noItem < maxItems; noItem++)
        {
            int spawnItemNo = Random.Range(0, spawnObjects.Count - 1);

            Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                                                transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2),
                                                0);
            if (VerifyGridPosition(spawnPosition, spawnObjects[spawnItemNo]) == true)
            {
                GameObject spawnObject = Instantiate(spawnObjects[spawnItemNo]);

                spawnObject.GetComponent<PositionInGrid>().LocationGrid = gridSave;

                spawnObject.transform.position = spawnPosition;
                spawnObject.transform.SetParent(transform);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}
