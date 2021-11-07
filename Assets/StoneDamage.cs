using UnityEngine;

public class StoneDamage : MonoBehaviour
{
    [SerializeField] private GameObject stonePrefab;

    private ParticleSystem particle;

    [SerializeField] private float health;
    [SerializeField] private int stoneLevel;

    public int startScaleX;
    public int startScaleY;

    public int scaleX;
    public int scaleY;

    private void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    public void GetDataFromPosition(int startScaleX, int startScaleY, int scaleX, int scaleY)
    {
        this.startScaleX = startScaleX;
        this.startScaleY = startScaleY;
        this.scaleX = scaleX;
        this.scaleY = scaleY;
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

                    Grid<GridNode> grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grig;

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
