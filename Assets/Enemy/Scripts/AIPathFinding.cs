using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFinding : MonoBehaviour
{
    [SerializeField] private LocationGridSave locationGrid;

    [SerializeField] private float distanceTolerance = .1f;

    [Header("Audio effects")]
    [SerializeField] private AudioClip slimeJump;
    [SerializeField] private AudioClip slimeHitGround;

    AudioSource audioSource;

    private bool walking;

    private bool canMove = true;

    private float initialScaleX;

    private float speed;

    private int currentIndex;

    private Vector3 changeScale;

    private Transform toLocation;
    private Vector3 lastPosition;

    private Pathfinding pathfinding;

    private List<Vector3> path;

    private Vector2 roaming;

    private Vector3 initialLocation;

    public Transform ToLocation { get => toLocation; set { toLocation = value; path = null; } }
    public float Speed { get => speed; set => speed = value; }
    public bool Walking { get => walking; set => walking = value; }
    public LocationGridSave LocationGrid { get => locationGrid; set => locationGrid = value; }

    private void Awake()
    {
        initialScaleX = transform.localScale.x;

        changeScale = transform.localScale;

        roaming = GetRoamingPosition();

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        pathfinding = new Pathfinding(LocationGrid.Grid);

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

    public void PlayHitGroundSound()
    {
        audioSource.clip = slimeHitGround;
        audioSource.Play();
    }

    public void PlayJumpSound()
    {
        audioSource.clip = slimeJump;
        audioSource.Play();
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

                    if (Vector3.Distance(transform.position, path[0]) <= distanceTolerance)
                    {
                        path = pathfinding.FindPath(transform.position, toLocation.position);

                        if (path != null && path.Count > 1)
                        {
                            path.RemoveAt(0);

                            lastPosition = ToLocation.position;
                        }
                        else
                        {
                            path = null;
                        }
                    }
                }
                else if (lastPosition != ToLocation.position)
                {
                    path = pathfinding.FindPath(transform.position, toLocation.position);

                    if (path != null && path.Count > 1)
                    {
                        path.RemoveAt(0);

                        lastPosition = ToLocation.position;
                    }
                    else
                    {
                        path = null;
                    }
                }
            }
            else
            {
                path = pathfinding.FindPath(transform.position, toLocation.position);

                if (path != null && path.Count > 1)
                {
                    path.RemoveAt(0);

                    lastPosition = ToLocation.position;
                }
                else
                {
                    path = null;
                }
            }
        }
        else if (Walking)
        {
            if (path != null)
            {
                if(currentIndex <= path.Count - 1)
                {
                    MoveToLocation(path[currentIndex]);

                    if (Vector3.Distance(transform.position, path[currentIndex]) <= distanceTolerance)
                    {
                        currentIndex++;
                    }
                }
                else
                {
                    path = null;
                }
            }
            else
            {
                roaming = GetRoamingPosition();

                path = pathfinding.FindPath(transform.position, new Vector3(roaming.x, roaming.y));

                if (path != null && path.Count > 1)
                {
                    path.RemoveAt(0);

                    currentIndex = 0;
                }
                else
                {
                    path = null;
                }
            }
        }
    }
}
