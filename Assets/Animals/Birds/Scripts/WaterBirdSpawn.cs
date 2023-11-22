using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBirdSpawn : MonoBehaviour
{
    [Header("Time in seconds before despawn animals after Player leave the area")]
    [SerializeField] private int secondsBeforeDespawnAnimals = 120;

    [Range(0f, 100)]
    [SerializeField] private int spawnRate = 10;

    [SerializeField] private List<GameObject> animals;

    [SerializeField] private RuntimeAnimatorController waterEffectAnimation;

    [SerializeField] private Transform spawnLocation;

    private GameObject spawnedAnimal;

    private BoxCollider2D boxCollider;

    private NightCheckForAnimals nightCheck;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        nightCheck = GameObject.Find("Global/DayTimer").GetComponent<NightCheckForAnimals>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DrawDistance"))
        {
            StopAllCoroutines();

            if(spawnedAnimal == null)
            {
                SpawnAnimal();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DrawDistance") && spawnedAnimal != null)
        {
            StartCoroutine(WaitBeforeDespawn());
        }
    }

    private IEnumerator WaitBeforeDespawn()
    {
        yield return new WaitForSeconds(secondsBeforeDespawnAnimals);

        if(spawnedAnimal != null)
        {
            Destroy(spawnedAnimal);

            Destroy(GetComponent<Animator>());

            spawnedAnimal = null;
        }
    }

    private Vector3 SpawnLocation()
    {
        return new Vector3(Random.Range(transform.position.x - boxCollider.size.x / 2, transform.position.x + boxCollider.size.x / 2),
                           Random.Range(transform.position.y - boxCollider.size.y / 2, transform.position.y + boxCollider.size.y / 2), 
                           0);
    }

    private bool CheckIfSpawn()
    {
        if(Random.Range(0, 100) <= spawnRate && nightCheck.CheckIfSpawn())
        {
            return true;
        }

        return false;
    }

    private void SpawnAnimal()
    {
        if (CheckIfSpawn())
        {
            int spawnAnimalIndex = Random.Range(0, animals.Count);

            spawnedAnimal = Instantiate(animals[spawnAnimalIndex], SpawnLocation(), transform.rotation);

            spawnedAnimal.transform.parent = spawnLocation;

            spawnedAnimal.name = "DuckAI";

            Animator animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = waterEffectAnimation;
        }
    }
}
