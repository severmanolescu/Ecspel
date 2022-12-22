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
