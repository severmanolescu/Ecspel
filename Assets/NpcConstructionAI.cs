using UnityEngine;

public class NpcConstructionAI : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private int changeToChangeAnimationPrepared = 75;
    [Range(0, 100)]
    [SerializeField] private int changeToSweatAnimation = 25;

    private ConstructionData construction;

    private NpcPathFinding npcPathFinding;

    private Animator animator;

    private bool goPickUpShovel = false;
    private bool goPutShovel = false;

    private bool atAHole = false;

    private bool digTheHole = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        npcPathFinding = GetComponent<NpcPathFinding>();
    }

    public void GoToTheNextHole()
    {
        Vector3 holeLocation = construction.GetNextHole();

        if (holeLocation != DefaulData.nullVector)
        {
            npcPathFinding.ChangeLocation(holeLocation);

            digTheHole = true;
        }
        else
        {
            digTheHole = false;
        }
    }

    public void StartToConstruct(ConstructionData construction)
    {
        if(construction != null)
        {
            this.construction = construction;

            construction.SetBuilderAI(this);

            bool shovel = animator.GetBool("Pick_Up_Shovel");

            if(!shovel)
            {
                goPickUpShovel = true;

                npcPathFinding.ChangeLocation(construction.ShovelPosition.position);
            }
            else
            {
                GoToTheNextHole();
            }
        }
    }

    private void TriggerPickUpShovel()
    {
        if(goPickUpShovel)
        {
            construction.GetShovel();

            goPickUpShovel = false;
        }   
    }

    private void TriggerPutShovel()
    {
        if (goPutShovel)
        {
            construction.PutShovel();

            goPutShovel = false;
        }
    }

    private void TriggerShovelIdle()
    {
        if (construction.BuilderHelperAI != null && atAHole == true)
        {
            construction.ReadyForHelperPilon();
        }
        else if(!digTheHole)
        {
            atAHole = false;

            GoToTheNextHole();

            if (!digTheHole)
            {
                npcPathFinding.ChangeLocation(construction.ShovelPosition.position);

                goPutShovel = true;
            }
        } 
    }

    private void TriggerShovelReady()
    {
        if(digTheHole)
        {
            int randomValue = Random.Range(0, 100);

            if (randomValue <= changeToChangeAnimationPrepared)
            {
                randomValue = Random.Range(0, 100);

                if (randomValue < changeToSweatAnimation)
                {
                    SetAnimatorTrigger("Sweating");
                }
                else
                {
                    SetAnimatorTrigger("Use_Shovel");
                }
            }
        }    
    }

    private void TriggerUseShovel()
    {
        bool finish = construction.WidenTheHole();
        
        if(finish)
        {
            digTheHole = false;

            SetAnimatorBoolean("Prepare", false);
        }
    }

    private void TriggerPutDirt()
    {
        construction.PutToPile();
    }

    private void TriggerHoleFill()
    {
        if(!construction.CheckIfLeftDirt())
        {
            SetAnimatorBoolean("Prepare", false);
            SetAnimatorBoolean("Hole_Fill", false);

            atAHole = false;
            digTheHole = false;
        }
    }

    private void TriggerHoleFillStart()
    {
        construction.RemoveFromPile();
    }

    public void HoleFill()
    {
        SetAnimatorBoolean("Prepare", true);
        SetAnimatorBoolean("Hole_Fill", true);
    }

    public void ArivedAtLocation()
    {
        if(goPickUpShovel)
        {
            PickUpShovelHandle();
        }
        else if(goPutShovel)
        {
            PutShovelHandle();
        }
        else
        {
            PrepareForHoleHandle();
        }
    }

    private void PickUpShovelHandle()
    {
        npcPathFinding.MoveIdleAnimation(construction.ShovelDirection);

        SetAnimatorBoolean("Pick_Up_Shovel", true);
    }
    
    private void PutShovelHandle()
    {
        npcPathFinding.MoveIdleAnimation(construction.ShovelDirection);

        SetAnimatorBoolean("Pick_Up_Shovel", false);
    }

    private void PrepareForHoleHandle()
    {
        npcPathFinding.SetAnimatorDirectionToLocation(construction.GetHolePosition());

        atAHole = true;

        if (digTheHole)
        {
            SetAnimatorBoolean("Prepare", true);
        }
    }

    private void SetAnimatorBoolean(string parameter, bool value)
    {
        animator.SetBool(parameter, value);
    }

    private void SetAnimatorTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }
}
