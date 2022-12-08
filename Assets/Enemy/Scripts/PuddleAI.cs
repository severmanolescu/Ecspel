using UnityEngine;

public class PuddleAI : MonoBehaviour
{
    enum State
    {
        IdleIn,
        GetOut,
        IdleOut,
        GetIn,
        Attack,
    }

    [Header("Puddle stats")]
    [SerializeField] private AudioClip attackSound;

    [SerializeField] private float distanceToGetOut = 1f;
    [SerializeField] private float distanceToAttack = 0.5f;

    [SerializeField] private float attackPower = 3f;

    [SerializeField] private float attackSpeed = 1;

    private float nextAttackTime;

    private State state;

    private Transform playerLocation;

    private Animator animator;

    private AudioSource audioSource;

    private void Awake()
    {
        playerLocation = GameObject.Find("Player").GetComponent<Transform>();

        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        state = State.IdleIn;

        nextAttackTime = attackSpeed;
    }

    private void Update()
    {
        switch (state)
        {
            case State.IdleIn:
                {
                    FindPlayer();

                    break;
                }

            case State.IdleOut:
                {
                    float distance = Vector3.Distance(transform.position, new Vector3(playerLocation.position.x, playerLocation.position.y, transform.position.z));

                    if (distance <= distanceToAttack)
                    {
                        state = State.Attack;
                    }
                    else if (distance >= distanceToGetOut)
                    {
                        state = State.IdleIn;

                        animator.SetBool("Player", false);
                    }

                    break;
                }
            case State.Attack:
                {
                    if (Time.time > nextAttackTime)
                    {
                        animator.SetTrigger("Attack");

                        nextAttackTime = Time.time + attackSpeed;
                    }

                    float distance = Vector3.Distance(transform.position, new Vector3(playerLocation.position.x, playerLocation.position.y, transform.position.z));

                    if (distance > distanceToAttack && distance <= distanceToGetOut)
                    {
                        state = State.IdleOut;
                    }

                    if (distance > distanceToGetOut)
                    {
                        state = State.IdleIn;

                        animator.SetBool("Player", false);
                    }

                    break;
                }
        }
    }

    public void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, new Vector3(playerLocation.position.x, playerLocation.position.y, transform.position.z)) <= distanceToAttack)
        {
            playerLocation.GetComponent<PlayerStats>().Health -= attackPower;
        }

        audioSource.clip = attackSound;

        audioSource.Play();
    }

    private void FindPlayer()
    {
        if (Vector3.Distance(transform.position, new Vector3(playerLocation.position.x, playerLocation.position.y, transform.position.z)) <= distanceToGetOut)
        {
            state = State.IdleOut;

            animator.SetBool("Player", true);
        }
    }

    public void DestroyEnemyAnimation()
    {
        GetComponent<EnemyDropLoot>().DropItem();

        GetComponent<SpriteRenderer>().sortingOrder = -1;

        Destroy(GetComponent<BoxCollider2D>());
        gameObject.AddComponent<TimeDegradation>();

        GameObject.Find("Player").GetComponent<PlayerAchievements>().PuddleKill++;

        Destroy(this);
    }
}
