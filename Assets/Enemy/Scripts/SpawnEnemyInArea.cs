using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyInArea : MonoBehaviour
{
    [SerializeField] private  List<GameObject> listOfEnemy = new List<GameObject>();

    [SerializeField] private int minSpawnEnemyNo;
    [SerializeField] private int maxSpawnEnemyNo;

    [SerializeField] private bool setParent = false;

    private LocationGridSave locationGrid;

    public LocationGridSave LocationGrid { get => locationGrid; set => locationGrid = value; }

    private void Awake()
    {
        LocationGrid = GetComponentInParent<LocationGridSave>();
    }

    private bool VerifySpawnLcoationInGrid(Vector3 position)
    {
        GridNode grid = LocationGrid.Grid.GetGridObject(position);

        if (grid != null)
        {
            return grid.isWalkable;
        }
        else
        {
            return false;
        }
    }

    public void SpawnEnemy(LocationGridSave locationGrid = null)
    {
        int noOfEnemy = Random.Range(minSpawnEnemyNo, maxSpawnEnemyNo);

        if(locationGrid != null)
        {
            this.locationGrid = locationGrid;
        }

        if (listOfEnemy != null && listOfEnemy.Count > 0)
        {
            for (int indexOfEnemy = 0; indexOfEnemy < noOfEnemy; indexOfEnemy++)
            {
                Vector3 spawnPosition = new Vector3(transform.position.x + Random.Range(-transform.localScale.x / 2, transform.localScale.x / 2),
                                                    transform.position.y + Random.Range(-transform.localScale.y / 2, transform.localScale.y / 2),
                                                    0);

                if (VerifySpawnLcoationInGrid(spawnPosition))
                {
                    AIPathFinding spawnEnemy = Instantiate(listOfEnemy[Random.Range(0, listOfEnemy.Count)]).GetComponent<AIPathFinding>();

                    spawnEnemy.transform.position = spawnPosition;

                    spawnEnemy.LocationGrid = LocationGrid;

                    if (setParent == false)
                    {
                        spawnEnemy.transform.SetParent(LocationGrid.transform);
                    }
                    else
                    {
                        spawnEnemy.transform.SetParent(transform.parent);
                    }
                }
            }
        }
    }
}
