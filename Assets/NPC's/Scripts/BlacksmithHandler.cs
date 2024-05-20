using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithHandler : MonoBehaviour
{
    [SerializeField] private int secondsToWait = 10;

    [Header("Anvil details")]
    [SerializeField] private Transform anvilLocation;
    [SerializeField] private Direction anvilDirection;
    [SerializeField] private List<GameObject> deactivateAnvil = new List<GameObject>();
    [SerializeField] private List<GameObject> activateAnvil = new List<GameObject>();

    [Header("Forge details")]
    [SerializeField] private Transform forgeLocation;
    [SerializeField] private Direction forgeDirection;
    [SerializeField] private List<GameObject> deactivateForge = new List<GameObject>();
    [SerializeField] private List<GameObject> activateForge = new List<GameObject>();

    [Header("Forge details")]
    [SerializeField] private Transform woodLocation;
    [SerializeField] private Direction woodDirection;
    [SerializeField] private NPCForgeHandler npcForgeHandler;

    [Header("Wood storage location")]
    [SerializeField] private WaypointData woodStorage;

    private NpcPathFinding npcPathFinding;

    private BlacksmithWoodHandler blacksmithWood;

    private NpcAIHandler npcAIHandler;

    private Animator animator;

    private bool anvil = false;
    private bool forge = false;
    private bool forgeFuel = false;
    private bool woodPickedUp = false;
    public bool movingToPickWood = false;
    private bool started = false;
    public bool enoughtWood = true;
    public bool gettingWood = false;

    private Coroutine coroutine;

    public void Awake()
    {
        npcPathFinding = GetComponent<NpcPathFinding>();

        animator = GetComponent<Animator>();

        blacksmithWood = woodLocation.GetComponent<BlacksmithWoodHandler>();

        npcAIHandler = GetComponent<NpcAIHandler>();
    }

    private int GetRandomState()
    {
        return Random.Range(0, 2);
    }

    public void StartBlacksmith()
    {
        if(!started)
        {
            started = true;
            
            if(!npcForgeHandler.FireOn)
            {
                ChooseAction(2);   
            }
            else
            {
                ChooseAction(GetRandomState());
            }

            npcForgeHandler.blacksmithStartShift(this);
        }
        else
        {
            bool carringWood = animator.GetBool("Wood");

            if(carringWood)
            {
                gettingWood = true;

                movingToPickWood = true;

                MoveToLocation(woodLocation.position, woodDirection);
            }
        }
    }

    public void StopTalking()
    {
        if(!started)
        {
            if (anvil)
            {
                ChooseAction(0);
            }
            else if (forge)
            {
                ChooseAction(1);
            }
            else if(forgeFuel)
            {
                ChooseAction(2);
            }

            started = true;
        }
    }

    public void StartTalking()
    {
        if(started)
        {
            StopCoroutine(coroutine);

            animator.SetBool("Anvil", false);
            animator.SetBool("Forge", false);

            if(!forgeFuel)
            {
                ChangeObjectsState(activateForge, deactivateForge);
            }
            ChangeObjectsState(activateAnvil, deactivateAnvil);

            started = false;
        }
    }

    private void ChangeObjectsState(List<GameObject> deactivate, List<GameObject> activate)
    {
        foreach(GameObject obj in activate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        foreach (GameObject obj in deactivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }

    private void DeactivateForgeObjects()
    {
        ChangeObjectsState(activateForge, deactivateForge);
    }

    private void GetNewAction()
    {
        ChooseAction(GetRandomState());
    }

    private void ChooseAction(int action)
    {
        ChangeObjectsState(activateForge, deactivateForge);
        ChangeObjectsState(activateAnvil, deactivateAnvil);

        switch (action)
        {
            // Anvil
            case 0:
            {
                if(anvil)
                {
                    ArrivedAtLocation();
                }
                else
                {
                    MoveToLocation(anvilLocation.position, anvilDirection);   

                    anvil = true;
                    forge = false;
                }

                break;
            }

            // Forge
            case 1:
            {
                if (forge)
                {
                    ArrivedAtLocation();
                }
                else
                {
                    MoveToLocation(forgeLocation.position, forgeDirection);

                    anvil = false;
                    forge = true;
                }

                break;
            }
            // Wood
            case 2:
            {
                animator.SetBool("Anvil", false);
                animator.SetBool("Forge", false);

                ChangeObjectsState(activateForge, deactivateForge);
                ChangeObjectsState(activateAnvil, deactivateAnvil);

                movingToPickWood = true;

                if(woodPickedUp)
                {
                    MoveToLocation(forgeLocation.position, forgeDirection);

                    anvil = false;
                    forge = false;
                    forgeFuel = true;
                    woodPickedUp = true;
                }
                else
                {
                    MoveToLocation(woodLocation.position, woodDirection);

                    anvil = false;
                    forge = false;
                    forgeFuel = true;
                    woodPickedUp = false;
                }

                break;
            }
        }
    }

    private void MoveToLocation(Vector3 location, Direction direction)
    {
        npcPathFinding.ChangeLocation(location);

        npcPathFinding.ChangeIdleAnimation(direction);
    }

    public void ArrivedAtLocation()
    {
        if(npcPathFinding.CanWalk == false)
        {
            if (movingToPickWood)
            {
                if(gettingWood)
                {
                    npcPathFinding.ChangeIdleAnimation(Direction.Up);

                    animator.SetBool("Wood", false);

                    animator.SetTrigger("Wood_Forge");


                    forgeFuel = false;

                    woodPickedUp = false;
                    movingToPickWood = false;

                    enoughtWood = true;
                    gettingWood = false;

                    forge = false;
                    anvil = false;

                    blacksmithWood.AddWood();

                    ChooseAction(2);
                }
                else if(woodPickedUp)
                {
                    ChangeObjectsState(deactivateForge, activateForge);

                    animator.SetBool("Anvil", false);
                    animator.SetBool("Forge", false);
                    animator.SetTrigger("Wood_Forge");
                    animator.SetBool("Wood", false);

                    forgeFuel = false;
                    woodPickedUp = false;

                    movingToPickWood = false;

                    npcForgeHandler.FuelForge();

                    forge = true;

                    if(!enoughtWood)
                    {
                        npcAIHandler.MoveToWaypoint(woodStorage, false);
                    }
                }
                else
                {
                    animator.SetBool("Anvil", false);
                    animator.SetBool("Forge", false);
                    animator.SetTrigger("Pick_Up");
                    animator.SetBool("Wood", true);

                    woodPickedUp = true;

                    enoughtWood = blacksmithWood.PickUpWood();

                    ChooseAction(2);
                }
            }
            else
            {
                if (anvil)
                {
                    ChangeObjectsState(deactivateAnvil, activateAnvil);
                }
                else if (forge)
                {
                    ChangeObjectsState(deactivateForge, activateForge);
                }

                animator.SetBool("Anvil", anvil);
                animator.SetBool("Forge", forge);

                coroutine = StartCoroutine(WaitBeforMove());
            }
        }
    }


    public void ForgeNeedsFuel()
    {
        forgeFuel = true;
    }

    private IEnumerator WaitBeforMove()
    {
        yield return new WaitForSeconds(secondsToWait);

        if (enoughtWood)
        {
            if (forgeFuel)
            {
                ChooseAction(2);
            }
            else
            {
                int action = GetRandomState();

                switch (action)
                {
                    case 0:
                        {
                            if (!anvil)
                            {
                                animator.SetBool("Forge", false);

                                ChangeObjectsState(activateForge, deactivateForge);

                                yield return new WaitForSeconds(.5f);
                            }

                            break;
                        }
                    case 1:
                        {
                            if (!forge)
                            {
                                animator.SetBool("Anvil", false);

                                ChangeObjectsState(activateAnvil, deactivateAnvil);

                                yield return new WaitForSeconds(.5f);
                            }

                            break;
                        }
                }

                ChooseAction(action);
            }
        }
    }
}
