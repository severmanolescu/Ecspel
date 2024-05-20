using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VladimirBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> vladimirBehavior;

    private MinerAI minerAI;

    protected override void Awake()
    {
        minerAI = GetComponent<MinerAI>();

        base.Awake();
    }

    public override void StartBehavior()
    {
        if (!working)
        {
            base.CheckForBehavior(vladimirBehavior);
        }
    }

    protected override void CheckActionWaypoint(WaypointData waypoint)
    {
        MinerLocation minerLocation = waypoint.GetComponent<MinerLocation>();

        if(minerLocation != null)
        {
            working = true;

            minerAI.StartWorking(minerLocation);
        }
        else
        {
            base.CheckActionWaypoint(waypoint);
        }
    }

    public override void ArrivedAtLocation(WaypointData waypoint = null)
    {
        if (working == true)
        {
            minerAI.ArrivedAtLocation(waypoint);
        }
        else
        {
            base.ArrivedAtLocation(waypoint);
        }
    }
}
