using UnityEngine;

public class StoneDamage : MonoBehaviour
{
    [SerializeField] private GameObject stonePrefab;

    [SerializeField] private Item spawnItem;
    [SerializeField] private int amount;

    private ParticleSystem particle;

    [SerializeField] private float health;
    [SerializeField] private int stoneLevel;

    [Header("Audio effects")]
    [SerializeField] private AudioClip soundEffect;

    private AudioSource audioSource;

    private int startScaleX;
    private int startScaleY;

    private int scaleX;
    private int scaleY;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();

        audioSource = GetComponent<AudioSource>();
    }

    public void GetDataFromPosition(int startScaleX, int startScaleY, int scaleX, int scaleY)
    {
        this.startScaleX = startScaleX;
        this.startScaleY = startScaleY;
        this.scaleX = scaleX;
        this.scaleY = scaleY;
    }

    public void GetData(out int startScaleX, out int startScaleY, out int scaleX, out int scaleY)
    {
        startScaleX = this.startScaleX;
        startScaleY= this.startScaleY;
        scaleX = this.scaleX;
        scaleY = this.scaleY;
    }

    private void ChangeGridData(GridNode gridNode, Grid<GridNode> grid)
    {
        for (int i = gridNode.x + startScaleX; i <= gridNode.x + scaleX; i++)
        {
            for (int j = gridNode.y + startScaleY; j <= gridNode.y + scaleY; j++)
            {
                if (grid.gridArray[i, j] != null)
                {
                    grid.gridArray[i, j].canPlace = true;
                    grid.gridArray[i, j].canPlant = false;
                    grid.gridArray[i, j].isWalkable = true;
                    grid.gridArray[i, j].objectInSpace = null;
                }
            }
        }
    }

    public void TakeDamage(float damage, int level)
    {
        if (level >= stoneLevel)
        {
            audioSource.clip = soundEffect;
            audioSource.Play();

            health -= damage;

            particle.Play();

            if (health <= 0)
            {
                GameObject stone = Instantiate(stonePrefab, transform.position, transform.rotation);

                ItemWorld itemWorld = stone.GetComponent<ItemWorld>();

                if (itemWorld != null)
                {
                    Item copyItem = spawnItem.Copy();

                    itemWorld.SetItem(DefaulData.GetItemWithAmount(copyItem, amount));

                    itemWorld.MoveToPoint();

                    Grid<GridNode> grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grid;

                    GridNode gridNode = grid.GetGridObject(transform.position);

                    if (gridNode != null)
                    {
                        ChangeGridData(gridNode, grid);
                    }

                    Destroy(this.gameObject);
                }

                GameObject.Find("Player").GetComponent<PlayerAchievements>().Stones++;
            }
        }
    }
}
