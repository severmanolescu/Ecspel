using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MihaelaBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> mihaelaBehavior;

    [Range(60, 150)]
    [SerializeField] private float timeBetweenChecks;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        base.Awake();
    }

    public override void StartBehavior()
    {
         base.CheckForBehavior(mihaelaBehavior);
    }

    public override void ArrivedAtLocation(WaypointData waypoint = null)
    {
        if(waypoint != null && waypoint.StartAnimation.CompareTo(string.Empty) != 0)
        {
            StartStay();
        }
        else
        {
            base.ArrivedAtLocation(waypoint);
        }
    }

    public override void GoToNextBehaviour()
    {
        base.CheckForBehavior(mihaelaBehavior);
    }

    private void StartStay()
    {
        pathFinding.MoveIdleAnimation(Direction.Down);

        animator.SetBool("Stay", true);
    } 
}
