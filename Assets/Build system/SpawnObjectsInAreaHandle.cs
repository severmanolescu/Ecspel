using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectsInAreaHandle : MonoBehaviour
{
    private List<SpawnObjectsArea> spawnObjectsInAreaList = new List<SpawnObjectsArea>();
    private List<SpawnCollectableInArea> spawnCollectableInAreaList = new List<SpawnCollectableInArea>();

    private void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Area");

        foreach (GameObject gameObject in gameObjects)
        {
            SpawnObjectsArea spawnObjectsInArea = gameObject.GetComponent<SpawnObjectsArea>();

            if (spawnObjectsInArea != null)
            {
                spawnObjectsInAreaList.Add(spawnObjectsInArea);
            }
        }

        gameObjects = GameObject.FindGameObjectsWithTag("AreaCollectable");

        foreach (GameObject gameObject in gameObjects)
        {
            SpawnCollectableInArea spawnObjectsInArea = gameObject.GetComponent<SpawnCollectableInArea>();

            if (spawnObjectsInArea != null)
            {
                spawnCollectableInAreaList.Add(spawnObjectsInArea);
            }
        }
    }

    public void DayChange(int day)
    {
        foreach (SpawnObjectsArea spawn in spawnObjectsInAreaList)
        {
            StartCoroutine(spawn.DayChange(day));
        }

        foreach (SpawnCollectableInArea spawn in spawnCollectableInAreaList)
        {
            StartCoroutine(spawn.DayChange(day));
        }
    }

    public void SpawnObjectsAtNewGame()
    {
        foreach (SpawnObjectsArea spawn in spawnObjectsInAreaList)
        {
            StartCoroutine(spawn.SpawnObjectAtLoad());
        }

        foreach (SpawnCollectableInArea spawn in spawnCollectableInAreaList)
        {
            spawn.SpawnObjectAtLoad();
        }
    }
}
