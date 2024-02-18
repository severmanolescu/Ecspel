using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenShopHandler : MonoBehaviour
{
    [Header("Types: \n" +
        "0 - Nothing \n" +
        "1 - normal items \n" +
        "2 - library items: books, crafting recipes")]
    [SerializeField] private int typeOfBuyItems;

    [SerializeField] private int minItems;
    [SerializeField] private int maxItems;

    [SerializeField] private List<ItemWithAmount> shopItems;

    [SerializeField] private List<Item> items;

    private TextMeshProUGUI text;

    private bool playerInArea = false;

    private PlayerMovement playerMovement;

    private GameObject playerInventory;

    private ShopInventory shopInventory;

    private CanvasTabsOpen canvasTabsOpen;

    private GameObject quickSlots;

    private bool wasOpen = false;

    private bool fKeyPress = true;

    public bool discount = false;

    public List<Item> Items { get => items; set => items = value; }
    public bool Discount { get => discount; set => discount = value; }

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();

        text.gameObject.SetActive(false);

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        playerInventory = GameObject.Find("Player/Canvas/PlayerItems");
        canvasTabsOpen = GameObject.Find("Player/Canvas").GetComponent<CanvasTabsOpen>();
        shopInventory = GameObject.Find("Player/Canvas/ShopItems").GetComponent<ShopInventory>();

        quickSlots = GameObject.Find("Player/Canvas/Field/QuickSlots");

        wasOpen = false;

        RefreshItems(0);

        GameObject.Find("Global/DayTimer").GetComponent<RefreshShopItems>().AddShop(this);
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

    public void RefreshItems(int days)
    {
        items.Clear();

        int noOfItems = UnityEngine.Random.Range(minItems, maxItems);

        for (int noOfItem = 0; noOfItem < noOfItems; noOfItem++)
        {
            int indexOfItemToAdd = UnityEngine.Random.Range(0, shopItems.Count - 1);

            Item newItem = shopItems[indexOfItemToAdd].Item.Copy();
            newItem.Amount = shopItems[indexOfItemToAdd].Amount;

            if (!items.Contains(newItem))
            {
                items.Add(newItem);
            }
        }
    }

    private void DeactivateAll()
    {
        playerMovement.TabOpen = false;
        playerInventory.SetActive(false);
        quickSlots.SetActive(true);
        quickSlots.GetComponent<QuickSlotsChanger>().Reinitialize();
        canvasTabsOpen.SetCanOpenTabs(true);

        if (wasOpen == true)
        {
            items = shopInventory.GetAllItems();

            wasOpen = false;
        }

        shopInventory.Close();
    }

    private void Update()
    {
        if (playerInArea)
        {
            if (Keyboard.current.fKey.wasPressedThisFrame || (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == false && fKeyPress == false))
            {
                if (shopInventory.gameObject.activeSelf == false)
                {
                    playerMovement.TabOpen = true;
                    playerInventory.SetActive(true);
                    quickSlots.SetActive(false);
                    canvasTabsOpen.SetCanOpenTabs(false);

                    shopInventory.gameObject.SetActive(true);

                    shopInventory.SetItems(Items, typeOfBuyItems, discount);

                    wasOpen = true;
                }
                else
                {
                    DeactivateAll();
                }
            }

            if (Joystick.current != null && Joystick.current.allControls[3].IsPressed() == true)
            {
                fKeyPress = false;
            }
        }
    }
}
