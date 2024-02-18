using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> bobBehavior;

    private NpcConstructionAI constructionAI;

    protected override void Awake()
    {
        constructionAI = GetComponent<NpcConstructionAI>();

        base.Awake();
    }

    public override void StartBehavior()
    {
        base.CheckForBehavior(bobBehavior);
    }

    public override void ArrivedAtLocation(WaypointData waypoint = null)
    {
        if(working)
        {
            constructionAI.ArivedAtLocation();
        }
        else
        {
            base.ArrivedAtLocation(waypoint);
        }

    }
}
