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

    [SerializeField] private float speed;

    private float tolerance = .01f;

    private float nextAttackTime;

    private Vector2 roaming;

    private State state;

    private Vector3 initialLocation;

    private AIPathFinding aIPath;

    private Transform playerLocation;

    private Animator animator;

    private void Awake()
    {
        aIPath = GetComponent<AIPathFinding>();

        playerLocation = GameObject.Find("Player").GetComponent<Transform>();

        animator = GetComponent<Animator>();

        state = State.Walking;
    }

    private void Start()
    {
        initialLocation = gameObject.transform.position;

        roaming = GetRoamingPosition();

        nextAttackTime = DefaulData.slimeAttackRate;
    }

    private Vector3 GetRoamingPosition()
    {
        return initialLocation + DefaulData.GetRandomMove() * Random.Range(2f, 5f);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Walking:
                {
                    aIPath.MoveToLocation(roaming, speed);

                    if (Vector3.Distance(transform.position, roaming) <= tolerance)
                    {
                        roaming = GetRoamingPosition();
                    }

                    FindPlayer();

                    break;
                }

            case State.GoToPlayer:
                {
                    aIPath.MoveToLocation(playerLocation.position, speed);

                    float distance = Vector3.Distance(transform.position, playerLocation.position);

                    if (distance <= DefaulData.slimeBigAttackDistance)
                    {
                        state = State.Attack;
                    }
                    else if (distance >= DefaulData.maxDinstanceToCatch)
                    {
                        state = State.Walking;
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
        Destroy(this);
    }
}
