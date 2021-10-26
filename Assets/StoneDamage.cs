using UnityEngine;

public class StoneDamage : MonoBehaviour
{
    [SerializeField] private GameObject stonePrefab;

    private ParticleSystem particle;

    private int health;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    private void Start()
    {
        health = DefaulData.stoneHealth;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        particle.Play();

        if(health <= 0)
        {
            GameObject stone = Instantiate(stonePrefab, transform.position, transform.rotation);

            ItemWorld itemWorld = stone.GetComponent<ItemWorld>();

            if(itemWorld != null)
            {
                itemWorld.SetItem(DefaulData.GetItemWithAmount(DefaulData.stone, 2));

                Destroy(this.gameObject);
            }

            GameObject.Find("Player").GetComponent<PlayerAchievements>().Stones++;
        }
    }
}
