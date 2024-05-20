using System.Collections.Generic;
using UnityEngine;

public class NpcPathFinding : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float distanceTolerance = .1f;

    [SerializeField] private LocationGridSave locationGrid;

    private bool canWalk = false;
    private bool talking = false;
    private bool action = false;

    private List<Vector3> path = new List<Vector3>();

    private Vector3 toLocation = DefaulData.nullVector;

    private int pathCurrentIndex;

    private Pathfinding pathfinding;

    private Animator animator;

    private NpcAIHandler npcAIHandler;

    private Transform playerTransform;

    public Vector3 ToLocation { get { return toLocation; } set { toLocation = value; GetNewPath(); } }

    public bool CanWalk { get => canWalk; set => canWalk = value; }
    public bool Talking { set { talking = value; CheckForNpcAI(); SetAnimatorValues(); } }
    public bool Action { set { action = value; SetAnimatorValues(); } }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerTransform = GameObject.Find("Global/Player").transform;

        npcAIHandler = GetComponent<NpcAIHandler>();   
    }

    private void CheckForNpcAI()
    {
        if(talking)
        {
            npcAIHandler.StartTalking();
        }
        else
        {
            npcAIHandler.StopTalking();
        }
    }

    public void SetAnimatorValues()
    {
        if(talking)
        {
            SetAnimatorDirectionToLocation(playerTransform.position);
        }
        else if(canWalk)
        {
            if (pathCurrentIndex < path.Count)
            {
                SetAnimator(GetDirection(path[pathCurrentIndex]));
            }
        }
    }

    public void SetTalkingToFalse()
    {
        talking = false;
    }

    public void SetTalkingToTrue()
    {
        talking = true;
    }

    public void SetActionToFalse()
    {
        action = false;
    }

    public void SetActionToTrue()
    {
        action = true;
    }

    private void MoveToPoint(Vector3 position)
    {
        Vector3 moveDir = (position - transform.position).normalized;

        transform.position = transform.position + moveDir * speed * Time.deltaTime;
    }

    private void GetNewPath()
    {
        pathfinding = new Pathfinding(locationGrid.Grid);

        path = pathfinding.FindPath(locationGrid.Grid.GetWorldPositionFronGridObject(transform.position),
                                    locationGrid.Grid.GetWorldPositionFronGridObject(ToLocation));

        pathCurrentIndex = 0;

        if (path != null)
        {
            if (path.Count == 1)
            {
                Arrived();
            }
            else if(path.Count > 1)
            {
                path.RemoveAt(0);
            }
            else
            {
                path = null;
            }
        }
        else
        {
            path = null;
        }

        if (path != null && pathCurrentIndex < path.Count)
        {
            SetAnimator(GetDirection(path[pathCurrentIndex]));
        }
    }

    private Vector3 GetDirection(Vector3 position)
    {
        if (position != DefaulData.nullVector)
        {
            Vector3 direction = Vector3.zero;

            GridNode npcNode = locationGrid.Grid.GetGridObject(transform.position);
            GridNode toNode = locationGrid.Grid.GetGridObject(position);

            if (npcNode != null && toNode != null)
            {
                if (npcNode.x < toNode.x)
                {
                    direction.x = 1;
                }
                else if (npcNode.x > toNode.x)
                {
                    direction.x = -1;
                }

                if (npcNode.y < toNode.y)
                {
                    direction.y = 1;
                }
                else if (npcNode.y > toNode.y)
                {
                    direction.y = -1;
                }

                return direction;
            }
        }

        return DefaulData.nullVector;
    }

    private void SetAnimator(Vector3 direction)
    {
        if (direction != DefaulData.nullVector)
        {
            if (direction.x > 0 && direction.y > 0) //Up right
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", direction.y);
            }
            else if (direction.x < 0 && direction.y > 0) //Up left
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", direction.y);
            }
            else if (direction.x > 0 && direction.y < 0) //Down right
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", direction.y);
            }
            else if (direction.x < 0 && direction.y < 0) //Down left
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", direction.y);
            }
            else
            {
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
            }

            if (direction != Vector3.zero)
            {
                animator.SetFloat("HorizontalFacing", direction.x);
                animator.SetFloat("VerticalFacing", direction.y);

                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", false);
            }
        }
    }

    public void SetAnimatorDirectionToLocation(Vector3 position)
    {
        SetAnimator(GetDirection(position));
    }

    private void FixedUpdate()
    {
        if (ToLocation != DefaulData.nullVector &&
           locationGrid != null && 
           canWalk && !talking && !action)
        {
            if (path == null || path.Count == 0)
            {
                GetNewPath();
            }
            else
            {
                if (pathCurrentIndex < path.Count)
                {
                    MoveToPoint(path[pathCurrentIndex]);

                    if (Vector3.Distance(transform.position, path[pathCurrentIndex]) <= distanceTolerance)
                    {
                        pathCurrentIndex++;

                        if (pathCurrentIndex < path.Count)
                        {
                            SetAnimatorDirectionToLocation(path[pathCurrentIndex]);
                        }
                    }
                }
                else
                {
                    Arrived();
                }
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    public void StopWalk()
    {
        animator.SetBool("Walking", false);

        path.Clear();

        toLocation = DefaulData.nullVector;

        pathCurrentIndex = 0;

        canWalk = false;
    }

    public void Arrived()
    {
        StopWalk();

        npcAIHandler.ArrivedAtLocation();
    }

    public void ChangeLocation(Vector3 toLocation)
    {
        ToLocation = toLocation;

        canWalk = true;
    }

    public void ChangeIdleAnimation(int direction) // 0 - left; 1 - up; 2 - right; 3 - down
    {
        Vector3 newDirection = Vector3.zero;

        switch (direction)
        {
            case 0:
                {
                    newDirection = Vector3.left;

                    break;
                }
            case 1:
                {
                    newDirection = Vector3.up;

                    break;
                }
            case 2:
                {
                    newDirection = Vector3.right;

                    break;
                }
            case 3:
                {
                    newDirection = Vector3.down;

                    break;
                }
        }

        if (newDirection != Vector3.zero)
        {
            animator.SetFloat("HorizontalFacing", newDirection.x);
            animator.SetFloat("VerticalFacing", newDirection.y);
        }
    }

    public void ChangeIdleAnimation(Direction direction)
    {
        switch(direction)
        {
            case Direction.Right:   ChangeIdleAnimation(2); break;
            case Direction.Left:    ChangeIdleAnimation(0); break;
            case Direction.Up:      ChangeIdleAnimation(1); break;
            case Direction.Down:    ChangeIdleAnimation(3); break;
        }
    }

    public int GetIdleDirection()
    {
        Vector3 direction = Vector3.zero;

        direction.x = animator.GetFloat("HorizontalFacing");
        direction.y = animator.GetFloat("VerticalFacing");

        switch (direction)
        {
            case Vector3 idleDirection when idleDirection.Equals(Vector3.left): return 0;
            case Vector3 idleDirection when idleDirection.Equals(Vector3.up): return 1;
            case Vector3 idleDirection when idleDirection.Equals(Vector3.right): return 2;
            case Vector3 idleDirection when idleDirection.Equals(Vector3.down): return 3;

            default: return 3;
        }
    }
}
