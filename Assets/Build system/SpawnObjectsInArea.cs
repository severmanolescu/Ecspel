using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectsInArea : MonoBehaviour
{
    [SerializeField] private int dayToSpawnNew = 3;

    [SerializeField] private List<GameObject> spawnObjects = new List<GameObject>();

    [SerializeField] private Transform spawnTransformLocation;

    public List<Transform> spawnTransforms = new List<Transform>();

    private LocationGridSave locationGrid;

    private void Awake()
    {
        locationGrid = GetComponent<LocationGridSave>();

        Transform[] position = spawnTransformLocation.GetComponentsInChildren<Transform>();

        for (int i = 1; i < position.Length; i++)
        {
            spawnTransforms.Add(position[i]);
        }
    }

    public void DayChange(int day)
    {
        if (day % dayToSpawnNew == 0)
        {
            foreach (Transform spawnLocation in spawnTransforms)
            {
                int objectIndex = Random.Range(0, spawnObjects.Count - 1);

                GridNode gridNode = locationGrid.Grid.GetGridObject(spawnLocation.position);

                if (gridNode != null && gridNode.canPlace == true)
                {
                    GameObject spawnGameObject = Instantiate(spawnObjects[objectIndex], spawnLocation.transform.position, spawnLocation.transform.rotation);

                    spawnGameObject.GetComponent<PositionInGrid>().LocationGrid = locationGrid;
                }
            }
        }
    }
}
