using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform teleportToPoint;

    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;

    private LocationFogParticleChange locationFog;
    private FogHandler fogHandler;

    private LocationRainParticleChange locationRain;
    private RainHandler rainHandler;

    [SerializeField] private bool setRainSound = true;

    private AudioSource audioSource;

    public Transform TeleportToPoint { get => teleportToPoint; set => teleportToPoint = value; }

    private void Awake()
    {
        fogHandler = GameObject.Find("Global/DayTimer").GetComponent<FogHandler>();
        rainHandler = GameObject.Find("Global/DayTimer").GetComponent<RainHandler>();

        audioSource = GetComponent<AudioSource>();
    }

    private void SetObject()
    {
        foreach (GameObject gameObject in objectsToSetActiveToFalse)
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject gameObject in objectsToSetActiveToTrue)
        {
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }

            collision.transform.position = TeleportToPoint.position;

            SetObject();

            if (fogHandler != null && locationFog != null)
            {
                fogHandler.ChangeFogLocation(locationFog);
            }

            if (rainHandler != null && locationRain != null)
            {
                rainHandler.ChangeRainLocation(locationRain);
            }

            if (setRainSound == true)
            {
                rainHandler.GetComponent<DayTimerHandler>().StartSoundEffects();
            }
            else
            {
                rainHandler.GetComponent<AudioSource>().Stop();
            }
        }
    }
}
