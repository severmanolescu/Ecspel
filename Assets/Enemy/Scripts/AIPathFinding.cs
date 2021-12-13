using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFinding : MonoBehaviour
{
    [SerializeField] private LocationGridSave locationGrid;

    private bool walking;

    private float initialScaleX;

    private Vector3 changeScale;

    private bool canMove = true;

    private Transform toLocation;
    private Vector3 lastPosition;

    private int currentIndex;

    private Pathfinding pathfinding;

    public List<Vector3> path;

    private float speed;

    private Vector2 roaming;

    private Vector3 initialLocation;

    public Transform ToLocation { get => toLocation; set { toLocation = value; path = null; } }
    public float Speed { get => speed; set => speed = value; }
    public bool Walking { get => walking; set => walking = value; }

    private void Awake()
    {
        initialScaleX = transform.localScale.x;

        changeScale = transform.localScale;

        roaming = GetRoamingPosition();
    }

    private void Start()
    {
        pathfinding = new Pathfinding(locationGrid.Grid);

        initialLocation = transform.position;

        roaming = GetRoamingPosition();
    }

    public void MoveToLocation(Vector3 position)
    {
        if (canMove)
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
    }

    public Vector3 GetRoamingPosition()
    {
        return initialLocation + DefaulData.GetRandomMove() * Random.Range(2f, 5f);
    }

    public void SetCanMoveToTrue()
    {
        canMove = true;
    }
    public void SetCanMoveToFalse()
    {
        canMove = false;
    }

    private void FixedUpdate()
    {
        if (ToLocation != null)
        {
            if (path != null)
            {
                if (path.Count > 1)
                {
                    MoveToLocation(path[0]);

                    if (Vector3.Distance(transform.position, path[0]) <= 0.1f)
                    {
                        path = pathfinding.FindPath(transform.position, new Vector3(ToLocation.position.x, ToLocation.position.y));

                        path.RemoveAt(0);

                        lastPosition = ToLocation.position;
                    }
                }
                else if (lastPosition != ToLocation.position)
                {
                    path = pathfinding.FindPath(transform.position, new Vector3(ToLocation.position.x, ToLocation.position.y));

                    path.RemoveAt(0);

                    lastPosition = ToLocation.position;
                }
            }
            else
            {
                path = pathfinding.FindPath(transform.position, new Vector3(ToLocation.position.x, ToLocation.position.y));

                path.RemoveAt(0);

                lastPosition = ToLocation.position;
            }
        }
        else if (Walking)
        {
            if (path != null)
            {
                if(currentIndex <= path.Count - 1)
                {
                    MoveToLocation(path[currentIndex]);

                    if (Vector3.Distance(transform.position, path[currentIndex]) <= 0.1f)
                    {
                        currentIndex++;
                    }
                }
                else
                {
                    path = null;
                    roaming = GetRoamingPosition();
                }
            }
            else
            {
                path = pathfinding.FindPath(transform.position, new Vector3(roaming.x, roaming.y));

                path.RemoveAt(0);

                currentIndex = 0;
            }
        }
    }
}
