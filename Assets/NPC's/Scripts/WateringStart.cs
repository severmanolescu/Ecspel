using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringStart : MonoBehaviour
{
    [SerializeField] List<Transform> waterLocations = new List<Transform>();

    [SerializeField] private Transform fountainLocation;

    [SerializeField] private LocationGridSave gridSave;

    private List<GridNode> wateringNodes = new List<GridNode>();

    private int locationIndex = 0;

    public Vector3 FountainLocation { get => fountainLocation.position; }

    private void Start()
    {
        wateringNodes = new List<GridNode>();

        foreach(Transform location in waterLocations)
        {
            GridNode newNode = gridSave.Grid.GetGridObject(location.position);

            if(newNode != null && newNode.isWalkable)
            {
                wateringNodes.Add(newNode);   
            } 
        }
    }

    public GridNode GetNewLocation()
    {
        if(locationIndex < wateringNodes.Count)
        {
            return wateringNodes[locationIndex++];
        }

        return null;
    }

    public Vector3 GetCurrentPosition()
    {
        if(locationIndex - 1 >= 0 && locationIndex < wateringNodes.Count)
        {
            return waterLocations[locationIndex - 1].position;
        }

        return DefaulData.nullVector;
    }
}
