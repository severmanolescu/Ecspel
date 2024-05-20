using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcHouseSweepingHandle : MonoBehaviour
{
    [SerializeField] private int sweepingDuration = 5;

    private HouseSweeping houseSweeping;

    private NpcPathFinding npcPathFinding;

    private Animator animator;

    private DialogueDisplay dialogueDisplay;

    private bool goGetBroom = false;

    private bool sweeping = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        npcPathFinding = GetComponent<NpcPathFinding>();

        dialogueDisplay = GetComponent<DialogueDisplay>();
    }

    public void StartSweeping(HouseSweeping houseSweeping)
    {
        if(houseSweeping != null)
        {
            this.houseSweeping = houseSweeping;

            GetComponent<NpcBehavior>().working = true;

            npcPathFinding.ChangeLocation(houseSweeping.GetBroomLocation());

            goGetBroom = true;
        }
    }

    public void ArrivedAtLocation()
    {
        if(goGetBroom)
        {
            npcPathFinding.ChangeIdleAnimation(Direction.Up);

            animator.SetBool("Broom", true);

            goGetBroom = false;
        }
        else if(sweeping)
        {
            StartCoroutine(WaitForSweeping());
        }
    }

    private IEnumerator WaitForSweeping()
    {
        dialogueDisplay.CanTalk = false;

        npcPathFinding.ChangeIdleAnimation(Direction.Up);

        animator.SetBool("Sweep", true);

        yield return new WaitForSeconds(sweepingDuration);

        animator.SetBool("Sweep", false);

        yield return new WaitForSeconds(.75f);

        dialogueDisplay.CanTalk = true;

        MoveToANewLocation();
    }

    private void TriggerDeactivateBroom()
    {
        houseSweeping.DeactivateBroom();
    }

    private void TriggerPickUpBroom()
    {
        sweeping = true;

        houseSweeping.HideBroom();

        MoveToANewLocation();
    }

    private void MoveToANewLocation()
    {
        npcPathFinding.ChangeLocation(houseSweeping.GetRandomPosition());
    }
}
