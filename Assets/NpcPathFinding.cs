using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcPathFinding : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float distanceTolerance = .1f;

    [SerializeField] private LocationGridSave locationGrid;

    private float initialScaleX;

    private List<Vector3> path = new List<Vector3>();

    private Vector3 changeScale;

    [SerializeField] private Vector3 toLocation;

    private int pathCurrentIndex;

    private Pathfinding pathfinding;

    private Animator animator;

    public LocationGridSave LocationGrid { set { locationGrid = value; LocationGridChange(value); } }

    private void Awake()
    {
        initialScaleX = transform.localScale.x;

        changeScale = transform.localScale;

        pathfinding = new Pathfinding(locationGrid.Grid);

        animator = GetComponent<Animator>();
    }

    private void MoveToPoint(Vector3 position)
    {
        Vector3 moveDir = (position - transform.position).normalized;

        float distanceBefore = Vector3.Distance(transform.position, position);

        transform.position = transform.position + moveDir * speed * Time.deltaTime;

        if (position.x > transform.position.x)
        {
            changeScale.x = initialScaleX;

            transform.localScale = changeScale;
        }
        else
        {
            changeScale.x = -initialScaleX;

            transform.localScale = changeScale;
        }
    }

    private void GetNewPath()
    {
        path = pathfinding.FindPath(transform.position, toLocation);

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
        else if (transform.position.x == position.x)
        {
            direction.x = 0;
        }
        else
        {
            direction.x = -1;
        }

        if (transform.position.y < position.y)
        {
            direction.y = 1;
        }
        else if (transform.position.y - position.y <= .1f)
        {
            direction.y = 0;
        }
        else
        {
            direction.y = -1;
        }

        if(direction != Vector3.zero)
        {
            animator.SetFloat("HorizontalFacing", direction.x);
            animator.SetFloat("VerticalFacing", direction.y);
        }

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        animator.SetBool("Walking", true);
    }

    private void FixedUpdate()
    {
        if(toLocation != null && locationGrid != null)
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
                }
            }
        }
    }

    public void ChangeToLocation(Vector3 location)
    {
        this.toLocation = location;
    }

    private void LocationGridChange(LocationGridSave locationGrid)
    {

    }
}
