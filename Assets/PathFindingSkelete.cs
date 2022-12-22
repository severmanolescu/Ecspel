using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class PathFindingSkelete : MonoBehaviour
{
    [SerializeField] private float maxDistanceFromSpawn = 5f;
    [SerializeField] private float toleranteToNewLocation = 0.1f;

    [SerializeField] private LocationGridSave locationGrid;

    [SerializeField] private float minMovementSpeed = 1f;
    [SerializeField] private float maxMovementSpeed = 2f;
    private float moveSpeed;

    [SerializeField] private float minAttackDistance = 1f;
    [SerializeField] private float maxAttackDistance = 2f;

    [SerializeField] private float procentageToIdle = 50f;
    [SerializeField] private int minTimetoIdle = 5;
    [SerializeField] private int maxTimetoIdle = 10;

    [SerializeField] private int minAmountOfHeads = 1;
    [SerializeField] private int maxAmountOfHeads = 3;

    [SerializeField] private Transform headSpawnLocation;
    [SerializeField] private GameObject headPrefab;

    private float attackDistance;

    private Vector3 spawnLocation;

    private Pathfinding pathFinding;

    private Vector2 toLocation;

    private bool canMove = true;

    private bool idle = false;

    private int pathIndex = 0;

    private bool moveToPlayer = false;

    private List<Vector3> path = new();

    private Transform player;

    private GridNode lastPlayerNodeLocation;

    private Animator animator;

    private bool attackStarted = true;

    private Vector3 initialScale;

    private void Awake()
    {
        spawnLocation = transform.position;

        initialScale = transform.localScale;

        player = GameObject.Find("Global/Player").transform;

        animator = GetComponent<Animator>();

        attackDistance = Random.Range(minAttackDistance, maxAttackDistance);
        moveSpeed = Random.Range(minMovementSpeed, maxMovementSpeed);

        GetComponent<AttackPlayer>().MaxAttackDistance = maxAttackDistance;
    }

    private void Start()
    {
        pathFinding = new Pathfinding(locationGrid.Grid);

        ChangeDestination(GetNewRoamingPosition());
    }

    public void ChangeDestination(Vector2 destination)
    {
        if(!destination.Equals(toLocation)) 
        {
            toLocation = destination;

            path = pathFinding.FindPath(transform.position, toLocation);

            if (path != null && path.Count > 1)
            {
                path.RemoveAt(0);
            }
            else
            {
                path = null;
            }

            pathIndex = 0;
        }
    }

    private Vector3 GetNewRoamingPosition()
    {
        return spawnLocation + DefaulData.GetRandomMove() * Random.Range(2f, 5f);
    }

    private void MoveToLocation(Vector3 location)
    {
        if (attackStarted == true)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);
            animator.SetBool("AttackHead", false);

            animator.SetTrigger("AttackStop");

            attackStarted = false;
        }

        ChangeSpriteDirection(location.x);

        Vector3 moveDir = (location - transform.position).normalized;

        transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, path[pathIndex]) <= toleranteToNewLocation)
        {
            pathIndex++;
        }
    }

    public void ChangeSpriteDirection()
    {
        ChangeSpriteDirection(player.position.x);
    }

    public void ChangeSpriteDirection(float location)
    {
        if (location < transform.position.x)
        {
            initialScale.x = Mathf.Abs(initialScale.x);

            transform.localScale = initialScale;
        }
        else
        {
            initialScale.x = -Mathf.Abs(initialScale.x);

            transform.localScale = initialScale;
        }
    }

    private void MoveToPlayer()
    {
        GridNode newPlayerLocation = locationGrid.Grid.GetGridObject(player.position);

        if (newPlayerLocation != lastPlayerNodeLocation)
        {
            ChangeDestination(player.position);

            lastPlayerNodeLocation = newPlayerLocation;
        }
        else
        {
            if (path == null || path.Count == 0)
            {
                ChangeDestination(player.position);
            }
            else
            {
                MoveToLocation(path[pathIndex]);
            }
        }
    }

    private void PlayerLost()
    {
        moveToPlayer = false;

        path = null;

        AttackPlayerWithHead();
    }

    private void AttackPlayer()
    {
        canMove = false;

        if (attackStarted == false)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);
            animator.SetBool("AttackHead", false);

            attackStarted = true;
        }
    }

    private void AttackPlayerWithHead()
    {
        canMove = false;

        if (attackStarted == false)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", false);
            animator.SetBool("AttackHead", true);

            attackStarted = true;
        }
    }

    public void SpawnHead()
    {
        Instantiate(headPrefab, headSpawnLocation.position, headSpawnLocation.rotation);
    }

    private IEnumerator Idle()
    {
        animator.SetBool("Walk", false);
        animator.SetBool("Attack", false);

        attackStarted = true;

        yield return new WaitForSeconds(Random.Range(minTimetoIdle, maxTimetoIdle));

        idle = false;

        ChangeDestination(GetNewRoamingPosition());
    }

    private void CheckIfIdle()
    {
        float procentage = Random.Range(0, 50);

        if(procentage <= procentageToIdle)
        {
            idle = true;

            StartCoroutine(Idle());
        }
    }

    private void FixedUpdate()
    {
        if(canMove == true && idle == false && toLocation != null && !toLocation.Equals(transform.position))
        {
            if(moveToPlayer == true)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance <= attackDistance)
                {
                    AttackPlayer();
                }
                else if(distance >= maxDistanceFromSpawn)
                {
                    PlayerLost();
                }
                else
                {
                    MoveToPlayer();
                }
            }
            else
            {
                if(path != null)
                {
                    if (pathIndex < path.Count)
                    {
                        MoveToLocation(path[pathIndex]);
                    }
                    else
                    {
                        CheckIfIdle();

                        if(idle == false)
                        {
                            ChangeDestination(GetNewRoamingPosition());
                        }
                    }
                }
                else
                {
                    ChangeDestination(GetNewRoamingPosition());
                }
            }
        }
    }

    public void ChangeCanMoveToTrue()
    {
        canMove = true;
    }

    public void ChangeCanMoveToFalse()
    {
        canMove = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StopAllCoroutines();

            idle = false;

            moveToPlayer = true;

            ChangeDestination(player.position);
        }
    }
}
