using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoriaBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> victoriaBehavior;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void StartBehavior()
    {
        base.CheckForBehavior(victoriaBehavior);
    }

    public override void ArrivedAtLocation(WaypointData waypoint = null)
    {
        base.ArrivedAtLocation(waypoint);
    }
}
