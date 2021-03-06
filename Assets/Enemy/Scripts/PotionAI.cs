using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionAI : MonoBehaviour
{
    enum State
    {
        Walking,
        GoToPlayer,
        Attack,
    }

    [SerializeField] private float speed;

    [SerializeField] private Effect effect;

    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip rotateSound;

    private State state;

    private AIPathFinding aIPath;

    private Transform playerLocation;

    private Animator animator;

    private ushort rotation = 0;

    private AudioSource audioSource;

    private void Awake()
    {
        aIPath = GetComponent<AIPathFinding>();

        aIPath.Speed = speed;

        playerLocation = GameObject.Find("Player").GetComponent<Transform>();

        animator = GetComponent<Animator>();

        state = State.Walking;

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        GetComponent<AIPathFinding>().SetCanMoveToTrue();
    }

    private void Update()
    {
        switch (state)
        {
            case State.Walking:
                {
                    aIPath.Walking = true;

                    FindPlayer();

                    break;
                }

            case State.GoToPlayer:
                {
                    animator.SetTrigger("Walk");

                    aIPath.Walking = false;

                    if (aIPath.ToLocation == null)
                    {
                        aIPath.ToLocation = playerLocation;
                    }

                    float distance = Vector3.Distance(transform.position, new Vector3(playerLocation.position.x, playerLocation.position.y, transform.position.z));

                    if (distance <= DefaulData.slimeLittleAttackDistance)
                    {
                        state = State.Attack;

                        aIPath.ToLocation = null;
                    }
                    else if (distance >= DefaulData.maxDinstanceToCatch)
                    {
                        state = State.Walking;

                        aIPath.ToLocation = null;
                    }

                    break;
                }
            case State.Attack:
                {
                    aIPath.ToLocation = null;
                    aIPath.Walking = false;

                    animator.SetTrigger("Prepare");

                    float distance = Vector3.Distance(transform.position, new Vector3(playerLocation.position.x, playerLocation.position.y, transform.position.z));

                    if (distance > DefaulData.slimeLittleAttackDistance)
                    {
                        state = State.GoToPlayer;

                        animator.SetTrigger("Walk");
                    }

                    break;
                }
        }
    }
    
    public void IncrementRotation()
    {
        rotation++;

        audioSource.clip = rotateSound;

        audioSource.Play();

        if (rotation >= 2)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void ResetRotation()
    {
        rotation = 0;
    }

    public void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, new Vector3(playerLocation.position.x, playerLocation.position.y, transform.position.z)) <= DefaulData.slimeLittleAttackDistance && effect != null)
        {
            playerLocation.GetComponent<EffectHandler>().AddEffect(effect);
        }

        audioSource.clip = attackSound;

        audioSource.Play();
    }

    private void FindPlayer()
    {
        if (Vector3.Distance(transform.position, new Vector3(playerLocation.position.x, playerLocation.position.y, transform.position.z)) <= DefaulData.distanceToFind)
        {
            state = State.GoToPlayer;
        }
    }

    public void DestroyEnemyAnimation()
    {
        GetComponent<SpriteRenderer>().sortingOrder = -1;

        Destroy(GetComponent<BoxCollider2D>());

        gameObject.AddComponent<TimeDegradation>();

        GameObject.Find("Player").GetComponent<PlayerAchievements>().PotionKill++;

        Destroy(this);
    }
}

