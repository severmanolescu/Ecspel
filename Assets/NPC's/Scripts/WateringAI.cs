using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WateringAI : MonoBehaviour
{
    [SerializeField] private int maxWaterUse = 7;

    [SerializeField] private LocationGridSave locationGrid;

    [SerializeField] private GameObject waterEffectPrefab;

    private Transform waterParent;

    private int waterUsed = 0;

    private WateringStart wateringStart;

    private NpcPathFinding npcPath;

    private Animator animator;

    bool getWater = false;

    private void Awake()
    {
        npcPath = GetComponent<NpcPathFinding>();

        waterParent = GameObject.Find("ToBeDestroyed").GetComponent<Transform>();

        animator = GetComponent<Animator>();
    }

    public void StartWorking(WateringStart watering)
    {
        if(watering != null)
        {
            waterUsed = 0;

            wateringStart = watering;

            GoToNextLocation();
        }
    }

    private void GoToNextLocation()
    {
        GridNode newLocation = wateringStart.GetNewLocation();

        if (newLocation != null)
        {
            npcPath.ChangeLocation(locationGrid.Grid.GetWorldPositionCenter(newLocation.x - 2, newLocation.y));
        }
        else
        {
            StopWorking();
        }
    }

    public void ArrivedAtLocation()
    {
        if (getWater)
        {
            waterUsed = 0;

            getWater = false;

            npcPath.ChangeIdleAnimation(Direction.Up);

            animator.SetTrigger("Fill");
        }
        else
        {
            npcPath.ChangeIdleAnimation(Direction.Right);

            animator.SetTrigger("Water");
        }
    }

    private void CreateWaterEffect()
    {
        GameObject water = Instantiate(waterEffectPrefab);

        water.transform.position = wateringStart.GetCurrentPosition();

        water.transform.SetParent(waterParent);
    }

    private void WateringTrigger()
    {
        CreateWaterEffect();

        if(++waterUsed > maxWaterUse)
        {
            getWater = true;

            npcPath.ChangeLocation(wateringStart.FountainLocation);
        }
        else
        {
            GoToNextLocation();
        }
    }

    private void FillTrigger()
    {
        GoToNextLocation();
    }

    private void StopWorking()
    {
        GetComponent<NpcBehavior>().StopWorking();
    }
}
