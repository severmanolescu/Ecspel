using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenDoorWithItem : MonoBehaviour
{
    [SerializeField] private Item needItem;

    [Header("New location")]
    [SerializeField] private GameObject newCamera;
    [SerializeField] private GameObject oldCamera;

    [SerializeField] private Transform newPosition;

    [Header("Input text")]
    [SerializeField] private GameObject text;

    [SerializeField] private List<GameObject> objectsToSetActiveToFalse;
    [SerializeField] private List<GameObject> objectsToSetActiveToTrue;

    private GameObject player = null;

    private CanvasTabsOpen canvasTabsOpen;

    private PlayerMovement playerMovement;

    private AudioSource audioSource;

    private WorldTextDetails worldText;

    private PlayerInventory playerInventory;

    private DayTimerHandler dayTimer;

    private bool fKeyPress = true;

    private void Awake()
    {
        text.SetActive(false);

        worldText = GameObject.Find("Global/Player/Canvas/WorldTextDetails").GetComponent<WorldTextDetails>();

        audioSource = GetComponent<AudioSource>();

        canvasTabsOpen = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();

        dayTimer = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();
        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            player = collision.gameObject;

            text.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            player = null;

            text.SetActive(false);
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if ((Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame) ||
               (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == false && fKeyPress == false))
            {
                fKeyPress = true;

                if (canvasTabsOpen.CanOpenTab() && playerMovement.MenuOpen == false && playerMovement.TabOpen == false)
                {
                    if (playerInventory.SearchInventory(needItem, 1) == true)
                    {
                        player.transform.position = newPosition.position;

                        newCamera.SetActive(true);
                        oldCamera.SetActive(false);

                        dayTimer.StopSoundEffects();

                        audioSource.Play();

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
                        worldText.ShowText("Ai nevoie de un obiect special pentru a deschide!");
                    }
                }
            }

            if (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == true)
            {
                fKeyPress = false;
            }
        }
    }
}
