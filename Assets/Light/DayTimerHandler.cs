using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DayTimerHandler : MonoBehaviour
{
    [Header("Enemy hours spawn")]
    [Range(0, 23)]
    [SerializeField] private int hourToSpawnEnemyStart;
    [Range(0, 23)]
    [SerializeField] private int hourToSpawnEnemyFinal;

    [Header("Day details:")]
    [SerializeField] private float timeSpeed = .5f;

    [SerializeField] private float intensity;

    [Header("Timer:")]
    [SerializeField] private float minutes = 0;
    [SerializeField] private int hours = 0;
    [SerializeField] private int days  = 0;

    [SerializeField] private Gradient gradient;
    [SerializeField] private Gradient gradientWindowLight;

    [Header("Wake up time:")]
    [Range(0, 23)]
    [SerializeField] private int wakeupHour;
    [Range(0, 100)]
    [SerializeField] private float sleepTimeSpeed;

    [Header("Fog data:")]
    [Range(0, 100)]
    [SerializeField] private int percentOfFog;
    [Range(5, 100)]
    [SerializeField] private int smokeMaxAlpha = 50;

    [Header("Rain data:")]
    [Range(0, 100)]
    [SerializeField] private int percentOfRain;
    [Range(0, 1)]
    [SerializeField] private float rainIntensity;

    [Header("Firefly data:")]
    [Range(0, 100)]
    [SerializeField] private int percentOfFirefly;

    [Header("Sound effects:")]
    [SerializeField] private AudioClip daySound;
    [SerializeField] private AudioClip nightSound;
    [SerializeField] private AudioClip rainSound;

    [Header("Hours of sleep"), Range(0, 23)]
    [SerializeField] private int sleepHour;

    private int fogAlpha;
    private bool raining = false;
    private bool fog = false;
    private bool soundEffectSetted = true;
    private bool stopSoundEffects = false;

    private bool startFeelingTired = false;
    private bool sleepNotFromBed = false;

    private UnityEngine.Rendering.Universal.Light2D globalLight;

    private CropGrowHandler cropGrow;

    private RefreshShopItems refreshShopItems;

    private SpawnObjectsInAreaHandle spawnObjects;

    private SourceLightShadow sourceLight;

    private SaveSystemHandler saveSystem;

    private ChangeWindowLightIntensity changeWindowLight;

    private AudioSource audioSource;

    private PlayerStats playerStats;

    private WorldTextDetails worldTextDetails;

    private StartAllFireflyParticles startAllFirefly;

    private ChangeSoilsState changeSoilsState;

    private bool sleepFromStamina = false;

    private bool sleep = false;
    private float speed;
    private int startDay;
    private SleepHandler sleepHandler;

    public int Days { get { return days; } set { days = value; } }

    public int Hours { get => hours; set => hours = value; }
    public float Minutes { get => minutes; }
    public float Intensity { get => intensity; set => intensity = value; }
    public int FogAlpha { get => fogAlpha; set => fogAlpha = value; }
    public bool Raining { get => raining; set => raining = value; }
    public bool Fog { get => fog; set => fog = value; }

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        globalLight = gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        sourceLight = gameObject.GetComponent<SourceLightShadow>();

        speed = timeSpeed;

        spawnObjects = GetComponent<SpawnObjectsInAreaHandle>();

        refreshShopItems = GetComponent<RefreshShopItems>();

        cropGrow = GetComponent<CropGrowHandler>();

        changeWindowLight = GetComponent<ChangeWindowLightIntensity>();

        saveSystem = GameObject.Find("SaveSystem").GetComponent<SaveSystemHandler>();

        startAllFirefly = GetComponent<StartAllFireflyParticles>();

        startAllFirefly.StopParticles();

        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        worldTextDetails = GameObject.Find("Global/Player/Canvas/WorldTextDetails").GetComponent<WorldTextDetails>();

        changeSoilsState = GetComponent<ChangeSoilsState>();
    }

    private void Update()
    {
        minutes += timeSpeed * Time.deltaTime;

        if(Minutes >= 60)
        {
            minutes = 0;
            Hours++;

            if((hours >= hourToSpawnEnemyStart && hours <= 23) || 
               (hours >= 0 && hours <= hourToSpawnEnemyFinal) &&
               raining == false)
            {
                float chance = Random.Range(0, 100);

                if (chance <= percentOfFirefly)
                {
                    startAllFirefly.StartParticles();
                }

                if(soundEffectSetted == true)
                {
                    audioSource.clip = nightSound;

                    if (stopSoundEffects == false)
                    {
                        audioSource.Play();
                    }

                    soundEffectSetted = false;
                }
            }
            else
            {
                startAllFirefly.StopParticles();

                if(raining == false && soundEffectSetted == false)
                {
                    audioSource.clip = daySound;

                    if (stopSoundEffects == false)
                    {
                        audioSource.Play();
                    }

                    soundEffectSetted = true;
                }
            }

            if(Hours >= 24)
            {
                Hours = 0;
                days++;

                cropGrow.DayChange(days);
            }

            if(hours == sleepHour && sleep == false)
            {
                worldTextDetails.ShowText("Esti obosit!");

                startFeelingTired = true;
            }
            else
            {
                startFeelingTired = false;
            }
            if (hours == sleepHour + 1 && sleep == false)
            {
                Sleep();
            }
        }

        if(startFeelingTired == true && minutes >= 30)
        {
            startFeelingTired = false;

            worldTextDetails.ShowText("Esti foarte obosit!");
        }

        intensity = (Hours + Minutes / 60) / 24;

        sourceLight.ChangeLightsIntensity(1f - intensity);

        globalLight.color = gradient.Evaluate(intensity);

        changeWindowLight.SetIntensity(gradientWindowLight.Evaluate(intensity));

        if(sleep == true && (days > startDay || sleepNotFromBed == true))
        {
            if (Hours == wakeupHour)
            {
                WakeUp();
            }
            else if(Hours == wakeupHour - 3)
            {
                StartTodayWeather();
            }
        }
    }

    public void Sleep(bool sleepOutOfStamina = false)
    {
        sleepNotFromBed = true;

        sleep = true;

        if(sleepOutOfStamina == true)
        {
            sleepFromStamina = true;
        }

        GameObject.Find("PlayerHouse/Bed").GetComponent<SleepHandler>().Sleep();
    }

    private void WakeUp()
    {
        cropGrow.DayChange(days);
        spawnObjects.DayChange(days);
        refreshShopItems.Refresh(days);

        if (raining)
        {
            changeSoilsState.WetAllDoils();
        }
        else
        {
            changeSoilsState.DryAllDoils();
        }

        timeSpeed = speed;

        Time.timeScale = 1;

        sleep = false;

        if(sleepNotFromBed == true && sleepFromStamina == false)
        {
            worldTextDetails.ShowText("Ai adormit, dar te-ai trezit in pat!");

            sleepNotFromBed = false;
        }
        else if(sleepFromStamina == true)
        {
            worldTextDetails.ShowText("Ai obosit foarte rau asa ca ai ajuns ca prin miracol acasa");

            sleepNotFromBed = false;

            sleepFromStamina = false;
        }

        sleepHandler.StopSleep();

        sleepHandler = null; 

        playerStats.SetToMaxStats();

        startAllFirefly.StopParticles();        

        saveSystem.StartSaveGame();
    }

    public void StopTime()
    {
        timeSpeed = 0;
    }

    public void StartTime()
    {
        timeSpeed = 1;
    }

    private void StartTodayWeather()
    {
        float chanceOfFog = Random.Range(0, 100);

        if (chanceOfFog <= percentOfFog)
        {
            fogAlpha = Random.Range(5, smokeMaxAlpha);

            GetComponent<FogHandler>().StopPArticles();

            GetComponent<FogHandler>().StartParticle(fogAlpha);

            fog = true;
        }
        else
        {
            GetComponent<FogHandler>().StopPArticles();

            fog = false;
        }

        float changeOfRain = Random.Range(0, 100);

        if (changeOfRain <= percentOfRain)
        {
            GetComponent<RainHandler>().StartParticle();

            globalLight.intensity = rainIntensity;

            raining = true;

            PlayRainSound();
        }
        else
        {
            GetComponent<RainHandler>().StopPArticles();

            audioSource.Stop();

            raining = false;

            globalLight.intensity = 1;
        }
    }

    public void PlayRainSound()
    {
        if(raining == true)
        {
            audioSource.clip = rainSound;

            if (stopSoundEffects == false)
            {
                audioSource.Play();
            }
        }
    }

    public void StopRainSound()
    {
        audioSource.Stop();
    }

    public void StopSoundEffects()
    {
        stopSoundEffects = true;

        audioSource.Stop();
    }

    public void StartSoundEffects()
    {
        stopSoundEffects = false;

        audioSource.Play();
    }

    public void SetTimer(float minutes, int hours, int days)
    {
        this.minutes = minutes;
        this.Hours  = hours;
        this.days   = days;
    }

    public void GetTimer(out float minutes, out int hours, out int days)
    {
        minutes = (int)this.Minutes;
        hours   = this.Hours;
        days    = this.Hours;
    }

    public void GetTimer(out float minutes, out int hours)
    {
        minutes = this.Minutes;
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

        Time.timeScale = 50f;

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

    public void LoadGameSetWakeUpHour()
    {
        hours = wakeupHour;

        if (fog == false && raining == false && soundEffectSetted == false)
        {
            audioSource.clip = daySound;

            if (stopSoundEffects == false)
            {
                audioSource.Play();
            }

            soundEffectSetted = true;
        }
    }

    public void SetWeatherAtLoad(bool rain, bool fog, int fogIntensity)
    {
        if(rain == true)
        {
            GetComponent<RainHandler>().StartParticle();

            globalLight.intensity = rainIntensity;

            raining = true;

            PlayRainSound();
        }
        else
        {
            GetComponent<RainHandler>().StopPArticles();

            audioSource.Stop();

            raining = false;

            globalLight.intensity = 1;
        }

        if(fog == true)
        {
            fogAlpha = fogIntensity;

            GetComponent<FogHandler>().StartParticle(fogAlpha);

            fog = true;
        }
        else
        {
            GetComponent<FogHandler>().StopPArticles();

            fog = false;
        }
    }
}
