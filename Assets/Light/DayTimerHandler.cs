using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DayTimerHandler : MonoBehaviour
{
    [SerializeField] private int hourToSpawnEnemyStart;
    [SerializeField] private int hourToSpawnEnemyFinal;

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

    private CropGrowHandler cropGrow;

    private SpawnObjectsInAreaHandle spawnObjects;

    private SourceLightShadow sourceLight;

    private SaveSystemHandler saveSystem;

    private bool sleep = false;
    private float speed;
    private int startDay;
    private SleepHandler sleepHandler;

    public int Days { get { return days; } set { days = value; } }

    public int Hours { get => hours; set => hours = value; }

    private void Awake()
    {
        globalLight = gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        sourceLight = gameObject.GetComponent<SourceLightShadow>();

        speed = timeSpeed;

        spawnObjects = GetComponent<SpawnObjectsInAreaHandle>();

        cropGrow = GetComponent<CropGrowHandler>();

        saveSystem = GameObject.Find("SaveSystem").GetComponent<SaveSystemHandler>();
    }

    private void Update()
    {
        minutes += timeSpeed * Time.deltaTime;

        if(minutes >= 60)
        {
            minutes = 0;
            Hours++;

            if(Hours >= 24)
            {
                Hours = 0;
                days++;

                cropGrow.DayChange(days);
                spawnObjects.DayChange(days);

                saveSystem.SaveGame();
            }
        }

        intensity = (Hours + minutes / 60) / 24;

        sourceLight.ChangeLightsIntensity(1f - intensity);

        globalLight.color = gradient.Evaluate(intensity);

        if(sleep == true && wakeupHour == Hours && days > startDay)
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
        this.Hours  = hours;
        this.days   = days;
    }

    public void GetTimer(out float minutes, out int hours, out int days)
    {
        minutes = (int)this.minutes;
        hours   = this.Hours;
        days    = this.Hours;
    }

    public void GetTimer(out float minutes, out int hours)
    {
        minutes = this.minutes;
        hours = this.Hours;
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

    public bool CanSpawnEnemy()
    {
        if((Hours >= hourToSpawnEnemyStart && Hours <= 23) ||
           (Hours >= 0 && Hours <= hourToSpawnEnemyFinal))
        {
            return true;
        }
        return false;
    }
}
