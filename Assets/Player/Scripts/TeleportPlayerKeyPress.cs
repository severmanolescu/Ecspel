using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TeleportPlayerKeyPress : MonoBehaviour
{
    [Header("Can enter interval:")]
    [Range(0, 24)]
    [SerializeField] private int startHour;
    [Range(0, 24)]
    [SerializeField] private int finishHour;

    [SerializeField] private Transform teleportToPoint;

    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;

    private CanvasTabsOpen canvasTabsOpen;

    private PlayerMovement playerMovement;

    private WorldTextDetails worldText;

    private DayTimerHandler dayTimer;

    private GameObject text;

    private GameObject player = null;

    private Keyboard keyboard;

    private AudioSource audioSource;

    public Transform TeleportToPoint { get => teleportToPoint; set => teleportToPoint = value; }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>().gameObject;

        text.SetActive(false);

        dayTimer = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();

        worldText = GameObject.Find("Global/Player/Canvas/WorldTextDetails").GetComponent<WorldTextDetails>();

        audioSource = GetComponent<AudioSource>();

        keyboard = InputSystem.GetDevice<Keyboard>();

        canvasTabsOpen = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;

            text.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;

            text.SetActive(false);
        }
    }

    private void Update()
    {
        if (player != null && keyboard.fKey.wasPressedThisFrame)
        {
            if (canvasTabsOpen.CanOpenTab() && playerMovement.MenuOpen == false && playerMovement.TabOpen == false)
            {
                if ((startHour == 0 && finishHour == 0) ||
                     dayTimer.Hours >= startHour &&
                     dayTimer.Hours <= finishHour)
                {
                    if (audioSource != null)
                    {
                        audioSource.Play();
                    }

                    player.transform.position = TeleportToPoint.position;

                    foreach (GameObject gameObject in objectsToSetActiveToFalse)
                    {
                        gameObject.SetActive(false);
                    }

                    foreach (GameObject gameObject in objectsToSetActiveToTrue)
                    {
                        gameObject.SetActive(true);
                    }

                    GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>().StopSoundEffects();
                }
                else
                {
                    worldText.ShowText("Usa inchisa!");
                }
            }
        }
    }
}
