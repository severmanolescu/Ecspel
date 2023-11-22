using System.Collections;
using UnityEngine;

public class SwanAI : MonoBehaviour
{
    [SerializeField] private float minSecondsAnimationDuration;
    [SerializeField] private float maxSecondsAnimationDuration;

    [SerializeField] private float secondsToHug;

    [SerializeField] private float swimmingSpeed = 1f;

    [Range(0f, 100f)]
    [SerializeField] private int putHeadDownRate = 20;

    [Range(0, 24)]
    [SerializeField] private int hourToSleep = 20;

    [Range(0, 24)]
    [SerializeField] private int hourToWake = 6;

    private bool animationStarted = false;

    private bool moving = false;

    private bool headDown = false;

    private bool hug = false;
    private bool startedHugAction = false;
    private bool readyToHug = false;

    private bool sleeping = false;

    private Vector3 moveToLocation;

    private SwanAI partnerAI;

    private Animator animator;

    private BoxCollider2D boxCollider;

    private SpriteMask spriteMask;
    private SpriteRenderer spriteRenderer;

    private DayTimerHandler dayTimerHandler;

    public bool Hug { set => hug = value; }
    public SwanAI PartnerAI { set => partnerAI = value; }
    public bool ReadyToHug { get => readyToHug; }

    private void Start()
    {
        animator = GetComponent<Animator>();

        boxCollider = GetComponentInParent<BoxCollider2D>();

        spriteMask = GetComponent<SpriteMask>();   
        spriteRenderer = GetComponent<SpriteRenderer>();

        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();
    }

    IEnumerator WaitForAnimation(float duration)
    {
        animationStarted = true;

        yield return new WaitForSeconds(duration);

        animationStarted = false;

        SetAnimatorValues(false, false);
    }

    private void SetAnimatorValues(bool turnHead, bool swimming)
    {
        animator.SetBool("Turn_Head", turnHead);
        animator.SetBool("Swimming", swimming);
    }

    private void SetIdleAnimation(int state)
    {
        float duration = Random.Range(minSecondsAnimationDuration, maxSecondsAnimationDuration);

        switch (state)
        {
            case 1: SetAnimatorValues(true, false); break;
            case 2: StartMoving(); break;
            default: SetAnimatorValues(false, false); break;
        }

        if (state != 2)
        {
            StartCoroutine(WaitForAnimation(duration));
        }

    }

    public void ChangeIdleState()
    {
        if (!animationStarted && !moving && !startedHugAction && !sleeping)
        {
            if(dayTimerHandler.Hours >= hourToSleep)
            {
                if(partnerAI != null)
                {
                    GoToHugLocation();
                }
                else
                {
                    StartToSleep();
                }

                sleeping = true;
            }
            else if(hug == true)
            {
                startedHugAction = true;

                GoToHugLocation();

                SetAnimatorValues(false, true);
            }
            else if(!MoveHeadDown())
            {
                int state = Random.Range(0, 5);

                SetIdleAnimation(state);
            }
        }
    }

    private void StartToSleep()
    {
        SetAnimatorValues(true, false);
        animator.SetBool("Head_Down", false);
        animator.SetBool("Start_Sleeping", true);
    }

    private bool MoveHeadDown()
    {
        if(Random.Range(0,100) <= putHeadDownRate)
        {
            headDown = !headDown;

            animator.SetBool("Head_Down", headDown);

            return true;
        }

        return false;
    }

    private void StartMoving()
    {
        moving = true;

        StopAllCoroutines();

        SetAnimatorValues(false, true);

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

    private void GoToHugLocation()
    {
        SetAnimatorValues(false, true);

        animator.SetBool("Head_Down", true);

        if(transform.position.x > partnerAI.transform.position.x)
        {
            moveToLocation = new Vector3(boxCollider.transform.position.x - boxCollider.size.x / 2,
                                     boxCollider.transform.position.y,
                                     transform.position.z);
        }
        else
        {
            moveToLocation = new Vector3(boxCollider.transform.position.x + boxCollider.size.x / 2,
                                     boxCollider.transform.position.y,
                                     transform.position.z);
        }
        
        moving = true;

        ChangeDirection();  
    }

    private IEnumerator WaitForHug()
    {
        yield return new WaitForSeconds(secondsToHug);

        readyToHug = false;

        startedHugAction = false;

        hug = false;

        animator.SetBool("Head_Down", false);
        animator.SetBool("Head_Partner", false);
    }

    private void FixedUpdate()
    {
        spriteMask.sprite = spriteRenderer.sprite;

        if (sleeping == true && dayTimerHandler.Hours <= 12 && dayTimerHandler.Hours >= hourToWake)
        {
            SetAnimatorValues(false, false);
            animator.SetBool("Start_Sleeping", false);

            sleeping = false;

            hug = false;
        }

        if (moving)
        {
            Vector3 moveDir = (moveToLocation - transform.position).normalized;

            transform.position = transform.position + moveDir * swimmingSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, moveToLocation) <= 0.01)
            {
                moving = false;

                if(sleeping == true)
                {
                    StartToSleep();
                }
                else
                {
                    SetAnimatorValues(false, false);

                    if (startedHugAction == true)
                    {
                        readyToHug = true;
                    }
                }
            }
        }

        if(readyToHug == true && partnerAI.readyToHug)
        {
            animator.SetBool("Head_Partner", true);

            StartCoroutine(WaitForHug());
        }
    }
}
