using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    enum State
    {
        Walking,
        Standing,
        GoToPlayer,
        Attack,
    }

    [SerializeField] private float speed;

    private float tolerance = .01f;

    private Vector2 roaming;

    private new Rigidbody2D rigidbody;
    private State state;

    private Vector3 initialLocation;
    private float maxDistante = 10f;

    private Animator animator;

    private AIPathFinding aIPath;

    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();

        animator = gameObject.GetComponent<Animator>();

        aIPath = gameObject.GetComponent<AIPathFinding>();
    }

    private void Start()
    {
        initialLocation = gameObject.transform.position;

        roaming = GetRoamingPosition();
    }

    private Vector3 GetRoamingPosition()
    {
        return initialLocation + DefaulData.GetRandomMove() * Random.Range(2f, 5f);
    }

    private void Update()
    {
        aIPath.MoveToLocation(roaming, speed);



        if(Vector3.Distance(transform.position, roaming) <= tolerance)
        {
            roaming = GetRoamingPosition();
        }
    }
}
