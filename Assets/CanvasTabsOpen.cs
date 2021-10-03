using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTabsOpen : MonoBehaviour
{
    private GameObject playerInventory;
    private GameObject quickSlot;
    private GameObject chestSlots;

    private QuestShow questShow;

    private ChestStorage chestStorage;
    private List<Item> chestItems = new List<Item>();
    private bool chestSet = false;

    private PlayerMovement playerMovement;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);

        playerInventory.SetActive(false);

        chestSlots.SetActive(false);

        questShow.gameObject.SetActive(false);
    }

    private void Awake()
    {
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();

        playerInventory = gameObject.transform.Find("Field/Inventory/PlayerInventory").gameObject;
        quickSlot = gameObject.transform.Find("Field/QuickSlots").gameObject;
        chestSlots = gameObject.transform.Find("Field/Inventory/PlayerInventory/ChestInventory").gameObject;
        questShow = gameObject.transform.Find("Field/QuestTab").gameObject.GetComponent<QuestShow>();
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (questShow.gameObject.activeSelf || !playerInventory.activeSelf)
            {
                questShow.HideQuest();

                playerInventory.SetActive(true);
                quickSlot.SetActive(false);

                playerMovement.SetPlayerMovementFalse();
            }
            else if (playerInventory.activeSelf)
            {
                questShow.HideQuest();

                playerInventory.SetActive(false);
                quickSlot.SetActive(true);

                playerMovement.SetPlayerMovementTrue();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (playerInventory.activeSelf || !questShow.gameObject.activeSelf)
            {
                questShow.gameObject.SetActive(true);
                playerInventory.SetActive(false);
                quickSlot.SetActive(false);

                playerMovement.SetPlayerMovementFalse();
            }
            else if (questShow.gameObject.activeSelf)
            {
                questShow.HideQuest();

                playerInventory.SetActive(false);
                quickSlot.SetActive(true);

                playerMovement.SetPlayerMovementTrue();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            questShow.HideQuest();

            playerInventory.SetActive(false);
            quickSlot.SetActive(true);

            playerMovement.SetPlayerMovementTrue();
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
}
