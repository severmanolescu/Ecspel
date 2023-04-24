using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFindingEye : MonoBehaviour
{
    [SerializeField] private float toleranteToNewLocation = 0.1f;

    [SerializeField] private LocationGridSave locationGrid;

    [SerializeField] private float minMovementSpeed = 1f;
    [SerializeField] private float maxMovementSpeed = 2f;

    [SerializeField] private float minAttackDistance = 1f;
    [SerializeField] private float maxAttackDistance = 2f;

    [SerializeField] private float procentageToIdle = 50f;
    [SerializeField] private int minTimetoIdle = 5;
    [SerializeField] private int maxTimetoIdle = 10;

    [SerializeField] private int minDistanceFromPlayer = 7;
    [SerializeField] private int maxDistanceFromPlayer = 15;

    private float moveSpeed;

    private float attackDistance;

    private float distanceFromPlayer;

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
        distanceFromPlayer = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);


        GetComponent<AttackPlayer>().MaxAttackDistance = maxAttackDistance;
    }

    private void Start()
    {
        if (locationGrid == null)
        {
            locationGrid = GameObject.Find("PlayerGround").GetComponent<LocationGridSave>();
        }

        pathFinding = new Pathfinding(locationGrid.Grid);

        ChangeDestination(GetNewRoamingPosition());
    }

    public void ChangeDestination(Vector2 destination)
    {
        if (!destination.Equals(toLocation))
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

            animator.SetTrigger("AttackStop");

            attackStarted = false;
        }

        ChangeSpriteDirection(location.x);

        Vector3 moveDir = (location - transform.position).normalized;

        transform.position = transform.position + moveDir * moveSpeed * Time.deltaTime;

        if (path != null &&
            pathIndex < path.Count &&
            Vector3.Distance(transform.position, path[pathIndex]) <= toleranteToNewLocation)
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

    private GridNode GetRandomNodeNearPlayer(GridNode playerPosition)
    {
        if (CheckGridNode(playerPosition.x - 1, playerPosition.y))
        {
            return locationGrid.Grid.GetGridObject(playerPosition.x - 1, playerPosition.y);
        }
        else if (CheckGridNode(playerPosition.x + 1, playerPosition.y))
        {
            return locationGrid.Grid.GetGridObject(playerPosition.x + 1, playerPosition.y);

        }
        else if (CheckGridNode(playerPosition.x, playerPosition.y - 1))
        {
            return locationGrid.Grid.GetGridObject(playerPosition.x, playerPosition.y - 1);

        }
        else if (CheckGridNode(playerPosition.x, playerPosition.y + 1))
        {
            return locationGrid.Grid.GetGridObject(playerPosition.x, playerPosition.y + 1);

        }

        return null;
    }

    private bool CheckGridNode(int x, int y)
    {
        GridNode gridNode = locationGrid.Grid.GetGridObject(x, y);

        if (gridNode != null && gridNode.isWalkable)
        {
            return true;
        }

        return false;
    }

    private void ChangeDestination(GridNode position, int xOffset, int yOffset)
    {
        if (CheckGridNode(position.x + xOffset, position.y + yOffset))
        {
            Vector3 positionInCenter = new Vector3(position.x + xOffset, position.y + yOffset);

            positionInCenter.x += locationGrid.Grid.CellSize / 2;
            positionInCenter.y += locationGrid.Grid.CellSize / 2;

            ChangeDestination(positionInCenter);
        }
        else
        {
            GridNode nearPlayer = GetRandomNodeNearPlayer(position);

            if (nearPlayer != null)
            {
                Vector3 positionInCenter = locationGrid.Grid.GetWorldPosition(nearPlayer);

                positionInCenter.x += locationGrid.Grid.CellSize / 2;
                positionInCenter.y += locationGrid.Grid.CellSize / 2;

                ChangeDestination(positionInCenter);
            }
            else
            {
                PlayerLost();
            }
        }
    }

    private void GetNewPositionToPlayer(GridNode newPlayerLocation)
    {
        GridNode enemyPosition = locationGrid.Grid.GetGridObject(transform.position);

        lastPlayerNodeLocation = newPlayerLocation;

        if (enemyPosition != null)
        {
            if (enemyPosition.x == newPlayerLocation.x)
            {
                if (enemyPosition.y < newPlayerLocation.y)
                {
                    ChangeDestination(newPlayerLocation, 0, -1);
                }
                else
                {
                    ChangeDestination(newPlayerLocation, 0, 1);
                }
            }
            else if (enemyPosition.y == newPlayerLocation.y)
            {
                if (enemyPosition.x < newPlayerLocation.x)
                {
                    ChangeDestination(newPlayerLocation, -1, 0);
                }
                else
                {
                    ChangeDestination(newPlayerLocation, 1, 0);
                }
            }
        }
        else
        {
            PlayerLost();
        }
    }

    private void MoveToPlayer()
    {
        GridNode newPlayerLocation = locationGrid.Grid.GetGridObject(player.position);

        if (attackStarted == true)
        {
            animator.SetBool("Walk", true);
            animator.SetBool("Attack", false);

            attackStarted = false;
        }

        if (newPlayerLocation != null && newPlayerLocation != lastPlayerNodeLocation)
        {
            if (newPlayerLocation.isWalkable == true)
            {
                ChangeDestination(player.position);

                lastPlayerNodeLocation = newPlayerLocation;
            }
            else
            {
                GetNewPositionToPlayer(newPlayerLocation);
            }
        }
        else
        {
            if (path == null || path.Count == 0 || pathIndex >= path.Count)
            {
                ChangeDestination(player.position);

                if (path == null || path.Count == 0)
                {
                    MoveToLocation(player.position);
                }
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
    }

    private void AttackPlayer()
    {
        canMove = false;

        if (attackStarted == false)
        {
            animator.SetBool("Walk", false);
            animator.SetBool("Attack", true);

            attackStarted = true;
        }
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

        if (procentage <= procentageToIdle)
        {
            idle = true;

            StartCoroutine(Idle());
        }
    }

    private void FixedUpdate()
    {
        if (canMove == true && idle == false && toLocation != null && !toLocation.Equals(transform.position))
        {
            if (moveToPlayer == true)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance <= attackDistance)
                {
                    AttackPlayer();
                }
                else if (distance >= distanceFromPlayer)
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
                if (path != null)
                {
                    if (pathIndex < path.Count)
                    {
                        MoveToLocation(path[pathIndex]);
                    }
                    else
                    {
                        CheckIfIdle();

                        if (idle == false)
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
        if (moveToPlayer == false)
        {
            StopAllCoroutines();

            idle = false;

            moveToPlayer = true;

            ChangeDestination(player.position);
        }
    }
}
