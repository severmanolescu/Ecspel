using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTabsOpen : MonoBehaviour
{
    private GameObject playerInventory;
    private QuickSlotsChanger quickSlot;
    private GameObject chestSlots;

    private QuestTabDataSet questShow;

    private ChestStorage chestStorage;
    private List<Item> chestItems = new List<Item>();
    private bool chestSet = false;

    private PlayerMovement playerMovement;

    private bool canOpenTabs = true;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);

        playerInventory.SetActive(false);

        chestSlots.SetActive(false);

        questShow.DeleteData();
        questShow.gameObject.SetActive(false);
    }

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        playerInventory = transform.Find("Field/Inventory/PlayerInventory").gameObject;
        quickSlot = transform.Find("Field/QuickSlots").gameObject.GetComponent<QuickSlotsChanger>();
        chestSlots = transform.Find("Field/Inventory/PlayerInventory/ChestInventory").gameObject;
        questShow = transform.Find("Field/QuestTab").gameObject.GetComponent<QuestTabDataSet>();
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (canOpenTabs == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (questShow.gameObject.activeSelf || !playerInventory.activeSelf)
                {
                    questShow.DeleteData();
                    questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(true);
                    quickSlot.gameObject.SetActive(false);

                    playerMovement.SetPlayerMovementFalse();
                }
                else if (playerInventory.activeSelf)
                {
                    questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(false);
                    quickSlot.gameObject.SetActive(true);
                    quickSlot.Reinitialize();

                    playerMovement.SetPlayerMovementTrue();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (playerInventory.activeSelf || !questShow.gameObject.activeSelf)
                {
                    questShow.gameObject.SetActive(true);

                    playerInventory.SetActive(false);
                    quickSlot.gameObject.SetActive(false);

                    playerMovement.SetPlayerMovementFalse();
                }
                else if (questShow.gameObject.activeSelf)
                {
                    questShow.DeleteData();
                    questShow.gameObject.SetActive(false);

                    playerInventory.SetActive(false);
                    quickSlot.gameObject.SetActive(true);
                    quickSlot.Reinitialize();

                    playerMovement.SetPlayerMovementTrue();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                questShow.DeleteData();
                questShow.gameObject.SetActive(false);

                playerInventory.SetActive(false);
                quickSlot.gameObject.SetActive(true);
                quickSlot.Reinitialize();

                playerMovement.SetPlayerMovementTrue();
            }
        }
    }

    public void SetChestItems(List<Item> items, ChestStorage chestStorage)
    {
        this.chestItems = items;

        chestSet = true;

        this.chestStorage = chestStorage;
    }

    public void DeleteChestItems()
    {
        this.chestItems = null;

        chestSet = false;
    }

    public void SetCanOpenTabs(bool canOpenTabs)
    {
        this.canOpenTabs = canOpenTabs;
    }
}
