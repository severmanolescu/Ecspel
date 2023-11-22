using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FishSpawn : MonoBehaviour
{
    [Header("Time in seconds before despawn animals after Player leave the area")]
    [SerializeField] private int secondsBeforeDespawnAnimals = 120;

    [SerializeField] private List<GameObject> fishPrefab = new List<GameObject>();

    [SerializeField] private int minSpawnNumber = 3;
    [SerializeField] private int maxSpawnNumber = 3;

    private List<GameObject> fishes = new List<GameObject>();

    private BoxCollider2D boxCollider;

    private bool justSpawned = false;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DrawDistance"))
        {
             StopAllCoroutines();

            if (fishes.Count == 0 && justSpawned == false)
            {
                SpawnAnimals();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DrawDistance"))
        {
            if (fishes.Count != 0)
            {
                StartCoroutine(WaitBeforDespawn());
            }
        }
    }

    private Vector3 GetRandomLocationInArea()
    {
        return new Vector3(Random.Range(transform.position.x - boxCollider.size.x / 2, transform.position.x + boxCollider.size.x / 2),
                           Random.Range(transform.position.y - boxCollider.size.y / 2, transform.position.y + boxCollider.size.y / 2),
                           0);
    }

    private void SpawnAnimals()
    {
        int animalCount = Random.Range(minSpawnNumber, maxSpawnNumber);

        while (fishes.Count < animalCount)
        {
            int animalPrefabIndex = Random.Range(0, fishPrefab.Count);

            fishes.Add(SpawnAnimaInArea(animalPrefabIndex));
        }

        justSpawned = true;
    }

    private GameObject SpawnAnimaInArea(int animalPrefabIndex)
    {
        GameObject newAnimal;

        newAnimal = Instantiate(fishPrefab[animalPrefabIndex], GetRandomLocationInArea(), transform.rotation);

        newAnimal.transform.SetParent(transform);

        if (Random.Range(0, 100) % 2 == 0)
        {
            newAnimal.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            newAnimal.transform.localScale = new Vector3(-1, 1, 1);
        }
        return newAnimal;
    }

    private IEnumerator WaitBeforDespawn()
    {
        yield return new WaitForSeconds(secondsBeforeDespawnAnimals);

        justSpawned = false;

        DespawnAnimals();
    }

    private void DespawnAnimals()
    {
        foreach (GameObject animal in fishes)
        {
            if (animal != null)
            {
                Destroy(animal);
            }
        }

        fishes.Clear();
    }
}
