using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DayTimerHandler : MonoBehaviour
{
    [Header("Day details:")]
    [SerializeField] private float timeSpeed = .5f;

    [SerializeField] private float intensity;

    [Header("Timer:")]
    [SerializeField] private float minutes = 0;
    [SerializeField] private int hours = 0;
    [SerializeField] private int days  = 0;

    private Light2D globalLight;

    private void Awake()
    {
        globalLight = gameObject.GetComponent<Light2D>();
    }

    private void Update()
    {
        minutes += timeSpeed * Time.deltaTime;

        if(minutes >= 60)
        {
            minutes = 0;
            hours++;

            if(hours >= 24)
            {
                hours = 0;
                days++;
            }
        }

        if(hours > DefaulData.dayStart + DefaulData.dayNightCycleTime && hours <= DefaulData.dayEnd)
        {
            intensity = DefaulData.maxDayIntensity;
        }
        else if((hours > DefaulData.dayEnd + DefaulData.dayNightCycleTime && hours < 24) || (hours >= 0 && hours < DefaulData.dayStart))
        {
            intensity = DefaulData.maxNightIntensity;
        }
        else if(hours >= DefaulData.dayStart && hours <= DefaulData.dayStart + DefaulData.dayNightCycleTime)
        {
            intensity = Mathf.SmoothStep(DefaulData.maxNightIntensity, DefaulData.maxDayIntensity, ((hours - DefaulData.dayStart) + minutes / 60f) / 5f);
        }
        else if(hours >= DefaulData.dayEnd && hours <= DefaulData.dayEnd + DefaulData.dayNightCycleTime)
        {
            intensity = Mathf.SmoothStep(DefaulData.maxDayIntensity, DefaulData.maxNightIntensity, ((hours - DefaulData.dayEnd) + minutes / 60f) / 5f);
        }

        globalLight.intensity = intensity;
    }

    public void SetTimer(int seconds, float minutes, int hours, int days)
    {
        this.minutes = minutes;
        this.hours  = hours;
        this.days   = days;
    }

    public void GetTimer(out float minutes, out int hours, out int days)
    {
        minutes = (int)this.minutes;
        hours   = this.hours;
        days    = this.hours;
    }

    public void GetTimer(out float minutes, out int hours)
    {
        minutes = this.minutes;
        hours = this.hours;
    }

    public void GetIntensity(out float intensity)
    {
        intensity = this.intensity;
    }
}
