using System.Collections;
using UnityEngine;

public class DuckAI : MonoBehaviour
{
    [SerializeField] private float minSecondsAnimationDuration;
    [SerializeField] private float maxSecondsAnimationDuration;

    [SerializeField] private float swimmingSpeed = 1f;

    [Range(0, 24)]
    [SerializeField] private int hourToSleep = 20;
    [Range(0, 24)]
    [SerializeField] private int hourToWake = 6;

    private Animator animator;

    private BoxCollider2D boxCollider;

    private DayTimerHandler dayTimerHandler;

    private SpriteMask mask;
    private SpriteRenderer spriteRenderer;

    private bool animationStarted = false;

    private bool moving = false;

    private bool sleeping = false;

    private Vector3 moveToLocation;

    private void Start()
    {
        animator = GetComponent<Animator>();

        boxCollider = GetComponentInParent<BoxCollider2D>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        mask = GetComponent<SpriteMask>();

        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();
    }

    IEnumerator WaitForAnimation(float duration)
    {
        animationStarted = true;

        yield return new WaitForSeconds(duration);

        animationStarted = false;

        SetAnimatorValues(false, false, false);
    }

    private void SetAnimatorValues(bool headUnderwater, bool turnHead, bool swimming)
    {
        animator.SetBool("Head_Underwater", headUnderwater);
        animator.SetBool("Turn_Head", turnHead);
        animator.SetBool("Swimming", swimming);
    }

    private void SetIdleAnimation(int state)
    {
        switch (state)
        {
            case 1: case 2: SetAnimatorValues(true, false, false); break;
            case 3: case 4: SetAnimatorValues(false, true, false); break;
            case 5: StartMoving(); break;
            default: SetAnimatorValues(false, false, false); break;
        }

        if(state != 5)
        {
            float duration = Random.Range(minSecondsAnimationDuration, maxSecondsAnimationDuration);

            StartCoroutine(WaitForAnimation(duration));
        }

    }

    public void ChangeIdleState()
    {
        if (!animationStarted && !moving && !sleeping)
        {
            if(dayTimerHandler.Hours >= hourToSleep)
            {
                SetAnimatorValues(false, true, false);
                animator.SetBool("Start_Sleeping", true);

                sleeping = true;
            }
            else
            {
                int state = Random.Range(0, 6);

                SetIdleAnimation(state);
            }
        }
    }

    private void StartMoving()
    {
        moving = true; 

        StopAllCoroutines(); 

        SetAnimatorValues(false, false, true); 

        GetNewLocation();

        ChangeDirection();
    }

    private void ChangeDirection()
    {
        if (transform.position.x > moveToLocation.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void GetNewLocation()
    {
        moveToLocation = new Vector3(Random.Range(boxCollider.transform.position.x - boxCollider.size.x / 2, boxCollider.transform.position.x + boxCollider.size.x / 2),
                                     Random.Range(boxCollider.transform.position.y - boxCollider.size.y / 2, boxCollider.transform.position.y + boxCollider.size.y / 2), 
                                     transform.position.z);
    }

    private void FixedUpdate()
    {
        mask.sprite = spriteRenderer.sprite;

        if (sleeping == true && dayTimerHandler.Hours <= 12 && dayTimerHandler.Hours >= hourToWake)
        {
            SetAnimatorValues(false, false, false);
            animator.SetBool("Start_Sleeping", false);

            sleeping = false;
        }

        if(moving)
        {
            Vector3 moveDir = (moveToLocation - transform.position).normalized;

            transform.position = transform.position + moveDir * swimmingSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, moveToLocation) <= 0.01)
            {
                SetAnimatorValues(false, false, false);

                moving = false;
            }
        }
    }
}
