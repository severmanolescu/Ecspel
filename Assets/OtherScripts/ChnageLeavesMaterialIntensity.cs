using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChnageLeavesMaterialIntensity : MonoBehaviour
{
    [SerializeField] private Material leavesMaterial;
    [SerializeField] private Material grassMaterial;

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
                leavesMaterial.SetFloat("_LightIntensity", 1);
                grassMaterial.SetFloat("_LightIntensity", 1);
            }
            else
            {
                float intensity = dayTimerHandler.Intensity.map(.74f, 1, 1, 2.5f);

                leavesMaterial.SetFloat("_LightIntensity", intensity);
                grassMaterial.SetFloat("_LightIntensity", intensity);
            }
        }
        else
        {
            float intensity = 4 - dayTimerHandler.Intensity.map(0f, .36f, 1, 3f);

            leavesMaterial.SetFloat("_LightIntensity", intensity);
            grassMaterial.SetFloat("_LightIntensity", intensity);
        }
    }
}