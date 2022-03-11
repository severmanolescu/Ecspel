using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform teleportToPoint;

    [SerializeField] private GameObject currentCamera;
    [SerializeField] private GameObject newCamera;

    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;

    private LocationFogParticleChange locationFog;
    private FogHandler fogHandler;

    private LocationRainParticleChange locationRain;
    private RainHandler rainHandler;

    [SerializeField] private bool deactivatePrevLocation = true;

    [SerializeField] private bool setRainSound = true;

    private AudioSource audioSource;

    private LocationGridSave newGrid;
    private LocationGridSave oldGrid;

    public Transform TeleportToPoint { get => teleportToPoint; set => teleportToPoint = value; }

    private void Awake()
    {
        newGrid = newCamera.GetComponentInParent<LocationGridSave>();
        oldGrid = currentCamera.GetComponentInParent<LocationGridSave>();

        locationRain = newGrid.GetComponent<LocationRainParticleChange>();
        locationFog = newGrid.GetComponent<LocationFogParticleChange>();

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
        if(collision.CompareTag("Player"))
        {
            if(audioSource != null)
            {
                audioSource.Play();
            }

            newGrid.gameObject.SetActive(true);

            collision.transform.position = TeleportToPoint.position;

            if (oldGrid != null && deactivatePrevLocation == true)
            {
                //oldGrid.ChangeLocation();
            }

            currentCamera.SetActive(false);

            Vector3 positionCamera = new Vector3();

            positionCamera.z = newCamera.transform.position.z;

            CameraHandler newCameraPositon = newCamera.GetComponent<CameraHandler>();

            newCamera.transform.position = positionCamera;

            SetObject();

            GameObject.Find("Global/BuildSystem").GetComponent<BuildSystemHandler>().LocationGrid = newGrid;

            if (fogHandler != null && locationFog != null)
            {
                fogHandler.ChangeFogLocation(locationFog);
            }

            if (rainHandler != null && locationRain != null)
            {
                rainHandler.ChangeRainLocation(locationRain);
            }

            if(setRainSound == true)
            {
                rainHandler.GetComponent<DayTimerHandler>().StartSoundEffects();
            }
            else
            {
                rainHandler.GetComponent<AudioSource>().Stop();
            }

            newCamera.SetActive(true);

            newGrid.SpawnEnemy();
        }
    }
}
