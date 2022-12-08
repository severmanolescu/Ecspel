using System.Collections;
using UnityEngine;

public class GroundWoodDamage : MonoBehaviour
{
    [SerializeField] private Item log;

    [SerializeField] private GameObject itemWorld;

    [SerializeField] private int level;

    [SerializeField] private GameObject destroyParticle;

    [Header("Audio effects")]
    [SerializeField] private AudioClip woodChop;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void ChangeGridData(GridNode gridNode, Grid<GridNode> grid)
    {
        for (int i = gridNode.x; i <= gridNode.x + 1; i++)
        {
            for (int j = gridNode.y; j <= gridNode.y + 1; j++)
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

    public void AxeDestroy(int level)
    {
        if (level >= this.level)
        {
            audioSource.clip = woodChop;
            audioSource.Play();

            ItemWorld newItem = Instantiate(itemWorld).GetComponent<ItemWorld>();

            newItem.transform.position = transform.position;

            Item newLog = Instantiate(log);

            newLog.Amount = 1;

            newItem.SetItem(newLog);
            newItem.MoveToPoint();

            Grid<GridNode> grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grid;

            GridNode gridNode = grid.GetGridObject(transform.position);

            if (gridNode != null)
            {
                ChangeGridData(gridNode, grid);
            }

            GameObject particles = Instantiate(destroyParticle);
            particles.transform.position = transform.position;
            particles.GetComponent<ParticleSystem>().Play();

            StartCoroutine(WaitForClip());
        }
    }

    IEnumerator WaitForClip()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;

        GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
