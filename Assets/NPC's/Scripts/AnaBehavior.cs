using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnaBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> anaBehavior;

    private NpcHouseSweepingHandle npcSweeping;

    protected override void Awake()
    {
        npcSweeping = GetComponent<NpcHouseSweepingHandle>();

        base.Awake();
    }

    public override void StartBehavior()
    {
        base.CheckForBehavior(anaBehavior);
    }

    public override void ArrivedAtLocation(WaypointData waypoint = null)
    {
        if(working)
        {
            npcSweeping.ArrivedAtLocation();
        }
        else
        {
            base.ArrivedAtLocation(waypoint);
        }
    }
}
