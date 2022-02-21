using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChnageLeavesMaterialIntensity : MonoBehaviour
{
    [SerializeField] private Material render;

    private DayTimerHandler dayTimerHandler;

    private void Awake()
    {
        dayTimerHandler = GetComponent<DayTimerHandler>();
    }

    public void Update()
    {
        if (dayTimerHandler.Intensity >= 0.36f)
        {
            if (dayTimerHandler.Intensity <= 0.743f)
            {
                render.SetFloat("_LightIntensity", 1);
            }
            else
            {
                render.SetFloat("_LightIntensity", dayTimerHandler.Intensity.map(.74f, 1, 1, 5)); ;
            }
        }
        else
        {
            render.SetFloat("_LightIntensity", 6 - dayTimerHandler.Intensity.map(0, .36f, 1, 5));
        }
    }
}