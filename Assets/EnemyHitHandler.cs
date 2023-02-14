using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitHandler : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        enemyHealth = GetComponentInParent<EnemyHealth>();
    }

    public void AttackEnemy(float damage)
    {
        enemyHealth.TakeDamage(damage);
    }
}
