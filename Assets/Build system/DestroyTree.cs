using UnityEngine;

public class DestroyTree : MonoBehaviour
{
    [SerializeField] private Item sapling;
    [SerializeField] private Item logItem;

    [SerializeField] private int minSaplingDrop;
    [SerializeField] private int maxSaplingDrop;

    [SerializeField] private int logAmountDrop;

    private SpawnItem spawnItem;

    [SerializeField] private GameObject treeDustWhenHitGround;

    private int spawn;

    public int Spawn { set { spawn = value; } }

    private void Awake()
    {
        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();
    }

    public void Destroy()
    {
        switch (spawn)
        {
            case 1:
                {
                    Vector3 position = GetComponentInParent<Transform>().position;

                    position.x -= 1f;

                    GameObject particles = Instantiate(treeDustWhenHitGround);
                    particles.transform.position = position;

                    particles.GetComponent<ParticleSystem>().Play();

                    spawnItem.SpawnItems(logItem, logAmountDrop, position);
                    spawnItem.SpawnItems(sapling, Random.Range(minSaplingDrop, maxSaplingDrop), position);

                    break;
                }

            default:
                {
                    Vector3 position = GetComponentInParent<Transform>().position;

                    position.x += 2f;

                    GameObject particles = Instantiate(treeDustWhenHitGround);
                    particles.transform.position = position;

                    particles.GetComponent<ParticleSystem>().Play();

                    spawnItem.SpawnItems(logItem, logAmountDrop, position);
                    spawnItem.SpawnItems(sapling, Random.Range(minSaplingDrop, maxSaplingDrop), position);

                    break;
                }
        }

        Destroy(this.gameObject);
    }
}