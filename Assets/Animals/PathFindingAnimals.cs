using System.Collections.Generic;
using UnityEngine;

public class PathFindingAnimals : MonoBehaviour
{
    [SerializeField] private LocationGridSave locationGrid;

    [SerializeField] private float distanceTolarance = .1f;

    [SerializeField] private float walkSpeed = 1f;

    private Pathfinding pathfinding;

    private List<Vector3> pathToLocation = null;

    private bool walking = false;

    private Animator animator;

    private void Start()
    {
        if(locationGrid == null)
        {
            locationGrid = GameObject.Find("Global/AI_Grid").GetComponent<LocationGridSave>();
        }

        pathfinding = new Pathfinding(locationGrid.Grid);

        animator = GetComponent<Animator>();
    }

    public void ChangeLocation(Vector3 newLocation)
    {
        pathToLocation = pathfinding.FindPath(transform.position, newLocation);

        if (pathToLocation != null && pathToLocation.Count > 1)
        {
            pathToLocation.RemoveAt(0);
        }
        else
        {
            pathToLocation = null;
        }

        ChangeDirection();

        walking = true;
    }

    private void MoveToLocation(Vector3 position)
    {
        Vector3 moveDir = (position - transform.position).normalized;

        transform.position = transform.position + moveDir * walkSpeed * Time.deltaTime;
    }

    private void ChangeDirection()
    {
        if(pathToLocation != null && pathToLocation.Count > 0)
        {
            animator.SetBool("Walking", true);

            if (transform.position.x > pathToLocation[0].x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }  
        else
        {
            animator.SetBool("Walking", false);
        }
    }

    private void FixedUpdate()
    {
        if(walking)
        {
            if(pathToLocation.Count > 0)
            {
                MoveToLocation(pathToLocation[0]);

                if(Vector3.Distance(transform.position, pathToLocation[0]) <= distanceTolarance)
                {
                    pathToLocation.RemoveAt(0);

                    ChangeDirection();
                }
            }
        }
    }
}
