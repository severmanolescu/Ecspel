using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCWaypointPath : MonoBehaviour
{
    private List<WaypointData> waypointPath = new List<WaypointData>();

    public List<WaypointData> FindTheWay(WaypointData fromLocation, WaypointData toLocation)
    {
        if(fromLocation.NextWaypoints == null ||
           fromLocation.NextWaypoints.Count == 0 ||
           toLocation.NextWaypoints == null ||
           toLocation.NextWaypoints.Count == 0)
        {
            return null;
        }

        waypointPath = new List<WaypointData>();

        CheckTheWaypoint(fromLocation, fromLocation, toLocation);

        return waypointPath;
    }

    public bool CheckTheWaypoint(WaypointData previousWaypoint, WaypointData fromWaypoint, WaypointData toWaypoint)
    {
        if(fromWaypoint == toWaypoint)
        {
            waypointPath.Add(fromWaypoint);

            return true;
        }

        foreach(WaypointData waypoint in fromWaypoint.NextWaypoints)
        {
            if(waypoint != previousWaypoint && CheckTheWaypoint(fromWaypoint, waypoint, toWaypoint))
            {
                waypointPath.Add(fromWaypoint);

                return true;
            }
        }

        return false;
    }
}
