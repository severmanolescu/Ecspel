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

    private SpawnItem spawnItem;

    private AudioSource audioSource;

    private void Awake()
    {
        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();

        audioSource = GetComponent<AudioSource>();
    }

    public void AxeDestroy(int level)
    {
        if (level >= this.level)
        {
            audioSource.clip = woodChop;
            audioSource.Play();

            spawnItem.SpawnItems(log, 1, transform.position);


            Grid grid = GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().Grid;

            grid.ReinitializeGrid(transform.position);

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
