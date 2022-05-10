using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class CraftingHandler : MonoBehaviour
{
    private GameObject player = null;

    private TextMeshProUGUI text;

    private PlayerMovement playerMovement;

    private GameObject playerInventory;

    private CraftCanvasHandler craftCanvas;

    private GameObject chestCanvas;

    private CanvasTabsOpen canvasTabsOpen;

    private GameObject quickSlots;

    private Keyboard keyboard;

    private bool fKeyPress = true;

    private bool opened = false;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.Find("Player/Canvas/PlayerItems");
        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
        craftCanvas = GameObject.Find("Player/Canvas/Field/Crafting").GetComponent<CraftCanvasHandler>();

        quickSlots = GameObject.Find("Player/Canvas/Field/QuickSlots");

        keyboard = InputSystem.GetDevice<Keyboard>();

        chestCanvas = GameObject.Find("Global/Player/Canvas/ChestStorage");
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
            playerInventory.SetActive(false);
            craftCanvas.gameObject.SetActive(false);

            canvasTabsOpen.SetCanOpenTabs(true);

            quickSlots.SetActive(true);

            opened = false;
        }
    }

    private IEnumerator WaitToNextFrame()
    {
        yield return new WaitForEndOfFrame();

        canvasTabsOpen.SetCanOpenTabs(true);
    }

    private void Update()
    {
        if (player != null)
        {
            if (keyboard.fKey.wasPressedThisFrame || (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == false && fKeyPress == false))
            {
                fKeyPress = true;

                if (playerMovement.MenuOpen == false && canvasTabsOpen.CanOpenTab())
                {
                    if(playerInventory.activeSelf == false)
                    {
                        playerMovement.TabOpen = true;
                        playerInventory.SetActive(true);

                        craftCanvas.gameObject.SetActive(true);
                        craftCanvas.ReinitializeAllCraftings();

                        quickSlots.SetActive(false);

                        canvasTabsOpen.SetCanOpenTabs(false);

                        craftCanvas.CraftingHandler = this;

                        opened = true;
                    }
                }
                else if(opened == true)
                {
                    CloseCraft();
                }
            }

            if (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == true)
            {
                fKeyPress = false;
            }
        }
    }

    public void CloseCraft()
    {
        playerMovement.TabOpen = false;
        playerInventory.SetActive(false);
        craftCanvas.gameObject.SetActive(false);

        quickSlots.SetActive(true);

        canvasTabsOpen.SetCanOpenTabs(true);

        opened = false;
    }
}
