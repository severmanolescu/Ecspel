using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionHandler : MonoBehaviour
{
    private Animator animator;

    private CanvasTabsOpen canvasTabs;

    private PlayerMovement playerMovement;

    private SleepHandler sleepHandler;

    private Image transitionImage;

    private Vector3 teleportTo;

    private bool toInterior = false;

    private void Awake()
    {
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();

        canvasTabs = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();

        transitionImage = GetComponent<Image>();

        animator = GetComponent<Animator>();
    }

    private void StartTransition()
    {
        transitionImage.gameObject.SetActive(true);

        playerMovement.TabOpen = true;

        canvasTabs.canOpenTabs = false;

        animator.SetTrigger("Play");

        canvasTabs.PrepareUIForDialogue();
    }

    public void PlayTransition(SleepHandler sleepHandler)
    {
        this.sleepHandler = sleepHandler;

        StartTransition();
    }

    public void PlayTransition(Vector3 teleportTo, bool toInterior)
    {
        this.toInterior = toInterior;

        this.teleportTo = teleportTo;

        StartTransition();
    }

    private void TriggerFinishTransition()
    {
        if (sleepHandler != null)
        {
            sleepHandler.WakeUp();

            sleepHandler = null;
        }

        playerMovement.TabOpen = false;

        canvasTabs.canOpenTabs = true;

        transitionImage.gameObject.SetActive(false);

        canvasTabs.ShowDefaultUIElements();
    }

    private void TriggerTeleportPlayer()
    {
        if(sleepHandler == null)
        {
            playerMovement.transform.position = teleportTo;

            if (toInterior)
            {
                playerMovement.GetComponent<SpriteRenderer>().sortingLayerName = "Interior";
            }
            else
            {
                playerMovement.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
            }
        }
        else
        {
            sleepHandler.TransitionStart();
        }
    }
}
