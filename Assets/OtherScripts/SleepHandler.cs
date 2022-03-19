using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class SleepHandler : MonoBehaviour
{
    [SerializeField] private SaveSystemHandler saveSystem;
    [SerializeField] private GameObject questdirection;

    [SerializeField] private List<GameObject> currentCamera;
    [SerializeField] private GameObject playerHouseCamera;
    [SerializeField] private GameObject sleepCamera;
    [SerializeField] private GameObject playerHouseGround;

    [SerializeField] private Transform bedLocation;

    private GameObject player = null;

    private TextMeshProUGUI text;

    private PlayerMovement playerMovement;

    private CanvasTabsOpen canvasTabsOpen;

    private GameObject quickSlots;

    private GameObject playerStats;

    private DayTimerHandler dayTimer;

    private Keyboard keyboard;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();

        quickSlots = GameObject.Find("Player/Canvas/Field/QuickSlots");

        playerStats = GameObject.Find("Player/Canvas/Stats");

        dayTimer = GameObject.Find("DayTimer").GetComponent<DayTimerHandler>();

        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = collision.gameObject;

            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;

            text.gameObject.SetActive(false);

            playerMovement.TabOpen = false;

            canvasTabsOpen.SetCanOpenTabs(true);

            quickSlots.SetActive(true);
        }
    }

    private void ChangeObjectsStates(bool state)
    {
        playerMovement.TabOpen = !state;

        canvasTabsOpen.SetCanOpenTabs(state);

        quickSlots.SetActive(state);
        playerHouseCamera.SetActive(state);

        playerStats.SetActive(state);

        playerHouseGround.SetActive(true);

        foreach (GameObject camera in currentCamera)
        {
            camera.SetActive(false);
        }

        questdirection.SetActive(state);

        sleepCamera.SetActive(!state);
    }

    public void Sleep()
    {
        ChangeObjectsStates(false);

        dayTimer.Sleep(this);

        playerMovement.transform.position = bedLocation.position;
    }

    private void Update()
    {
        if (player != null)
        {
            if (keyboard.fKey.wasPressedThisFrame)
            {
                Sleep();
            }
        }
    }

    public void StopSleep()
    {
        ChangeObjectsStates(true);

        saveSystem.StartSaveGame();
    }
}
