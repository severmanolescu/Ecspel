using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] private float attackPower;

    private float maxAttackDistance;

    private PlayerStats playerStats;

    [SerializeField] private Transform headSpawnLocation;
    [SerializeField] private GameObject headPrefab;

    public float MaxAttackDistance { set => maxAttackDistance = value; }

    private void Awake()
    {
        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();
    }

    public void AttackComplete()
    {
        if (Vector3.Distance(transform.position, playerStats.transform.position) < maxAttackDistance)
        {
            if ((transform.position.x < playerStats.transform.position.x && transform.localScale.x < 0) ||
               (transform.position.x > playerStats.transform.position.x && transform.localScale.x > 0))
            {
                playerStats.Health -= attackPower;
            }
        }
    }

    public void SpawnHead()
    {
        Instantiate(headPrefab, headSpawnLocation.position, headSpawnLocation.rotation);
    }
}
