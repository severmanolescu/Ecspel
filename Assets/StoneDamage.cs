using UnityEngine;

public class StoneDamage : MonoBehaviour
{
    [SerializeField] private GameObject stonePrefab;

    private ParticleSystem particle;

    [SerializeField] private float health;
    [SerializeField] private int stoneLevel;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public void TakeDamage(float damage, int level)
    {
        if (level >= stoneLevel)
        { 
            health -= damage;

            particle.Play();

            if (health <= 0)
            {
                GameObject stone = Instantiate(stonePrefab, transform.position, transform.rotation);

                ItemWorld itemWorld = stone.GetComponent<ItemWorld>();

                if (itemWorld != null)
                {
                    itemWorld.SetItem(DefaulData.GetItemWithAmount(DefaulData.stone, 2));

                    itemWorld.MoveToPoint();

                    Destroy(this.gameObject);
                }

                GameObject.Find("Player").GetComponent<PlayerAchievements>().Stones++;
            }
        }
    }
}
