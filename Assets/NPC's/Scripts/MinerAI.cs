using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class MinerAI : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private int chanceToGetTired = 15;

    [SerializeField] private int pauseDuration = 10;

    private MinerLocation minerLocation;

    private NpcPathFinding npcPathFinding;

    private Animator animator;

    private AiState state;

    private void Awake()
    {
        npcPathFinding = GetComponent<NpcPathFinding>();

        animator = GetComponent<Animator>();

        state = AiState.None;
    }

    public void StartWorking(MinerLocation minerLocation)
    {
        if(minerLocation != null)
        {
            this.minerLocation = minerLocation;

            state = AiState.GoToPickaxe;

            npcPathFinding.ChangeLocation(minerLocation.pickaxeLocation.position);
        }
    }

    public void ArrivedAtLocation(WaypointData waypoint = null)
    {
        switch (state)
        {
            case AiState.GoToPickaxe:
                {
                    npcPathFinding.ChangeIdleAnimation(Direction.Up);

                    animator.SetBool("Pickaxe", true);

                    break;
                }
            case AiState.GoToStone:
                {
                    npcPathFinding.ChangeIdleAnimation(Direction.Left);

                    animator.SetBool("Prepare", true);

                    state = AiState.StartMining;

                    break;
                }
            case AiState.StartMining:
            {


                break;
            }
        }
    }

    private void TriggerPickPickaxe()
    {
        npcPathFinding.ChangeLocation(minerLocation.stoneLocation.position);

        minerLocation.pickaxeSprite.gameObject.SetActive(false);

        state = AiState.GoToStone;
    }

    private void TriggerStartMining()
    {
        npcPathFinding.ChangeIdleAnimation(Direction.Left);

        animator.SetBool("Use", true);
    }

    private void TriggerPickaxeUse()
    {
        int chance = Random.Range(0, 100);

        if(chance < chanceToGetTired)
        {
            animator.SetBool("Use", false);

            StartCoroutine(Pause());
        }
    }

    private IEnumerator Pause()
    {
        yield return new WaitForSeconds(pauseDuration);

        animator.SetBool("Use", true);
    }

    public void PauseWorking()
    {
        animator.SetBool("Use", false);
        animator.SetBool("Prepare", false);
    }

    private enum AiState
    {
        GoToPickaxe,
        GoToStone,
        StartMining,
        None
    }
}
