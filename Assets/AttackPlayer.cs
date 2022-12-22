using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private float attackPower;

    private float maxAttackDistance;

    private PlayerStats playerStats;

    public float MaxAttackDistance { set => maxAttackDistance = value; }

    private void Awake()
    {
        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();
    }

    public void AttackComplete()
    {
        if(Vector3.Distance(transform.position, playerStats.transform.position) < maxAttackDistance)
        {
            playerStats.Health -= attackPower;
        }
    }
}
