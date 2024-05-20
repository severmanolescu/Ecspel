using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows;

public class BirdAI : MonoBehaviour
{
    [SerializeField] private float minFlySpeed = 1.0f;
    [SerializeField] private float maxFlySpeed = 1.0f;

    [SerializeField] private int minSecondsAnimationDuration = 5;
    [SerializeField] private int maxSecondsAnimationDuration = 10;

    [SerializeField] private float minAltitude = 1;
    [SerializeField] private float maxAltitude = 1.5f;

    [SerializeField] private GameObject dustParticlePrefab;

    [Range(0, 100)]
    [SerializeField] private int changeToSpawnFeather = 10;
    [SerializeField] private Item feather;

    private bool animationStarted = false;

    private bool scared = false;

    private float flySpeed;

    private Transform playerTransform;

    private Animator animator;

    private Vector3 newPosition = Vector3.zero;

    private void Start()
    {
        animator = GetComponent<Animator>();

        flySpeed = Random.Range(minFlySpeed, maxFlySpeed);

        playerTransform = GameObject.Find("Global/Player").transform;

        GameObject.Find("Global/DayTimer").GetComponent<NightCheckForAnimals>().AddBirds(this);
    }

    IEnumerator WaitForAnimation(float duration)
    {
        animationStarted = true;

        yield return new WaitForSeconds(duration);

        animationStarted = false;

        SetAnimatorValues(false, false);
    }

    private void SetAnimatorValues(bool eating, bool turnHead)
    {
        animator.SetBool("Eating", eating);
        animator.SetBool("Turn_Head", turnHead);
    }

    private void SetIdleAnimation(int state)
    {
        float duration = Random.Range(minSecondsAnimationDuration, maxSecondsAnimationDuration);

        switch(state)
        {
            case 1: case 2: SetAnimatorValues(true, false); break;
            case 3:         SetAnimatorValues(false, true); break;
            default:        SetAnimatorValues(false, false); break;
        }

        StartCoroutine(WaitForAnimation(duration));
    }

    public void ChangeIdleState()
    {
        if (!animationStarted && !scared)
        {
            int state = Random.Range(0, 4);

            SetIdleAnimation(state);
        }
    }

    private Direction GetPlayerDirection(float playerPosition)
    {
        if(playerPosition < transform.position.x)
        {
            return Direction.Left;
        }

        return Direction.Right;
    }

    private void SetFacedDirection(Direction direction)
    {
        if(direction == Direction.Left)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    private void GetNewPositionStart(Direction direction)
    {
        float altitude = Random.Range(minAltitude, maxAltitude);

        switch(direction)
        {
            case Direction.Left:
            {
                    newPosition = transform.position + new Vector3(altitude * 100, altitude, 0);

                    break;
            }
            case Direction.Right:
            {
                    newPosition = transform.position - new Vector3(altitude * 100, altitude, 0);

                    break;
            }
        }
    }

    private void SpawnDustParticle()
    {
        Instantiate(dustParticlePrefab, transform.position, transform.rotation);
    }

    private void SpawnFeather()
    {
        if(Random.Range(0, 100) <= changeToSpawnFeather)
        {
            GameObject.Find("Global").GetComponent<SpawnItem>().SpawnItems(feather, 1, transform.position);
        }
    }

    public void Scare(float playerPosition)
    {
        if (!scared)
        {
            scared = true;

            SpawnFeather();

            StopAllCoroutines();

            SpawnDustParticle();

            Direction direction = GetPlayerDirection(playerPosition);

            SetFacedDirection(direction);

            animator.SetBool("Fly", true);

            GetNewPositionStart(direction);

            GetComponent<SpriteRenderer>().sortingOrder = 50;

            BirdsSpawn birdsSpawn = GetComponentInParent<BirdsSpawn>();

            if (birdsSpawn != null)
            {
                birdsSpawn.ScareAllTheBirds(playerPosition);
            }
        }
    }

    public void Sleep()
    {
        Scare(playerTransform.position.x);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && (collision.CompareTag("Player") || collision.CompareTag("NPC")))
        {
            playerTransform = collision.transform;

            Scare(collision.transform.position.x);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DrawDistance") && scared == true)
        {
            DespawnAnimal();
        }
    }

    public void DespawnAnimal()
    {
        BirdsSpawn animalSpawn = GetComponentInParent<BirdsSpawn>();

        if (animalSpawn != null)
        {
            animalSpawn.RemoveAnimalFromList(gameObject);
        }

        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if(scared && newPosition != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPosition, flySpeed * Time.fixedDeltaTime);
        }
    }
}
