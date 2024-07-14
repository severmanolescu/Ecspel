using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsSpawn : MonoBehaviour
{
    [Header("Time in seconds before despawn animals after Player leave the area")]
    [SerializeField] private int secondsBeforeDespawnAnimals = 120;

    [SerializeField] private int minAnimalCount;
    [SerializeField] private int maxAnimalCount;

    [SerializeField] private List<GameObject> animalsPrefab = new List<GameObject>();

    private bool justSpawned = false;

    private List<GameObject> animals = new List<GameObject>();

    private BoxCollider2D boxCollider;

    private Coroutine waitBeforDespawn;

    private NightCheckForAnimals nightCheck;

    private bool birdsScared = false;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();

        nightCheck = GameObject.Find("Global/DayTimer").GetComponent<NightCheckForAnimals>();
    }

    private Vector3 GetRandomLocationInArea()
    {
        return new Vector3(Random.Range(transform.position.x - boxCollider.size.x / 2, transform.position.x + boxCollider.size.x / 2),
                           Random.Range(transform.position.y - boxCollider.size.y / 2, transform.position.y + boxCollider.size.y / 2),
                           0);
    }

    private GameObject SpawnAnimaInArea(int animalPrefabIndex)
    {
        GameObject newAnimal;

        newAnimal = Instantiate(animalsPrefab[animalPrefabIndex], GetRandomLocationInArea(), transform.rotation);

        newAnimal.transform.SetParent(transform);

        if(Random.Range(0, 100) % 2 == 0)
        {
            newAnimal.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            newAnimal.transform.localScale = new Vector3(-1, 1, 1);
        }

        return newAnimal;
    }

    private void SpawnAnimals()
    {
        int animalCount = Random.Range(minAnimalCount, maxAnimalCount);

        int animalPrefabIndex = Random.Range(0, animalsPrefab.Count - 1);

        while (animals.Count < animalCount)
        {
            animals.Add(SpawnAnimaInArea(animalPrefabIndex));
        }

        justSpawned = true;
    }

    public void RemoveAnimalFromList(GameObject animal)
    {
        if(animals != null)
        {
            animals.Remove(animal);
        }
    }

    private IEnumerator WaitBeforDespawn()
    {
        yield return new WaitForSeconds(secondsBeforeDespawnAnimals);

        justSpawned = false;

        DespawnAnimals();
    }

    private void DespawnAnimals()
    {
        foreach(GameObject animal in animals)
        {
            if(animal != null)
            {
                Destroy(animal);
            }
        }

        animals.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DrawDistance")) 
        {
            if(waitBeforDespawn != null)
            {
                StopCoroutine(waitBeforDespawn);
            }

            if (animals.Count == 0 && justSpawned == false && nightCheck.CheckIfSpawn())
            {
                SpawnAnimals();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("DrawDistance"))
        {
            if (animals.Count != 0)
            {
                if(gameObject.activeSelf == true)
                {
                    waitBeforDespawn = StartCoroutine(WaitBeforDespawn());
                }
            }
        }
    }

    public void ScareAllTheBirds(float playerPosition)
    {
        if (!birdsScared)
        {
            birdsScared = true;

            foreach (GameObject animal in animals)
            {
                if (animal != null)
                {
                    BirdAI birdAI = animal.GetComponent<BirdAI>();

                    if (birdAI != null)
                    {
                        birdAI.Scare(playerPosition);
                    }
                }
            }

            birdsScared = false;
        }
    }

}