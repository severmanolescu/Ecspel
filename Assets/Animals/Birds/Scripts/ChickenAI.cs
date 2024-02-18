using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenAI : MonoBehaviour
{
    [SerializeField] private float minAnimationDuration = 2f;
    [SerializeField] private float maxAnimationDuration = 4f;

    [SerializeField] private float walkSpeed = 1f;

    [Range(0, 100)]
    [SerializeField] private int rateToEnterCoop = 5;

    [Header("Rooster data:")]
    [SerializeField] private bool rooster = false;

    [Range(0, 100)]
    [SerializeField] private int rateToCrow;

    [Header("Sound clips:")]
    [SerializeField] private List<AudioClip> roosterCrowing = new List<AudioClip>();
    [SerializeField] private List<AudioClip> eatingClips = new List<AudioClip>();
    [SerializeField] private List<AudioClip> movingClips = new List<AudioClip>();

    private Animator animator;

    private ChickenCoopHandler chickenCoopHandler;

    private AudioSource audioSource;

    private bool moving = false;
    private bool movingToCoop = false;
    private bool crowing = false;

    private float maxAudioVolume;

    private Vector3 moveToLocation;

    private void Start()
    {
        chickenCoopHandler = GetComponentInParent<ChickenCoopHandler>();

        audioSource = GetComponent<AudioSource>();

        animator = GetComponent<Animator>();
        
        maxAudioVolume = audioSource.volume;
    }

    public void ChangeIdleState()
    {
        if (!moving && !crowing)
        {
            if(Random.Range(0, 100) <= rateToEnterCoop)
            {
                MoveToCoop();
            }
            else if (rooster && Random.Range(0, 100) <= rateToCrow)
            {
                Crowing();
            }
            else
            {
                int state = Random.Range(0, 5);

                SetAnimation(state);
            }
        }
    }

    private void Crowing()
    {
        SetAnimatorValue(false, false, false, true);

        audioSource.clip = roosterCrowing[Random.Range(0, roosterCrowing.Count - 1)];

        audioSource.volume = maxAudioVolume;

        audioSource.Play();

        crowing = true;
    }

    public void MoveToCoop()
    {
        if(gameObject.activeSelf)
        {
            StopAllCoroutines();

            moving = true;
            movingToCoop = true;
            crowing = false;

            moveToLocation = chickenCoopHandler.EnterCoopArea.transform.position;

            audioSource.Stop();

            SetAnimatorValue(true, false, false);

            ChangeDirection();
        }
        else
        {
            moving = false;
            movingToCoop = false;
        }
    }

    private void SetAnimatorValue(bool walk, bool eating, bool turn_head, bool crowing = false)
    {
        animator.SetBool("Walk", walk);
        animator.SetBool("Eating", eating);
        animator.SetBool("Turn_Head", turn_head);
        if(rooster)
        {
            animator.SetBool("Crowing", crowing);
        }
    }

    private void SetAnimation(int state)
    {
        switch (state)
        {
            case 1:  StartMoving(); break;
            case 2:  Eating();  break;
            case 3:  SetAnimatorValue(false, false, true);  break;
            default: SetAnimatorValue(false, false, false); break;
        }

        if(state == 2 || state == 3)
        {
            float duration = Random.Range(minAnimationDuration, maxAnimationDuration);

            StartCoroutine(WaitForAnimation(duration));
        }
    }

    private void Eating()
    {
        SetAnimatorValue(false, true, false);

        audioSource.clip = eatingClips[Random.Range(0, eatingClips.Count - 1)];

        audioSource.volume = Random.Range(maxAudioVolume / 5, maxAudioVolume);
        audioSource.Play();
    }

    private IEnumerator WaitForAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);

        SetAnimatorValue(false, false, false);
    }

    private void GetNewLocation()
    {
        moveToLocation = new Vector3(Random.Range(chickenCoopHandler.SpawnArea.transform.position.x - chickenCoopHandler.SpawnArea.size.x / 2, chickenCoopHandler.SpawnArea.transform.position.x + chickenCoopHandler.SpawnArea.size.x / 2),
                                     Random.Range(chickenCoopHandler.SpawnArea.transform.position.y - chickenCoopHandler.SpawnArea.size.y / 2, chickenCoopHandler.SpawnArea.transform.position.y + chickenCoopHandler.SpawnArea.size.y / 2),
                                     transform.position.z);
    }

    private void ChangeDirection()
    {
        if (transform.position.x > moveToLocation.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void StartMoving()
    {
        if(chickenCoopHandler == null)
        {
            Start();
        }

        GetNewLocation();

        SetAnimatorValue(true, false, false);

        ChangeDirection();

        moving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(movingToCoop && collision != null && collision.gameObject.CompareTag("Entrance"))
        {
            chickenCoopHandler.EnterCoop(this);

            moving = false;
            movingToCoop = false;

            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if(crowing)
        {
            moving = false;

            if(!audioSource.isPlaying)
            {
                crowing = false;

                SetAnimatorValue(false, false, false);
            }
        }
        else if (moving)
        {
            Vector3 moveDir = (moveToLocation - transform.position).normalized;

            transform.position = transform.position + moveDir * walkSpeed * Time.deltaTime;

            if (Vector3.Distance(transform.position, moveToLocation) <= 0.05)
            {
                SetAnimatorValue(false, false, false);

                moving = false;
            }
        }
    }
}
