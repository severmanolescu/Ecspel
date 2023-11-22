using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAIHandler : MonoBehaviour
{
    private NpcPathFinding npcPath;

    private WaypointData currentWaypoint;
    public WaypointData stopWaypoint;

    public bool moveToWaypoint = false;

    public List<WaypointData> waypoints = new List<WaypointData>();

    public int waypointIndex = 0;

    private NPCWaypointPath waypointPath;

    private bool blacksmith = false;

    public NpcPathFinding NpcPath { get => npcPath; set => npcPath = value; }
    public WaypointData CurrentWaypoint { get => currentWaypoint; }

    private void Awake()
    {
        npcPath = GetComponent<NpcPathFinding>();

        waypointPath = GetComponent<NPCWaypointPath>();

        StartCoroutine(Wait());
    }

    public void GetNpcPath()
    {
        npcPath = GetComponent<NpcPathFinding>();
    }

    public void ArrivedAtLocation()
    {
        waypointIndex++;

        if(waypointIndex < waypoints.Count)
        {
            npcPath.ChangeLocation(waypoints[waypointIndex].transform.position);
            
            currentWaypoint = waypoints[waypointIndex];
        }
        else
        {
            if(blacksmith)
            {
                GetComponent<BlacksmithHandler>().ArrivedAtLocation();
            }
            else
            {
                CheckForAction();
            }
        }
    }

    private void CheckForAction()
    {
        WaypointData waypointData = currentWaypoint.GetComponent<WaypointData>();

        if (waypointData != null)
        {
            if(waypointData.Blacksmith)
            {
                BlacksmithHandler blacksmithHandler = GetComponent<BlacksmithHandler>();

                if(blacksmithHandler != null)
                {
                    blacksmith = true;

                    blacksmithHandler.StartBlacksmith();
                }
            }
        }
    }

    private void RotateList()
    {
        List<WaypointData> auxWaypoints = new List<WaypointData>(waypoints);

        for(int count = waypoints.Count - 1; count >= 0; count--)
        {
            waypoints[waypoints.Count - 1 - count] = auxWaypoints[count];
        }

        waypoints.RemoveAt(0);
    }

    private IEnumerator Wait()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);

            if (moveToWaypoint)
            {
                waypointIndex = -1;

                waypoints = waypointPath.FindTheWay(currentWaypoint, stopWaypoint);

                RotateList();

                ArrivedAtLocation();

                moveToWaypoint = false;
            }
        }
    }

    public void MoveToWaypoint()
    {

    }

    public void StartTalking()
    {
        if(blacksmith)
        {
            GetComponent<BlacksmithHandler>().StartTalking();
        }
    }

    public void StopTalking()
    {
        if (blacksmith)
        {
            GetComponent<BlacksmithHandler>().StopTalking();
        }
    }
}
