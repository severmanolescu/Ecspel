using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StoneDamage : MonoBehaviour
{
    [SerializeField] private GameObject stonePrefab;

    [SerializeField] private Item spawnItem;
    [SerializeField] private int amount;

    [SerializeField] private float health;
    [SerializeField] private int stoneLevel;

    [SerializeField] private GameObject destroyParticle;

    [Header("Audio effects")]
    [SerializeField] private AudioClip soundEffect;
    [SerializeField] private AudioClip soundEffectNotLevel;

    private AudioSource audioSource;

    private int startScaleX;
    private int startScaleY;

    private int scaleX;
    private int scaleY;

    private void Awake()
    {
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
        startScaleY = this.startScaleY;
        scaleX = this.scaleX;
        scaleY = this.scaleY;
    }

    private IEnumerator WaitForSoundEffect()
    {
        Destroy(GetComponent<SpriteRenderer>());

        Destroy(GetComponent<BoxCollider2D>());

        Destroy(GetComponent<ShadowCaster2D>());

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites != null && sprites.Length > 1)
        {
            Destroy(sprites[1].gameObject);
        }

        while (audioSource.isPlaying)
        {
            yield return null;

            yield return new WaitForSeconds(10);
        }

        Destroy(this.gameObject);
    }

    public void TakeDamage(float damage, int level)
    {
        if (level >= stoneLevel)
        {
            audioSource.clip = soundEffect;
            audioSource.Play();

            health -= damage;

            if (health <= 0)
            {
                GameObject stone = Instantiate(stonePrefab, transform.position, transform.rotation);

                ItemWorld itemWorld = stone.GetComponent<ItemWorld>();

                if (itemWorld != null)
                {
                    Item copyItem = spawnItem.Copy();

                    itemWorld.SetItem(DefaulData.GetItemWithAmount(copyItem, amount));

                    itemWorld.MoveToPoint();

                    itemWorld.transform.parent = transform.parent;

                    Grid grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grid;

                    GridNode gridNode = grid.GetGridObject(transform.position);

                    grid.ReinitializeGrid(gridNode);

                    GameObject.Find("Player").GetComponent<PlayerAchievements>().Stones++;

                    IncreseCaveObjects increseCave = GetComponent<IncreseCaveObjects>();

                    if (increseCave != null)
                    {
                        increseCave.Increse();
                    }

                    GameObject particles = Instantiate(destroyParticle);
                    particles.transform.position = transform.position;
                    particles.GetComponent<ParticleSystem>().Play();

                    StartCoroutine(WaitForSoundEffect());
                }
            }
        }
        else
        {
            audioSource.clip = soundEffectNotLevel;
            audioSource.Play();
        }
    }
}
