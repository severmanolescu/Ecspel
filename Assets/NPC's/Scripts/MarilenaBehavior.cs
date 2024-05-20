using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarilenaBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> marilenaBehavior;

    private WateringAI wateringAI;

    protected override void Awake()
    {
        wateringAI = GetComponent<WateringAI>();

        base.Awake();
    }

    public override void StartBehavior()
    {
        base.CheckForBehavior(marilenaBehavior);
    }

    public override void ArrivedAtLocation(WaypointData waypoint = null)
    {
        if(working)
        {
            wateringAI.ArrivedAtLocation();
        }
        else if(waypoint != null)
        {
             WateringStart wateringStart = waypoint.GetComponent<WateringStart>();

            if(wateringStart != null)
            {
                working = true;

                wateringAI.StartWorking(wateringStart);
            }
            else
            {
                base.ArrivedAtLocation(waypoint);
            }
        }
        else
        {
            base.ArrivedAtLocation(waypoint);
        }
    }
}
