using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DayTimerHandler : MonoBehaviour
{
    [Header("Day details:")]
    [SerializeField] private float timeSpeed = .5f;

    [SerializeField] private float intensity;

    [Header("Timer:")]
    [SerializeField] private float minutes = 0;
    [SerializeField] private int hours = 0;
    [SerializeField] private int days  = 0;

    [SerializeField] private Gradient gradient;

    [Header("Wake up time:")]
    [SerializeField] private int wakeupHour;
    [SerializeField] private float sleepTimeSpeed;

    private UnityEngine.Rendering.Universal.Light2D globalLight;

    private SourceLightShadow sourceLight;

    private bool sleep = false;
    private float speed;
    private int startDay;
    private SleepHandler sleepHandler;

    public int Days { get { return days; } }

    private void Awake()
    {
        globalLight = gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        sourceLight = gameObject.GetComponent<SourceLightShadow>();

        speed = timeSpeed;
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

                GetComponent<CropGrowHandler>().DayChange(days);
                GetComponent<SpawnObjectsInAreaHandle>().DayChange(days);
            }
        }

        intensity = (hours + minutes / 60) / 24;

        sourceLight.ChangeLightsIntensity(1 - intensity);

        globalLight.color = gradient.Evaluate(intensity);

        if(sleep == true && wakeupHour == hours && days > startDay)
        {
            timeSpeed = speed;

            sleep = false;

            sleepHandler.StopSleep();

            sleepHandler = null;
        }
    }

    public void SetTimer(float minutes, int hours, int days)
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

    public void Sleep(SleepHandler sleepHandler)
    {
        this.sleepHandler = sleepHandler;

        timeSpeed = sleepTimeSpeed;

        startDay = days;

        sleep = true;
    }
}