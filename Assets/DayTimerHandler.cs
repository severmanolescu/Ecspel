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

    private int dayStart;
    private int dayEnd;
    private float maxDayIntensity;
    private float maxNightIntensity;
    private int dayNightCycleTime;

    private void Awake()
    {
        globalLight = gameObject.GetComponent<Light2D>(); 
    }

    private void Start()
    {
        dayStart = DefaulData.dayStart;
        dayEnd = DefaulData.dayEnd;
        maxDayIntensity = DefaulData.maxDayIntensity;
        maxNightIntensity = DefaulData.maxNightIntensity;
        dayNightCycleTime = DefaulData.dayNightCycleTime;
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

        if(hours > dayStart + dayNightCycleTime && hours <= dayEnd)
        {
            intensity = maxDayIntensity;
        }
        else if((hours > dayEnd + dayNightCycleTime && hours < 24) || (hours >= 0 && hours < dayStart))
        {
            intensity = maxNightIntensity;
        }
        else if(hours >= dayStart && hours <= dayStart + dayNightCycleTime)
        {
            intensity = Mathf.SmoothStep(maxNightIntensity, maxDayIntensity, ((hours - dayStart) + minutes / 60f) / 5f);
        }
        else if(hours >= dayEnd && hours <= dayEnd + dayNightCycleTime)
        {
            intensity = Mathf.SmoothStep(maxDayIntensity, maxNightIntensity, ((hours - dayEnd) + minutes / 60f) / 5f);
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
