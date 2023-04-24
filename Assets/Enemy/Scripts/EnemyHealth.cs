using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy stats")]
    [SerializeField] private float maxHealth;

    [Header("Audio effects")]
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip dieClip;

    [SerializeField] private GameObject fadeObject;
    [SerializeField] private Sprite dieSprite;

    private AudioSource audioSource;

    private Animator animator;

    private SpriteRenderer enemySprite;

    private bool die = false;

    private float health;

    private void Awake()
    {
        health = maxHealth;

        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>();

        enemySprite = GetComponent<SpriteRenderer>();
    }

    IEnumerator WaitDamage()
    {
        enemySprite.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        enemySprite.color = Color.white;
    }

    public void TakeDamage(float damage)
    {
        if (die == false)
        {
            health -= damage;

            if (health <= 0)
            {
                die = true;

                DestroyEnemy();
            }
            else
            {
                audioSource.clip = damageClip;
                audioSource.Play();

                StartCoroutine(WaitDamage());
            }
        }
    }

    private void DestroyEnemy()
    {
        animator.SetTrigger("Dead");

        audioSource.clip = dieClip;
        audioSource.Play();
    }

    public void DestroyEnemyDie()
    {
        if (dieSprite != null && fadeObject != null)
        {
            GameObject newObject = Instantiate(fadeObject, transform.position, transform.rotation);

            newObject.transform.localScale = transform.localScale;

            newObject.GetComponent<SpriteRenderer>().sprite = dieSprite;

            SpawnLittleEnemy spawnLittle = GetComponent<SpawnLittleEnemy>();

            if (spawnLittle != null)
            {
                spawnLittle.SpawnEnemy();
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
