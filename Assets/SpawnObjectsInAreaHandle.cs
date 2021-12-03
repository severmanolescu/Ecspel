using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectsInAreaHandle : MonoBehaviour
{
    private List<SpawnObjectsArea> spawnObjectsInAreaList = new List<SpawnObjectsArea>();

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
    }

    public void DayChange(int day)
    {
        foreach (SpawnObjectsArea spawn in spawnObjectsInAreaList)
        {
            spawn.DayChange(day);
        }
    }
}
