using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyInArea : MonoBehaviour
{
    [SerializeField] private  List<GameObject> listOfEnemy = new List<GameObject>();

    [SerializeField] private int minSpawnEnemyNo;
    [SerializeField] private int maxSpawnEnemyNo;

    private LocationGridSave locationGrid;

    private void Awake()
    {
        locationGrid = GetComponentInParent<LocationGridSave>();
    }

    private bool VerifySpawnLcoationInGrid(Vector3 position)
    {
        GridNode grid = locationGrid.Grid.GetGridObject(position);

        if (grid != null)
        {
            return grid.isWalkable;
        }
        else
        {
            return false;
        }
    }

    public void SpawnEnemy()
    {
        int noOfEnemy = Random.Range(minSpawnEnemyNo, maxSpawnEnemyNo);

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

                    spawnEnemy.transform.localPosition = spawnPosition;

                    spawnEnemy.LocationGrid = locationGrid;

                    spawnEnemy.transform.SetParent(locationGrid.transform);
                }
            }
        }
    }
}
