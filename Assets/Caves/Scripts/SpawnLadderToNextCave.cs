using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLadderToNextCave : MonoBehaviour
{
    [SerializeField] private GameObject ladderObject;

    private CaveSystemHandler caveSystem;

    private int noOfObjects = 0;
    private int noOfDestroyedObjects = 0;

    private bool spawned = false;

    private void Awake()
    {
        caveSystem = GameObject.Find("Caves").GetComponentInParent<CaveSystemHandler>();

        noOfObjects = GetComponentsInChildren<Rigidbody2D>().Length;
    }

    public void IncredeDestroyedObjects(Vector3 position)
    {
        noOfDestroyedObjects++;

        if (caveSystem.HaveNextLevel())
        {
            int chanceOfSpawn = Random.Range(0, 100);

            int chance = (100 * noOfDestroyedObjects) / noOfObjects;

            if (chanceOfSpawn <= chance && spawned == false)
            {
                GameObject ladder = Instantiate(ladderObject);

                ladder.transform.parent = transform;

                ladder.transform.position = position;

                spawned = true;
            }
        }
    }
}
