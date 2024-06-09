using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SleepHandler : Event
{
    [SerializeField] private SaveSystemHandler saveSystem;

    private TextMeshProUGUI text;

    private DayTimerHandler dayTimer;

    private Keyboard keyboard;

    private TransitionHandler transition;

    private bool playerInArea = false;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        transition = GameObject.Find("Player/Canvas/Transition").GetComponent<TransitionHandler>();

        dayTimer = GameObject.Find("DayTimer").GetComponent<DayTimerHandler>();

        keyboard = InputSystem.GetDevice<Keyboard>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SleepTrigger") && canTrigger == true)
        {
            playerInArea = true;

            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("SleepTrigger") && canTrigger == true)
        {
            playerInArea = false;

            text.gameObject.SetActive(false);
        }
    }

    public void Sleep()
    {
        transition.PlayTransition(this);
    }

    public void WakeUp()
    {
        //saveSystem.StartSaveGame();
    }

    public void TransitionStart()
    {
        dayTimer.Sleep();
    }

    private void Update()
    {
        if (playerInArea && canTrigger == true)
        {
            if (keyboard.fKey.wasPressedThisFrame)
            {
                Sleep();
            }
        }
    }
}
