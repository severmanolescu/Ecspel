using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSweeping : MonoBehaviour
{
    [Range(1, 10)]
    [SerializeField] private int maxSweepingLocation;

    [SerializeField] private GameObject broom;

    [SerializeField] private GameObject areasParent;

    [SerializeField] private LocationGridSave locationGrid;

    private int locationSweeps = -1;
    private int indexArea = 0;

    private BoxCollider2D[] areas;

    private void Awake()
    {
        areas = areasParent.GetComponentsInChildren<BoxCollider2D>();

        indexArea = Random.Range(0, areas.Length - 1);
    }

    public void DeactivateBroom()
    {
        if(broom != null)
        {
            broom.SetActive(false);
        }
    }

    public Vector3 GetBroomLocation()
    {
        locationGrid.grid.GetXY(broom.transform.position, out int x, out int y);

        Vector3 belowNodePosition = locationGrid.grid.GetWorldPosition(x, y - 1);

        GridNode belowNode = locationGrid.grid.GetGridObject(belowNodePosition);

        return locationGrid.grid.GetWorldPosition(x, y -1);
    }

    private Vector3 GetSweepLocationArea()
    {
        if(indexArea < areas.Length)
        {
            Vector3 newPosition = DefaulData.GetRandomPositionCollider(areas[indexArea], transform);

            locationGrid.grid.GetXY(newPosition, out int x, out int y);

            Vector3 belowNodePosition = locationGrid.grid.GetWorldPosition(x, y - 1);

            GridNode belowNode = locationGrid.grid.GetGridObject(belowNodePosition);

            if (belowNode != null)
            {
                if (belowNode.isWalkable)
                {
                    return locationGrid.grid.GetWorldPosition(x, y - 1);
                }
                else
                {
                    return GetSweepLocationArea();
                }
            }
            else
            {
                return GetSweepLocationArea();
            }
        }
        else
        {
            return DefaulData.nullVector;
        }
    }

    private void SetNewArea()
    {
        indexArea = Random.Range(0, areas.Length - 1);
    }

    public Vector3 GetRandomPosition()
    {
        if(areas != null)
        {
            locationSweeps++;

            if (locationSweeps > maxSweepingLocation)
            {
                locationSweeps = -1;

                SetNewArea();
            }

            return GetSweepLocationArea();
        }
        else
        {
            return DefaulData.nullVector;
        }
    }
}
