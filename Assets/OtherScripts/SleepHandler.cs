using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SleepHandler : MonoBehaviour
{
    [SerializeField] private GameObject currentCamera;
    [SerializeField] private GameObject sleepCamera;

    private GameObject player = null;

    private TextMeshProUGUI text;

    private PlayerMovement playerMovement;

    private CanvasTabsOpen canvasTabsOpen;

    private GameObject quickSlots;

    private GameObject playerStats;

    private DayTimerHandler dayTimer;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();

        quickSlots = GameObject.Find("Player/Canvas/Field/QuickSlots");

        playerStats = GameObject.Find("Player/Canvas/Stats");

        dayTimer = GameObject.Find("DayTimer").GetComponent<DayTimerHandler>();
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

        playerStats.SetActive(state);

        currentCamera.SetActive(state);

        currentCamera.SetActive(state);
        sleepCamera.SetActive(!state);
    }

    private void Update()
    {
        if (player != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                ChangeObjectsStates(false);

                dayTimer.Sleep(this);
            }
        }
    }

    public void StopSleep()
    {
        ChangeObjectsStates(true);
    }
}
