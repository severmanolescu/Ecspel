using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float maxHealth;

    private Animator animator;

    private float health;

    private void Awake()
    {
        health = maxHealth;

        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(5);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if(health <= 0)
        {
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        animator.SetTrigger("Explode");
    }
}
