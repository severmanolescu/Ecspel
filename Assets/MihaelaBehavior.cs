using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MihaelaBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> mihaelaBehavior;

    public override void StartBehavior()
    {
         base.CheckForBehavior(mihaelaBehavior);
    }

    public override void ArrivedAtLocation(WaypointData waypoint = null)
    {
        base.ArrivedAtLocation(waypoint);
    }
}
