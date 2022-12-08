using UnityEngine;

public class DestroyTree : MonoBehaviour
{
    [SerializeField] private Item sapling;
    [SerializeField] private Item logItem;

    [SerializeField] private int minSaplingDrop;
    [SerializeField] private int maxSaplingDrop;

    [SerializeField] private int logAmountDrop;

    [SerializeField] private GameObject itemWorld;

    [SerializeField] private GameObject treeDustWhenHitGround;

    private int spawn;

    public int Spawn { set { spawn = value; } }
    public GameObject ItemWorld { get { return itemWorld; } }

    public void Destroy()
    {
        switch (spawn)
        { 
            case 1:
                {
                    ItemWorld item = Instantiate(itemWorld).GetComponent<ItemWorld>();

                    Vector3 position = GetComponentInParent<Transform>().position;

                    position.x -= 1f;

                    GameObject particles = Instantiate(treeDustWhenHitGround);
                    particles.transform.position = position;

                    particles.GetComponent<ParticleSystem>().Play();

                    item.transform.position = position;

                    item.SetItem(DefaulData.GetItemWithAmount(logItem, logAmountDrop));
                    item.MoveToPoint();

                    item = Instantiate(itemWorld).GetComponent<ItemWorld>();

                    item.transform.position = position;

                    item.SetItem(DefaulData.GetItemWithAmount(sapling, Random.Range(minSaplingDrop, maxSaplingDrop)));
                    item.MoveToPoint();

                    break;
                }

            default:
                {
                    ItemWorld game = Instantiate(itemWorld).GetComponent<ItemWorld>();

                    Vector3 position = GetComponentInParent<Transform>().position;

                    position.x += 2f;

                    GameObject particles = Instantiate(treeDustWhenHitGround);
                    particles.transform.position = position;

                    particles.GetComponent<ParticleSystem>().Play();

                    game.transform.position = position;

                    game.SetItem(DefaulData.GetItemWithAmount(logItem, logAmountDrop));
                    game.MoveToPoint();

                    game = Instantiate(itemWorld).GetComponent<ItemWorld>();

                    game.transform.position = position;

                    game.SetItem(DefaulData.GetItemWithAmount(sapling, Random.Range(minSaplingDrop, maxSaplingDrop)));
                    game.MoveToPoint();

                    break;
                }
        }

        Destroy(this.gameObject);
    }
}
