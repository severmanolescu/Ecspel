using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPathFinding : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float distanceTolerance = .1f;

    private LocationGridSave locationGrid;

    private bool canWalk;

    private List<Vector3> path = new List<Vector3>();

    private Vector3 toLocation;

    private int pathCurrentIndex;

    private Pathfinding pathfinding;

    private Animator animator;

    private NpcAIHandler npcAIHandler;

    public LocationGridSave LocationGrid { set { locationGrid = value; ChangePathfindingLocation(); } }

    public Vector3 ToLocation { get { return toLocation; } set { toLocation = value; GetNewPath(); } }

    public bool CanWalk { get => canWalk; set => canWalk = value; }

    private void Awake()
    {
        animator = GetComponent<Animator>();

        npcAIHandler = GetComponent<NpcAIHandler>();
    }

    private void MoveToPoint(Vector3 position)
    {
        Vector3 moveDir = (position - transform.position).normalized;

        float distanceBefore = Vector3.Distance(transform.position, position);

        transform.position = transform.position + moveDir * speed * Time.deltaTime;
    }

    private void GetNewPath()
    {
        path = pathfinding.FindPath(transform.position, ToLocation);

        pathCurrentIndex = 0;

        if (path != null && path.Count > 1)
        {
            path.RemoveAt(0);
        }
        else
        {
            path = null;
        }
    }

    private void SetAnimator(Vector3 position)
    {
        Vector3 direction = Vector3.zero;
        
        if(transform.position.x < position.x)
        {
            direction.x = 1;
        } 
        else if(transform.position.x > position.x)
        {
            direction.x = -1;
        }

        if (transform.position.y < position.y)
        {
            direction.y = 1;
        }
        else if (transform.position.y > position.y)
        {
            direction.y = -1;
        }

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

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

    private void FixedUpdate()
    {
        if(ToLocation != null && 
           locationGrid != null && 
           CanWalk == true)
        {
            if(path == null || path.Count == 0)
            {
                GetNewPath();   
            }
            else
            {
                if (pathCurrentIndex < path.Count)
                {
                    MoveToPoint(path[pathCurrentIndex]);

                    SetAnimator(path[pathCurrentIndex]);

                    if (Vector3.Distance(transform.position, path[pathCurrentIndex]) <= distanceTolerance)
                    {
                        pathCurrentIndex++;
                    }
                }
                else
                {
                    animator.SetBool("Walking", false);

                    path.Clear();

                    npcAIHandler.ArrivedAtLocation();
                }
            }
        }
    }

    private void ChangePathfindingLocation()
    {
        pathfinding = new Pathfinding(locationGrid.Grid);
    }

    public void ChangeLocation(LocationGridSave locationGrid, Vector3 toLocation)
    {
        this.locationGrid = locationGrid;

        ChangePathfindingLocation();

        ToLocation = toLocation;
    }

    public void MoveIdleAnimation(int direction) // 0 - left; 1 - up; 2 - right; 3 - down
    {
        Vector3 newDirection = Vector3.zero;

        switch(direction)
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

        if(newDirection != Vector3.zero)
        {
            animator.SetFloat("HorizontalFacing", newDirection.x);
            animator.SetFloat("VerticalFacing", newDirection.y);
        }
    }
}
