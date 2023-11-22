using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenCoopHandler : MonoBehaviour
{
    [SerializeField] private BoxCollider2D spawnArea;
    [SerializeField] private BoxCollider2D enterCoopArea;

    [Range(0, 100)]
    [SerializeField] private int rateToExitCoop = 20;

    [Header("Interval in seconds to check again if a chicken exit the coop")]
    [SerializeField] private int checkInterval = 3;

    [SerializeField] private List<ChickenAI> chickens = new List<ChickenAI>(); 

    [SerializeField] private List<ChickenAI> chickensInCoop = new List<ChickenAI>();

    private NightCheckForAnimals nightCheckForAnimals;

    public BoxCollider2D EnterCoopArea { get => enterCoopArea; }
    public BoxCollider2D SpawnArea { get => spawnArea; }

    private void Start()
    {
        nightCheckForAnimals = GameObject.Find("Global/DayTimer").GetComponent<NightCheckForAnimals>();

        nightCheckForAnimals.AddChickenCoop(this);

        StartCoroutine(WaitBeforeCheckAgain());
    }

    private IEnumerator WaitBeforeCheckAgain()
    {
        while(true)
        {
            yield return new WaitForSeconds(checkInterval);

            if(nightCheckForAnimals.CheckIfSpawn() && chickensInCoop.Count != 0)
            {
                if(Random.Range(0, 100) <= rateToExitCoop)
                {
                    chickensInCoop[0].gameObject.SetActive(true);

                    chickensInCoop[0].StartMoving();

                    chickensInCoop.Remove(chickensInCoop[0]);
                }
            }
        }
    }

    public void Sleep()
    {
        if (chickensInCoop != null)
        {
            foreach (ChickenAI chickenAI in chickens)
            {
                if (chickenAI != null)
                {
                    chickenAI.MoveToCoop();
                }
            }
        }
    }

    public void EnterCoop(ChickenAI chickenAI)
    {
        if(chickenAI != null && !chickensInCoop.Contains(chickenAI))
        {
            chickensInCoop.Add(chickenAI);
        }
    }
}
