using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAIHandler : MonoBehaviour
{
    private NpcPathFinding npcPath;

    public WaypointData currentWaypoint;
    public WaypointData stopWaypoint;

    public bool moveToWaypoint = false;

    private List<WaypointData> waypoints = new List<WaypointData>();

    private int waypointIndex = 0;

    private NPCWaypointPath waypointPath;

    private bool blacksmith = false;
    private bool construction = false;

    public NpcPathFinding NpcPath { get => npcPath; set => npcPath = value; }
    public WaypointData CurrentWaypoint { get => currentWaypoint; }

    private void Awake()
    {
        npcPath = GetComponent<NpcPathFinding>();

        waypointPath = GameObject.Find("Global").GetComponent<NPCWaypointPath>();

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
            if(!CheckForHandlers())
            {
                CheckForAction();
            }
        }
    }

    private bool CheckForHandlers()
    {
        if (blacksmith)
        {
            GetComponent<BlacksmithHandler>().ArrivedAtLocation();

            return true;
        }
        else if (construction)
        {
            GetComponent<NpcConstructionAI>().ArivedAtLocation();

            return true;
        }

        BuilderHelperAI builderHelperAI = GetComponent<BuilderHelperAI>();

        if(builderHelperAI != null)
        {
            builderHelperAI.ArivedAtLocation();

            return true;
        }    

        return false;
    }

    private void CheckForAction()
    {
        WaypointData waypointData = currentWaypoint.GetComponent<WaypointData>();

        if (waypointData != null)
        {
            if (waypointData.Blacksmith)
            {
                BlacksmithHandler blacksmithHandler = GetComponent<BlacksmithHandler>();

                if (blacksmithHandler != null)
                {
                    blacksmith = true;

                    blacksmithHandler.StartBlacksmith();
                }
            }
            else if(waypointData.Disapear > 0)
            {
                StartCoroutine(WaitForWaypoint(waypointData, true));
            }
            else if(waypointData.Construction != null)
            {
                NpcConstructionAI npcConstructionAI = GetComponent<NpcConstructionAI>();

                if (npcConstructionAI != null)
                {
                    construction = true;

                    npcConstructionAI.StartToConstruct(waypointData.Construction);
                }
            }
        }
    }

    private IEnumerator WaitForWaypoint(WaypointData waypoint, bool disapear)
    {
        if(disapear)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        yield return new WaitForSeconds(waypoint.Disapear);

        if (disapear)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }

        if(waypoint.StartAnimation.CompareTo(string.Empty) != 0)
        {
            GetComponent<Animator>().SetBool(waypoint.StartAnimation, true);

            if (waypoint.GoToWaypoint != null)
            {
                MoveToWaypoint(waypoint.GoToWaypoint, true);
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
    }

    public void MoveToWaypoint(WaypointData waypoint, bool removeFirstWaypoint)
    {
        if(waypoint == null)
        {
            return;
        }

        blacksmith = false;

        waypointIndex = -1;

        stopWaypoint = waypoint;

        waypoints = waypointPath.FindTheWay(currentWaypoint, stopWaypoint);

        RotateList();

        if (removeFirstWaypoint)
        {
            waypoints.RemoveAt(0);
        }

        ArrivedAtLocation();

        moveToWaypoint = false;
    }

    private IEnumerator Wait()
    {
        while(true)
        {
            yield return new WaitForSeconds(1);

            if (moveToWaypoint)
            {
                MoveToWaypoint(stopWaypoint, true);
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
