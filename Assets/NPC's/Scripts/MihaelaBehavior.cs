using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MihaelaBehavior : NpcBehavior
{
    [SerializeField] private List<Beharior> mihaelaBehavior;

    [Range(0, 150)]
    [SerializeField] private float timeBetweenChecks;

    [Range(0, 100)]
    [SerializeField] private int chanceToGetUp = 10;

    [Range(0, 100)]
    [SerializeField] private int chanceToSleep = 30;

    [Range(0, 150)]
    [SerializeField] private int sleepDuration = 120;

    bool justPut = false;

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
        pathFinding.ChangeIdleAnimation(Direction.Down);

        pathFinding.CanWalk = false;

        animator.SetBool("Stay", true);

        justPut = true;

        StartCoroutine(WaitForChecks());
    } 

    private IEnumerator WaitForChecks()
    {
        while(true)
        {
            yield return new WaitForSeconds(timeBetweenChecks);

            if(!justPut)
            {
                int chanceToChange = Random.Range(0, 100);

                if(chanceToChange < chanceToGetUp) 
                {
                    animator.SetBool("Stay", false);

                    base.GoToNextBehaviour();
                }
                else
                {
                    chanceToChange = Random.Range(0, 100);

                    if(chanceToChange < chanceToSleep)
                    {
                        dialogueDisplay.CanTalk = false;

                        animator.SetBool("Sleep", true);

                        yield return new WaitForSeconds(sleepDuration);

                        animator.SetBool("Sleep", false);

                        dialogueDisplay.CanTalk = true;
                    }
                }
            }
            else
            {
                justPut = false;
            }
        }
    }

    public void CanWalkAgain()
    {
        pathFinding.CanWalk = true;
    }
}
