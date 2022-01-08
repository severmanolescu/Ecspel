using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameLoadingHandler : MonoBehaviour
{
    [SerializeField] private int noOfGrid;

    private int gridCheck = 0;

    public void IncreseGridCheck()
    {
        gridCheck++;
    }

    public void StartAllGridLocationCheckObjects()
    {
        GameObject[] locationGrid = GameObject.FindGameObjectsWithTag("Location");

        foreach (GameObject location in locationGrid)
        {
            LocationGridSave locationGridSave = location.GetComponent<LocationGridSave>();

            if(locationGridSave != null)
            {
                locationGridSave.CheckGridForObjects(this);
            }
        }
    }

    public void SpawnObjectsInAreas()
    {
        GameObject[] areaSpawn = GameObject.FindGameObjectsWithTag("Area");

        foreach (GameObject area in areaSpawn)
        {
            SpawnObjectsArea spawnObjects = area.GetComponent<SpawnObjectsArea>();

            if (spawnObjects != null)
            {
                spawnObjects.SpawnObjectAtLoad();
            }
        }
    }

    IEnumerator WaitForAllLocations(LoadSceneHandler loadSceneHandler)
    {
        SpawnObjectsInAreas();

        yield return new WaitForSeconds(2);

        while(gridCheck < noOfGrid)
        {
            yield return null;
        }

        loadSceneHandler.FinishGridSearchProcess = true;

        Destroy(gameObject);
    }

    public void StartNewGame(LoadSceneHandler loadSceneHandler)
    {
        StartAllGridLocationCheckObjects();

        StartCoroutine(WaitForAllLocations(loadSceneHandler));
    }

    IEnumerator WaitForAllLocations1()
    {
        SpawnObjectsInAreas();

        yield return new WaitForSeconds(2);

        while (gridCheck < noOfGrid)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    private void Start()
    {
        StartAllGridLocationCheckObjects();

        StartCoroutine(WaitForAllLocations1());
    }
}
