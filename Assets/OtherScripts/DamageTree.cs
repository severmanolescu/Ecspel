using System.Collections.Generic;
using UnityEngine;

public class DamageTree : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float trunkHealth;

    [SerializeField] private int trunkItemDropNO;

    [SerializeField] private int treeLevel;
    [SerializeField] private int trunkLevel;
    [SerializeField] private Item logItem;

    [Header("Audio effects")]
    [SerializeField] private List<AudioClip> woodChop;

    [Header("Particles to play when tree is destoyed")]
    [SerializeField] private GameObject treeDustWhenDestroy;

    [SerializeField] private GameObject shadowToDestroy;
    [SerializeField] private GameObject sunShadow;

    [SerializeField] private bool canDestroy = true;

    private AudioSource audioSource;

    private Animator animator;

    private bool destroyed = false;

    private DestroyTree destroyTree;

    private SpawnItem spawnItem;

    private int startScaleX;
    private int startScaleY;

    private int scaleX;
    private int scaleY;

    public bool Destroyed { get => destroyed; set { destroyed = value; } }

    private void Awake()
    {
        destroyTree = GetComponentInChildren<DestroyTree>();

        animator = GetComponentInChildren<Animator>();

        audioSource = gameObject.GetComponent<AudioSource>();

        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();

        if (destroyTree == null)
        {
            destroyed = true;

            ChangeSunShadowLength();
        }
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
        startScaleY = this.startScaleY;
        scaleX = this.scaleX;
        scaleY = this.scaleY;
    }

    private void ChangeGridData()
    {
        Grid grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grid;

        GridNode gridNode = grid.GetGridObject(transform.position);

        if (gridNode != null)
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
    }

    public void TakeDamage(float damage, int spawn, int itemLevel)
    {
        if(canDestroy)
        {
            if (Destroyed == false)
            {
                if (itemLevel >= treeLevel)
                {
                    health -= damage;

                    PlayChopClip();
                }
            }
            else
            {
                if (itemLevel >= trunkLevel)
                {
                    health -= damage;

                    PlayChopClip();
                }
            }

            if (health <= 0)
            {
                if (Destroyed == false)
                {
                    destroyTree.Spawn = spawn;

                    switch (spawn)
                    {
                        case 1: animator.SetTrigger("Left"); break;
                        default: animator.SetTrigger("Right"); break;
                    }

                    ChangeSunShadowLength();

                    health = trunkHealth;

                    if (shadowToDestroy != null)
                    {
                        Destroy(shadowToDestroy);
                    }

                    Destroyed = true;
                }
                else
                {
                    spawnItem.SpawnItems(logItem, trunkItemDropNO, transform.position);

                    ChangeGridData();

                    GameObject particles = Instantiate(treeDustWhenDestroy);
                    particles.transform.position = transform.position;

                    particles.GetComponent<ParticleSystem>().Play();

                    Destroy(this.gameObject);
                }
            }
        }
    }

    private void ChangeSunShadowLength()
    {
        if (sunShadow != null)
        {
            Vector3 newScale = sunShadow.transform.localScale;

            newScale.y /= 2;

            sunShadow.transform.localScale = newScale;
        }
    }

    public void PlayChopClip()
    {
        audioSource.clip = woodChop[Random.Range(0, woodChop.Count)];
        audioSource.Play();
    }

    public void ChangeCrowDesroy()
    {
        if (destroyed == true)
        {
            Destroy(GetComponentInChildren<DestroyTree>().gameObject);

            ChangeSunShadowLength();

            if (shadowToDestroy != null)
            {
                Destroy(shadowToDestroy);
            }

            health = trunkHealth;
        }
    }
}
