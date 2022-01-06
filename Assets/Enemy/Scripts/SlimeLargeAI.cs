using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeLargeAI : MonoBehaviour
{
    enum State
    {
        Walking,
        GoToPlayer,
        Attack,
    }

    [Header("Slime stats")]
    [SerializeField] private float speed;

    private float nextAttackTime;

    private State state;

    private AIPathFinding aIPath;

    private Transform playerLocation;

    private Animator animator;

    private void Awake()
    {
        aIPath = GetComponent<AIPathFinding>();

        aIPath.Speed = speed;

        playerLocation = GameObject.Find("Player").GetComponent<Transform>();

        animator = GetComponent<Animator>();

        state = State.Walking;
    }

    private void Start()
    {
        nextAttackTime = DefaulData.slimeAttackRate;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Walking:
                {
                    aIPath.Walking = true;

                    FindPlayer();

                    break;
                }

            case State.GoToPlayer:
                {
                    aIPath.Walking = false;

                    if (aIPath.ToLocation == null)
                    {
                        aIPath.ToLocation = playerLocation;
                    }

                    float distance = Vector3.Distance(transform.position, playerLocation.position);

                    if (distance <= DefaulData.slimeBigAttackDistance)
                    {
                        state = State.Attack;

                        aIPath.ToLocation = null;
                    }
                    else if (distance >= DefaulData.maxDinstanceToCatch)
                    {
                        state = State.Walking;

                        aIPath.ToLocation = null;
                    }

                    break;
                }
            case State.Attack:
                {
                    if (Time.time > nextAttackTime)
                    {
                        animator.SetTrigger("Attack");

                        nextAttackTime = Time.time + DefaulData.slimeAttackRate;
                    }

                    float distance = Vector3.Distance(transform.position, playerLocation.position);

                    if (distance > DefaulData.slimeBigAttackDistance)
                    {
                        state = State.GoToPlayer;
                    }

                    break;
                }
        }
    }

    public void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, playerLocation.position) <= DefaulData.slimeBigAttackDistance)
        {
            playerLocation.GetComponent<PlayerStats>().Health -= DefaulData.slimeBigAttackPower;
        }
    }

    private void FindPlayer()
    {
        if (Vector3.Distance(transform.position, playerLocation.position) <= DefaulData.distanceToFind)
        {
            state = State.GoToPlayer;
        }
    }

    public void DestroyEnemyAnimation()
    {
        GetComponent<EnemyDropLoot>().DropItem();

        GetComponent<SpriteRenderer>().sortingOrder = -1;

        Destroy(GetComponent<BoxCollider2D>());
        gameObject.AddComponent<TimeDegradation>();

        GameObject.Find("Player").GetComponent<PlayerAchievements>().smallSlime++;

        Destroy(this);
    }
}
