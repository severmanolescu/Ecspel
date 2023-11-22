using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SwanSpawn : MonoBehaviour
{
    [Header("Time in seconds before despawn animals after Player leave the area")]
    [SerializeField] private int secondsBeforeDespawnAnimals = 120;

    [Header("Time in seconds before check again the chance for the swans to hug")]
    [SerializeField] private int secondsBeforeCheckHugs = 3;

    [SerializeField] private GameObject swanPrefab;

    [Range(0, 100)]
    [SerializeField] private int spawnRate = 20;

    [Range(0, 100)]
    [SerializeField] private int spawnRatePartner = 20;

    [Range(0, 100)]
    [SerializeField] private int swanHugRate = 20;

    [SerializeField] private BoxCollider2D spawnAreaMainSwan;
    [SerializeField] private Transform spawnLocationMainSwan;

    [SerializeField] private BoxCollider2D spawnAreaSecondSwan;
    [SerializeField] private Transform spawnLocationSecondSwan;

    [SerializeField] private RuntimeAnimatorController waterEffectAnimation;

    private SwanAI mainSwan;
    private SwanAI secondSwan;

    private Coroutine waitBeforeDestroy = null;

    private NightCheckForAnimals nightCheck;

    private void Start()
    {
        nightCheck = GameObject.Find("Global/DayTimer").GetComponent<NightCheckForAnimals>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DrawDistance"))
        {
            if(waitBeforeDestroy != null)
            {
                StopCoroutine(waitBeforeDestroy);

                waitBeforeDestroy = null;
            }

            if (mainSwan == null)
            {
                SpawnAnimals();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DrawDistance") && mainSwan != null)
        {
            waitBeforeDestroy = StartCoroutine(WaitBeforDestroy());
        }
    }

    private Vector3 SpawnLocation(BoxCollider2D spawnArea)
    {
        return new Vector3(Random.Range(spawnArea.transform.position.x - spawnArea.size.x / 2, spawnArea.transform.position.x + spawnArea.size.x / 2),
                           Random.Range(spawnArea.transform.position.y - spawnArea.size.y / 2, spawnArea.transform.position.y + spawnArea.size.y / 2),
                           0);
    }

    private GameObject InstantiatePrefab(Transform objectParent, BoxCollider2D spawnArea)
    {
        GameObject swan = Instantiate(swanPrefab, SpawnLocation(spawnArea), transform.rotation);

        swan.transform.parent = objectParent;

        swan.name = "SwanAI";

        return swan;
    }

    private void SpawnAnimals()
    {
        if(Random.Range(0, 100) <= spawnRate && nightCheck.CheckIfSpawn())
        {
            mainSwan = InstantiatePrefab(spawnLocationMainSwan, spawnAreaMainSwan).GetComponent<SwanAI>();

            Animator waterEffectAnimator = spawnAreaMainSwan.AddComponent<Animator>();

            waterEffectAnimator.runtimeAnimatorController = waterEffectAnimation;

            if (Random.Range(0, 100) <= spawnRatePartner)
            {
                secondSwan = InstantiatePrefab(spawnLocationSecondSwan, spawnAreaSecondSwan).GetComponent<SwanAI>();

                waterEffectAnimator = spawnAreaSecondSwan.AddComponent<Animator>();

                waterEffectAnimator.runtimeAnimatorController = waterEffectAnimation;

                mainSwan.PartnerAI = secondSwan;
                secondSwan.PartnerAI = mainSwan;

                StartCoroutine(CheckForHug());
            }
        }
    }

    private IEnumerator WaitBeforDestroy()
    {
        yield return new WaitForSeconds(secondsBeforeDespawnAnimals);

        if(mainSwan != null)
        {
            Destroy(mainSwan.gameObject);
        }
        if(secondSwan != null)
        {
            Destroy(secondSwan.gameObject);
        }

        Destroy(spawnAreaMainSwan.GetComponent<Animator>());
        Destroy(spawnAreaSecondSwan.GetComponent<Animator>());

        StopAllCoroutines();
    }

    private IEnumerator CheckForHug()
    {
        while(true)
        {
            yield return new WaitForSeconds(secondsBeforeCheckHugs);

            if( Random.Range(0, 100) <= swanHugRate)
            {
                mainSwan.Hug = secondSwan.Hug = true;

                yield return new WaitForSeconds(60);
            }
        }
    }
}
