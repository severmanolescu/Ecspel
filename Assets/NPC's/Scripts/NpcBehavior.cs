using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class NpcBehavior : MonoBehaviour
{
    protected NpcAIHandler aiHandler;

    protected NpcPathFinding pathFinding;

    protected DialogueDisplay dialogueDisplay;

    protected int indexOfBehavior = 0;

    public bool working = false;

    protected virtual void Awake()
    {
        aiHandler = GetComponent<NpcAIHandler>();
        pathFinding = GetComponent<NpcPathFinding>();
        dialogueDisplay = GetComponent<DialogueDisplay>();
    }

    public virtual void StartBehavior()
    {
        
    }

    virtual public void GoToNextBehaviour()
    {
        indexOfBehavior++;

        StartBehavior();
    }

    public virtual void ArrivedAtLocation(WaypointData waypoint = null)
    {
        if(waypoint == null)
        {
            GoToNextBehaviour();
        }
        else
        {
            CheckActionWaypoint(waypoint);
        }
    }

    private void CheckForBuilding(WaypointData waypoint)
    {
        NpcConstructionAI npcConstructionAI = GetComponent<NpcConstructionAI>();

        if (npcConstructionAI != null)
        {
            aiHandler.Construction = true;

            npcConstructionAI.StartToConstruct(waypoint.Construction);
        }
    }

    private void CheckForSweeping(WaypointData waypoint)
    {
        NpcHouseSweepingHandle npcHouseSweeping = GetComponent<NpcHouseSweepingHandle>();

        if (npcHouseSweeping != null)
        {
            aiHandler.Sweeping = true;

            npcHouseSweeping.StartSweeping(waypoint.HouseSweeping);
        }
    }

    private void CheckForBlacksmith()
    {
        BlacksmithHandler blacksmithHandler = GetComponent<BlacksmithHandler>();

        if (blacksmithHandler != null)
        {
            aiHandler.Blacksmith = true;

            blacksmithHandler.StartBlacksmith();
        }
    }

    private void CheckActionWaypoint(WaypointData waypoint)
    {
        if (waypoint != null)
        {
            StopAllCoroutines();

            if (waypoint.Blacksmith)
            {
                CheckForBlacksmith();
            }
            else if (waypoint.Disapear > 0)
            {
                StartCoroutine(WaitForWaypointDisapear(waypoint));
            }
            else if (waypoint.Construction != null)
            {
                CheckForBuilding(waypoint);
            }
            else if (waypoint.TimeToWait > 0)
            {
                StartCoroutine(WaitForWait(waypoint));
            }
            else if (waypoint.HouseSweeping != null)
            {
                CheckForSweeping(waypoint);
            }
            else
            {
                CheckForOtherActions(waypoint);
            }
        }
    }

    private void CheckForOtherActions(WaypointData waypoint)
    {
        TeleportNPC teleport = waypoint.GetComponent<TeleportNPC>();

        if (teleport != null)
        {
            teleport.TeleportObject(this);
        }
        else
        {
            GoToNextBehaviour();
        }
    }

    private bool CheckTheState(List<Beharior> behaviour)
    {
        if (aiHandler != null && behaviour[indexOfBehavior].Waypoints.Count > 0)
        {
            return true;
        }

        return false;
    }

    private void GoToBehaviorHandle(List<Beharior> behaviour, bool direct = false)
    {
        if (CheckTheState(behaviour))
        {
            aiHandler.MoveToWaypoint(behaviour[indexOfBehavior].Waypoints[0], true, direct);
        }
    }

    private void RandomDirectBehaviorHandle(List<Beharior> behaviour)
    {
        if (CheckTheState(behaviour))
        {
            int randomIndex = UnityEngine.Random.Range(0, behaviour[indexOfBehavior].Waypoints.Count - 1);

            aiHandler.MoveToWaypoint(behaviour[indexOfBehavior].Waypoints[randomIndex], true, true);
        }
    }

    protected void CheckForBehavior(List<Beharior> behaviour)
    {
        if (indexOfBehavior >= 0 && indexOfBehavior < behaviour.Count)
        {
            if (behaviour[indexOfBehavior] != null)
            {
                switch (behaviour[indexOfBehavior].Behaviors)
                {
                    case TypesOfBehavior.GoTo: GoToBehaviorHandle(behaviour); break;
                    case TypesOfBehavior.GoToDirect: GoToBehaviorHandle(behaviour, true); break;
                    case TypesOfBehavior.RandomDirect: RandomDirectBehaviorHandle(behaviour); break;
                }
            }
        }
    }

    private void ShowRandomMonolog(WaypointData waypointData)
    {
        if(waypointData.Monolog != null && waypointData.Monolog.Count > 0)
        {
            int randomMonolog = UnityEngine.Random.Range(0, waypointData.Monolog.Count - 1);

            dialogueDisplay.StartShowDialogue(waypointData.Monolog[randomMonolog].DialogueRespons[0]);
            dialogueDisplay.ShowAllText();
        }
    }

    private void HideMonolog()
    {
        dialogueDisplay.HideText(false);

        dialogueDisplay.ResetCanvas();
    }

    private IEnumerator WaitForWait(WaypointData waypoint)
    {
        pathFinding.MoveIdleAnimation(waypoint.DirectionToWait);

        yield return new WaitForSeconds(waypoint.TimeToWait / 2);

        ShowRandomMonolog(waypoint);

        yield return new WaitForSeconds(waypoint.TimeToWait / 2);

        HideMonolog();

        StartBehavior();
    }

    private IEnumerator WaitForWaypointDisapear(WaypointData waypoint)
    {
        if (waypoint.Disapear > 0)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        yield return new WaitForSeconds(waypoint.Disapear);

        if (waypoint.Disapear > 0)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }

        if (waypoint.StartAnimation.CompareTo(string.Empty) != 0)
        {
            GetComponent<Animator>().SetBool(waypoint.StartAnimation, true);

            if (waypoint.GoToWaypoint != null)
            {
                aiHandler.MoveToWaypoint(waypoint.GoToWaypoint, true);
            }
        }
    }
}

[Serializable]
public class Beharior
{
    [SerializeField] private TypesOfBehavior behaviors;

    [SerializeField] private  List<WaypointData> waypoints;

    public TypesOfBehavior Behaviors { get => behaviors; }
    public List<WaypointData> Waypoints { get => waypoints; }
}

[Serializable]
public enum TypesOfBehavior
{
    GoTo,
    GoToDirect,
    RandomDirect,
    Stay
}

