using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLittleEnemy : MonoBehaviour
{
    [SerializeField] private int minAmountOfMinions = 3;
    [SerializeField] private int maxAmountOfMinions = 7;

    [SerializeField] private float minTimeBetweenSpawns = 3f;
    [SerializeField] private float maxTimeBetweenSpawns = 7f;

    [SerializeField] private List<GameObject> spawnPoints = new();

    [SerializeField] private GameObject EnemyPrefab;

    private List<GameObject> minions = new();

    private int amountOfMinions;

    private float timeBetweenSpawns;

    private void Awake()
    {
        amountOfMinions = Random.Range(minAmountOfMinions, maxAmountOfMinions);

        timeBetweenSpawns = Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns);

        SpawnEnemy();

        StartCoroutine(WaitBetweenSpawn());
    }

    private IEnumerator WaitBetweenSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenSpawns);

            CheckMinions();

            SpawnEnemy();
        }
    }

    private void CheckMinions()
    {
        foreach (GameObject minion in minions)
        {
            if (minion == null)
            {
                minions.Remove(minion);
            }
        }
    }

    public void SpawnEnemy()
    {
        if (spawnPoints.Count > 0)
        {
            int enemyNo = Random.Range(1, spawnPoints.Count - 1);

            for (int enemyIndex = 0; enemyIndex < enemyNo; enemyIndex++)
            {
                if (minions.Count < amountOfMinions)
                {
                    GameObject enemy = Instantiate(EnemyPrefab, spawnPoints[enemyIndex].transform.position, spawnPoints[enemyIndex].transform.rotation);

                    minions.Add(enemy);
                }
                else
                {
                    break;
                }
            }
        }
    }
}
