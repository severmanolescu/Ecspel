using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndreiBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> andreiBehavior;

    private BuilderHelperAI helperAI;

    protected override void Awake()
    {
        helperAI = GetComponent<BuilderHelperAI>();

        base.Awake();
    }

    public override void StartBehavior()
    {
        if(!working)
        {
            base.CheckForBehavior(andreiBehavior);
        }
    }

    public override void ArrivedAtLocation(WaypointData waypoint = null)
    {
        if (working == true)
        {
            helperAI.ArivedAtLocation();
        }
        else
        {
            base.ArrivedAtLocation(waypoint);
        }
    }
}
