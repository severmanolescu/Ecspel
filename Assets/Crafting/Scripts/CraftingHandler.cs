using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingHandler : MonoBehaviour
{
    private GameObject player = null;

    private TextMeshProUGUI text;

    private PlayerMovement playerMovement;

    private GameObject playerInventory;

    private CraftCanvasHandler craftCanvas;

    private CanvasTabsOpen canvasTabsOpen;

    private GameObject quickSlots;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.Find("Player/Canvas/PlayerItems");
        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
        craftCanvas = GameObject.Find("Player/Canvas/Field/Crafting").GetComponent<CraftCanvasHandler>();

        quickSlots = GameObject.Find("Player/Canvas/Field/QuickSlots");
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
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (playerInventory.activeSelf == false)
                {
                    playerMovement.TabOpen = true;
                    playerInventory.SetActive(true);

                    craftCanvas.gameObject.SetActive(true);
                    craftCanvas.ReinitializeAllCraftings();

                    canvasTabsOpen.SetCanOpenTabs(false);

                    quickSlots.SetActive(false);
                }
                else
                {
                    playerMovement.TabOpen = false;
                    playerInventory.SetActive(false);
                    craftCanvas.gameObject.SetActive(false);

                    canvasTabsOpen.SetCanOpenTabs(true);

                    quickSlots.SetActive(true);
                }
            }

            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                playerMovement.TabOpen = false;
                playerInventory.SetActive(false);
                craftCanvas.gameObject.SetActive(false);

                canvasTabsOpen.SetCanOpenTabs(true);

                quickSlots.SetActive(true);
            }
        }
    }
}