using System.Collections.Generic;
using UnityEngine;

public class NpcAIHandler : MonoBehaviour
{
    private NpcPathFinding npcPath;

    [SerializeField] private WaypointData currentWaypoint;
    private WaypointData stopWaypoint;

    private List<WaypointData> waypoints = new List<WaypointData>();

    private NpcBehavior npcBehavior;

    private NPCWaypointPath waypointPath;
    
    private int waypointIndex = 0;

    private bool blacksmith = false;
    private bool construction = false;
    private bool sweeping = false;

    public NpcPathFinding NpcPath { get => npcPath; set => npcPath = value; }
    public WaypointData CurrentWaypoint { get => currentWaypoint; }

    public bool Blacksmith { set => blacksmith = value; }
    public bool Construction { set => construction = value; }
    public bool Sweeping { set => sweeping = value; }

    private void Awake()
    {
        npcPath = GetComponent<NpcPathFinding>();

        waypointPath = GameObject.Find("Global").GetComponent<NPCWaypointPath>();
    }

    public void Start()
    {
        npcBehavior = GetComponent<NpcBehavior>();

        if (npcBehavior != null)
        {
            npcBehavior.StartBehavior();
        }
    }

    public void GetNpcPath()
    {
        npcPath = GetComponent<NpcPathFinding>();
    }

    public void ArrivedAtLocation()
    {
        ++waypointIndex;

        if(waypointIndex < waypoints.Count && waypoints[waypointIndex] != null)
        {
            npcPath.ChangeLocation(waypoints[waypointIndex].transform.position);
            
            if(waypointIndex < waypoints.Count)
            {
                currentWaypoint = waypoints[waypointIndex];
            }
        }
        else
        {
            CheckForHandlers();
        }
    }

    private void CheckForHandlers()
    {
        if (blacksmith)
        {
            GetComponent<BlacksmithHandler>().ArrivedAtLocation();
        }
        else if (construction)
        {
            GetComponent<NpcConstructionAI>().ArivedAtLocation();
        }
        else if(npcBehavior != null)
        {
            npcBehavior.ArrivedAtLocation(currentWaypoint);
        }  
    }

    private void RotateList()
    {
        List<WaypointData> auxWaypoints = new List<WaypointData>(waypoints);

        for(int count = waypoints.Count - 1; count >= 0; count--)
        {
            waypoints[waypoints.Count - 1 - count] = auxWaypoints[count];
        }
    }

    private void WalkDirectToWaypoint(WaypointData waypoint)
    {
        waypoints.Clear();

        waypoints.Add(waypoint);

        ArrivedAtLocation();
    }

    public void MoveToWaypoint(WaypointData waypoint, bool removeFirstWaypoint, bool direct = false)
    {
        if(waypoint == null)
        {
            return;
        }

        blacksmith = false;
        construction = false;

        waypointIndex = -1;

        if (currentWaypoint == null || direct)
        {
            WalkDirectToWaypoint(waypoint);

            return;
        }

        stopWaypoint = waypoint;

        waypoints = waypointPath.FindTheWay(currentWaypoint, stopWaypoint);

        if(waypoints.Count == 0)
        {
            WalkDirectToWaypoint(stopWaypoint);

            return;
        }
        else
        {
            RotateList();

            if (removeFirstWaypoint)
            {
                waypoints.RemoveAt(0);
            }

            ArrivedAtLocation();
        }
       
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
