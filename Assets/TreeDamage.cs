using System.Collections;
using UnityEngine;

public class TreeDamage : MonoBehaviour
{
    [SerializeField] private GameObject stickPrefab;
    [SerializeField] private GameObject prefabSpawnLocation1;
    [SerializeField] private GameObject prefabSpawnLocation2;

    private Animator animator;
    private ParticleStart particleStart;

    private int health;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        particleStart = GetComponentInChildren<ParticleStart>();
    }

    private void Start()
    {
        health = DefaulData.treeHealth;
    }

    IEnumerator Wait(Rigidbody2D rigidbody)
    {
        rigidbody.gravityScale = DefaulData.stickGravity;

        yield return new WaitForSeconds(.75f);

        rigidbody.gravityScale = 0f;
        rigidbody.velocity = Vector2.zero;
    }

    public void TakeDamage(int damage, int spawn)
    {
        health -= damage;

        if(particleStart != null)
        {
            particleStart.StartParticles();

            animator.SetTrigger("Damage");
        }

        if (health <= 0)
        {
            if (particleStart != null)
            {
                particleStart.Spawn = spawn;
                
                switch(spawn)
                {
                    case 1:   animator.SetTrigger("Left"); break;
                    case 2:   animator.SetTrigger("Right"); break;
                    default:  animator.SetTrigger("Right"); particleStart.Spawn = 2; break;
                }

                GameObject.Find("Player").GetComponent<PlayerAchievements>().Trees++;
            }
        }
        else
        {
            if (Random.Range(0, 100) < DefaulData.stickSpawnRate)
            {
                GameObject stick;

                if (Random.Range(0, 3) < 1)
                {
                    stick = Instantiate(stickPrefab, prefabSpawnLocation1.transform.position, prefabSpawnLocation1.transform.rotation);
                }
                else
                {
                    stick = Instantiate(stickPrefab, prefabSpawnLocation2.transform.position, prefabSpawnLocation2.transform.rotation);
                }

                stick.GetComponent<ItemWorld>().SetItem(DefaulData.GetItemWithAmount(DefaulData.stick, 1));

                StartCoroutine(Wait(stick.GetComponent<Rigidbody2D>()));
            }
        }
    }
}
