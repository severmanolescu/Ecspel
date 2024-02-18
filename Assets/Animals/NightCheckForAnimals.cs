using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightCheckForAnimals : MonoBehaviour
{
    [Range(0, 24)]
    [SerializeField] private int startSpawnHours;
    [Range(0, 24)]
    [SerializeField] private int finalSpawnHours;

    [Range(0, 24)]
    [SerializeField] private int sleepHour;
    [Range(0, 24)]
    [SerializeField] private int wakeUpHour;

    [Header("Check interval for night in seconds")]
    [SerializeField] private int checkInterval = 3;

    private DayTimerHandler dayTimerHandler;

    private List<BirdAI> birds = new List<BirdAI>();
    public List<ChickenCoopHandler> chickenCoopHandlers = new List<ChickenCoopHandler>();

    private void Start()
    {
        dayTimerHandler = GetComponent<DayTimerHandler>();

        WaitBeforeCheckAgain();

        StartCoroutine(WaitBeforeCheckAgain());
    }

    public void AddBirds(BirdAI bird)
    {
        if(bird != null && birds != null)
        {
            birds.Add(bird);
        }
    }

    public void AddChickenCoop(ChickenCoopHandler chickenCoop)
    {
        if (chickenCoopHandlers != null && chickenCoop != null)
        {
            chickenCoopHandlers.Add(chickenCoop);
        }
    }

    public bool CheckIfSpawn()
    {
        if(dayTimerHandler.Hours >= startSpawnHours && dayTimerHandler.Hours < finalSpawnHours)
        {
            return true;
        }

        return false;
    }

    private void BirdSleep()
    {
        if(birds != null  && birds.Count > 0)
        {
            List<BirdAI> toRemove = new List<BirdAI>();

            foreach (BirdAI bird in birds)
            {
                if (bird != null)
                {
                    bird.Sleep();
                }
                else
                {
                    toRemove.Add(bird);
                }
            }

            foreach (BirdAI bird in toRemove)
            {
                birds.Remove(bird);
            }
        }
    }

    private void ChickenSleep()
    {
        foreach(ChickenCoopHandler coopHandler in chickenCoopHandlers)
        {
            coopHandler.Sleep();
        }
    }

    private void AnimalsSleep()
    {
        BirdSleep();
        ChickenSleep();
    }

    private IEnumerator WaitBeforeCheckAgain()
    {
        while(true)
        {
            if(dayTimerHandler.Hours >= sleepHour)
            {
                AnimalsSleep();
            }

            yield return new WaitForSeconds(checkInterval);
        }
    }
}
