using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy stats")]
    [SerializeField] private float maxHealth;

    [Header("Audio effects")]
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private AudioClip dieClip;

    private AudioSource audioSource;

    private Animator animator;

    private SpriteRenderer enemySprite;

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
        health -= damage;

        if (health <= 0)
        {
            DestroyEnemy();
        }
        else
        {
            audioSource.clip = damageClip;
            audioSource.Play();

            StartCoroutine(WaitDamage());
        }
    }

    private void DestroyEnemy()
    {
        animator.SetTrigger("Explode");

        audioSource.clip = dieClip;
        audioSource.Play();
    }
}
