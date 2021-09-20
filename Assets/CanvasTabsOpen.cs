using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTabsOpen : MonoBehaviour
{
    [Header("Player Inventory")]
    [SerializeField] private GameObject PlayerInventory;

    [Header("Player Quickslots")]
    [SerializeField] private GameObject quickSlot;

    [Header("Chest Slots")]
    [SerializeField] private GameObject chestSlots;

    private ChestStorage chestStorage;
    private List<Item> chestItems = new List<Item>();
    private bool chestSet = false;

    private PlayerMovement playerMovement;

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1.5f);

        PlayerInventory.SetActive(false);

        chestSlots.SetActive(false);
    }

    private void Awake()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        StartCoroutine(Wait());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PlayerInventory.SetActive(!PlayerInventory.activeSelf);

            if(PlayerInventory.activeSelf == false)
            {
                quickSlot.GetComponent<QuickSlotsChanger>().Reinitialize();

                quickSlot.gameObject.SetActive(true);

                playerMovement.SetPlayerMovementTrue();
            }
            else
            {
                quickSlot.gameObject.SetActive(false);

                playerMovement.SetPlayerMovementFalse();
            }

            if(chestSet)
            {
                if(chestSlots.activeSelf)
                {
                    chestStorage.SetItems(chestSlots.GetComponent<ChestStorageCanvas>().ReturnItemsList());

                    chestSlots.SetActive(false);

                    playerMovement.SetPlayerMovementFalse();
                }
                else
                {
                    chestSlots.SetActive(true);

                    playerMovement.SetPlayerMovementTrue();

                    chestItems = chestStorage.GetComponent<ChestStorage>().GetItems();

                    if (chestItems != null)
                        chestSlots.GetComponent<ChestStorageCanvas>().SetItems(chestItems);
                }
            }
            else
            {
                chestSlots.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PlayerInventory.activeSelf)
            {
                PlayerInventory.SetActive(false);
                quickSlot.SetActive(true);

                playerMovement.SetPlayerMovementFalse();
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
}
