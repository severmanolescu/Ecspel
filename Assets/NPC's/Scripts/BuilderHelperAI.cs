using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderHelperAI : MonoBehaviour
{
    [SerializeField] private ConstructionData construction;

    private NpcPathFinding npcPathFinding;

    private Animator animator;

    private DialogueDisplay dialogueDisplay;

    private bool goGetPilon = false;
    private bool goPutPilon = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        npcPathFinding = GetComponent<NpcPathFinding>();

        dialogueDisplay = GetComponent<DialogueDisplay>();

        ChangeWorkState(true);
    }

    private void Start()
    {
        if(construction != null)
        {
            npcPathFinding.ChangeLocation(construction.HelperPosition.position);

            construction.SetHelperAI(this);
        }
    }

    public void ChangeWorkState(bool work)
    {
        NpcBehavior npcBehavior = GetComponent<NpcBehavior>();

        if (npcBehavior != null)
        {
            npcBehavior.working = work;

            if(!work)
            {
                npcBehavior.StartBehavior();
            }
        }
    }

    public void ArivedAtLocation()
    {
        if(goGetPilon)
        {
            if(!animator.GetBool("Pilon"))
            {
                dialogueDisplay.CanTalk = false;

                npcPathFinding.ChangeIdleAnimation(Direction.Right);

                animator.SetBool("Pilon", true);
            }
            else
            {
                dialogueDisplay.CanTalk = false;

                npcPathFinding.ChangeIdleAnimation(Direction.Down);

                animator.SetBool("Put_Down", true);
            }
        }
        else
        {
            npcPathFinding.ChangeIdleAnimation(Direction.Down);
        }
    }

    private void PilonIdleTrigger()
    {
        if (!goPutPilon)
        {
            Vector3 holePosition = construction.GetHolePositionForTheHelper();

            if(holePosition != DefaulData.nullVector) 
            {
                npcPathFinding.ChangeLocation(holePosition);
            }

            goPutPilon = true;
        }
        else
        {
            npcPathFinding.ChangeIdleAnimation(Direction.Down);
        }
    }

    private void PickUpPilonTrigger()
    {
        construction.PickUpPilon();
    }

    private void PutPilonTrigger()
    {
        construction.PilonInPlace();
    }

    public void LetThePilon()
    {
        animator.SetBool("Pilon", false);

        goGetPilon = false;
        goPutPilon = false;

        npcPathFinding.ChangeLocation(construction.HelperPosition.position);

        dialogueDisplay.CanTalk = true;
    }

    public void GoGetPilon()
    {
        if(!goGetPilon)
        {
            goGetPilon = true;

            npcPathFinding.ChangeLocation(construction.PilonsLocation.position);
        }
    }
}
