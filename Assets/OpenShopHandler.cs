using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class OpenShopHandler : MonoBehaviour
{
    [SerializeField] private List<ItemWithAmount> items;

    //Types:
    //0 - Nothing
    //1 - normal items
    //2 - library items: books, crafting recipes
    [SerializeField] private int typeOfBuyItems;

    private TextMeshProUGUI text;

    private bool playerInArea = false;

    private PlayerMovement playerMovement;

    private GameObject playerInventory;

    private ShopInventory shopInventory;

    private CanvasTabsOpen canvasTabsOpen;

    private GameObject quickSlots;

    public List<ItemWithAmount> Items { get => items; set => items = value; }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.Find("Player/Canvas/PlayerItems");
        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
        shopInventory = GameObject.Find("Player/Canvas/ShopItems").GetComponent<ShopInventory>();

        quickSlots = GameObject.Find("Player/Canvas/Field/QuickSlots");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(true);

            playerInArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text.gameObject.SetActive(false);

            playerInArea = false;

            DeactivateAll();
        }
    }

    private void DeactivateAll()
    {
        playerMovement.TabOpen = false;
        playerInventory.SetActive(false);
        quickSlots.SetActive(true);
        quickSlots.GetComponent<QuickSlotsChanger>().Reinitialize();
        canvasTabsOpen.SetCanOpenTabs(true);

        items = shopInventory.GetAllItems();

        shopInventory.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(playerInArea)
        {
            if(Keyboard.current.fKey.wasPressedThisFrame)
            {
                if(shopInventory.gameObject.activeSelf == false)
                {
                    playerMovement.TabOpen = true;
                    playerInventory.SetActive(true);
                    quickSlots.SetActive(false);
                    canvasTabsOpen.SetCanOpenTabs(false);

                    shopInventory.gameObject.SetActive(true);

                    shopInventory.SetItems(Items, typeOfBuyItems);
                }
                else
                {
                    DeactivateAll();
                }
            }
        }
    }
}
