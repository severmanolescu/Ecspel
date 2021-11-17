using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestOpenHandler : MonoBehaviour
{
    private GameObject player = null;

    private TextMeshProUGUI text;

    private ChestStorage chestStorage;

    private PlayerMovement playerMovement;

    private GameObject playerInventory;

    private ChestStorageHandler chestStorageHandler;

    private CanvasTabsOpen canvasTabsOpen;

    private GameObject quickSlots;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        chestStorage = GetComponent<ChestStorage>();

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.Find("Player/Canvas/Field/Inventory/PlayerInventory");
        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
        chestStorageHandler = GameObject.Find("Player/Canvas/Field/Inventory/PlayerInventory/ChestStorage").GetComponent<ChestStorageHandler>();

        quickSlots = GameObject.Find("Player/Canvas/Field/QuickSlots");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = collision.gameObject;

            text.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            player = null;

            text.gameObject.SetActive(false);

            playerMovement.TabOpen = false;
            playerInventory.SetActive(false);
            chestStorageHandler.gameObject.SetActive(false);
            chestStorageHandler.SetChestStorage(chestStorage.Items, chestStorage.ChestMaxSlots);

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
                if (GameObject.Find("Player/Canvas/Field/Inventory/PlayerInventory").activeSelf == false)
                {
                    playerMovement.TabOpen = true;
                    playerInventory.SetActive(true);
                    chestStorageHandler.gameObject.SetActive(true);
                    chestStorageHandler.SetChestStorage(chestStorage.Items, chestStorage.ChestMaxSlots);

                    canvasTabsOpen.SetCanOpenTabs(false);

                    quickSlots.SetActive(false);
                }
                else
                {
                    playerMovement.TabOpen = false;
                    playerInventory.SetActive(false);
                    chestStorageHandler.gameObject.SetActive(false);
                    chestStorage.SetItems(GameObject.Find("Player/Canvas/Field/Inventory/PlayerInventory/ChestStorage").GetComponent<ChestStorageHandler>().GetChestStorage());

                    canvasTabsOpen.SetCanOpenTabs(true);

                    quickSlots.SetActive(true);
                }
            }

            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                playerMovement.TabOpen = false;
                playerInventory.SetActive(false);
                chestStorageHandler.gameObject.SetActive(false);
                chestStorageHandler.SetChestStorage(chestStorage.Items, chestStorage.ChestMaxSlots);

                canvasTabsOpen.SetCanOpenTabs(true);

                quickSlots.SetActive(true);
            }
        } 
    }
}
