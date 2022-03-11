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
        GameObject.Find("Global/DayTimer").GetComponent<SpawnObjectsInAreaHandle>().SpawnObjectsAtNewGame();
    }

    IEnumerator WaitForAllLocations(LoadSceneHandler loadSceneHandler)
    {
        SpawnObjectsInAreas();

        yield return new WaitForSeconds(3);

        StartAllGridLocationCheckObjects();

        while (gridCheck < noOfGrid)
        {
            yield return null;
        }

        loadSceneHandler.FinishGridSearchProcess = true;

        Destroy(gameObject);
    }

    public void StartNewGame(LoadSceneHandler loadSceneHandler)
    {
        StartCoroutine(WaitForAllLocations(loadSceneHandler));
    }
}
