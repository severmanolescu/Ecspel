using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler,
                                       IEndDragHandler, IDropHandler, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private bool canDrop = true;

    private bool shopItems = false;

    [SerializeField] private bool playerInventory = false;

    [SerializeField] private Item emptyWateringCan;
    [SerializeField] private Item fullWateringCan;

    private ItemDrag itemDrag;
    private ItemDetails itemDetails;

    private Image itemSprite;
    private TextMeshProUGUI amount;

    private PlayerInventory inventory;

    private CraftCanvasHandler craftCanvas;

    private ChestStorageHandler chestStorage;

    private QuickSlotsChanger quickSlots;

    private Keyboard keyboard;

    private ForgeHandler forgeHandler;

    private SetDataToBuySlider setData;

    private ShopInventory shopInventory;

    private CoinsHandler coinsHandler;

    private PlayerStats playerStats;

    private ItemUseByMouse itemUse;

    private bool dontShowDetails = false;

    //true  - player inventory
    //false - chest storage
    private bool locationOfItem;

    private bool discount = false;

    private Item item = null;

    public Item Item { get { return item; } set { item = value; ReinitializeItem(); } }

    public bool ShopItems { get => shopItems; set { shopItems = value; SearchForBuySlider(); } }

    public bool DontShowDetails { get => dontShowDetails; set => dontShowDetails = value; }
    public bool PlayerInventory { get => playerInventory; }

    private void Awake()
    {
        quickSlots = GameObject.Find("Global/Player/Canvas/QuickSlots").GetComponent<QuickSlotsChanger>();

        Image[] itemsSprite = GetComponentsInChildren<Image>();

        itemSprite = itemsSprite[1];
        amount = GetComponentInChildren<TextMeshProUGUI>();

        item = null;

        amount.gameObject.SetActive(false);

        itemSprite.gameObject.SetActive(false);

        shopInventory = GameObject.Find("Global/Player/Canvas/ShopItems").GetComponent<ShopInventory>();

        itemDrag = GameObject.Find("Player/Canvas/ItemDrag").GetComponent<ItemDrag>();
        itemDetails = GameObject.Find("Player/Canvas/ItemDetails").GetComponent<ItemDetails>();

        craftCanvas = GameObject.Find("Global/Player/Canvas/Crafting").GetComponent<CraftCanvasHandler>();

        itemUse = GameObject.Find("Global/Player/Canvas/ItemUseButton").GetComponent<ItemUseByMouse>();

        inventory = GetComponentInParent<PlayerInventory>();

        if (inventory == null)
        {
            locationOfItem = false;

            chestStorage = GetComponentInParent<ChestStorageHandler>();

            inventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();
        }
        else
        {
            locationOfItem = true;

            chestStorage = GameObject.Find("Global/Player/Canvas/ChestStorage").GetComponent<ChestStorageHandler>();
        }

        if (playerInventory == true)
        {
            coinsHandler = GameObject.Find("Global/Player/Canvas/PlayerItems/Coins").GetComponent<CoinsHandler>();

            playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();
        }

        playerStats = GameObject.Find("Global/Player").GetComponent<PlayerStats>();

        keyboard = InputSystem.GetDevice<Keyboard>();

        SearchForBuySlider();
    }

    private void SearchForBuySlider()
    {
        setData = GameObject.Find("Global/Player/Canvas/GetItems").GetComponent<SetDataToBuySlider>();
    }

    public void ShowItem()
    {
        itemSprite.gameObject.SetActive(true);

        itemSprite.sprite = item.ItemSprite;

        if (item.Amount > 1)
        {
            amount.text = item.Amount.ToString();

            amount.gameObject.SetActive(true);
        }
        else
        {
            amount.gameObject.SetActive(false);
        }

        if (item is WateringCan)
        {
            WateringCan wateringCan = (WateringCan)item;

            amount.text = wateringCan.RemainWater.ToString();

            amount.gameObject.SetActive(true);
        }
    }

    public void HideItem()
    {
        if (itemSprite != null)
        {
            itemSprite.gameObject.SetActive(false);
            itemSprite.sprite = null;

            amount.SetText("0");
            amount.gameObject.SetActive(false);
        }
    }

    public bool ExistItem()
    {
        if (item != null)
        {
            return true;
        }

        return false;
    }

    public void SetItem(Item item, bool discount = false)
    {
        if (item != null && item.Amount > 0)
        {
            this.discount = discount;

            if (item.ItemNO == 69 && playerInventory == true)
            {
                coinsHandler.Amount += item.Amount;

                return;
            }
     
            this.item = item;

            ShowItem();
        }
        else
        {
            HideItem();

            item = null;
        }
    }

    public void ReinitializeItem()
    {
        if (item != null)
        {
            if (item.Amount > 0)
            {
                ShowItem();
            }
            else
            {
                DeleteItem();
            }

            if (item is WateringCan)
            {
                WateringCan wateringCan = (WateringCan)item;

                amount.text = wateringCan.RemainWater.ToString();

                amount.gameObject.SetActive(true);
            }
        }
        else
        {
            if (item == null || item.Amount == 0)
            {
                HideItem();

                item = null;
            }
            else
            {
                amount.gameObject.SetActive(false);
            }
        }

        ForgeSlotItemChange();
    }

    public void DeleteItem()
    {
        item = null;

        ForgeSlotItemChange();

        HideItem();
    }

    public void DecreseAmount(int amount)
    {
        item.Amount -= amount;

        if (item.Amount <= 0)
        {
            item = null;

            HideItem();
        }
        else
        {
            ReinitializeItem();
        }

        ForgeSlotItemChange();
    }

    private void ForgeSlotItemChange()
    {
        if (forgeHandler != null)
        {
            forgeHandler.ChangeItemInSlot(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!dontShowDetails)
        {
            if (shopItems == false)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    itemDrag.SetData(item, this.gameObject);
                }
                else
                {
                    itemDrag.SetDataHalf(item, this.gameObject);
                }
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!dontShowDetails)
        {
            if (shopItems == false)
            {
                if (itemDrag.PreviousItem != this.gameObject && canDrop == true)
                {
                    if (item == null)
                    {
                        if (itemDrag.Item != null)
                        {
                            SetItem(itemDrag.Item);

                            itemDrag.DeleteData();

                            ForgeSlotItemChange();

                            return;
                        }
                        else
                        {
                            itemDrag.HideData();
                        }
                    }
                    else if (itemDrag != null)
                    {
                        if (item != null && itemDrag.Item != null && item.Name == itemDrag.Item.Name)
                        {
                            if (item.Amount <= item.MaxAmount)
                            {
                                ShowItem();

                                int auxiliar = item.MaxAmount - item.Amount;

                                if (itemDrag.Item.Amount <= auxiliar)
                                {
                                    item.Amount += itemDrag.Item.Amount;

                                    itemDrag.DeleteData();

                                    ReinitializeItem();

                                    ForgeSlotItemChange();

                                    return;
                                }
                                else
                                {
                                    item.Amount = item.MaxAmount;

                                    itemDrag.Item.Amount -= auxiliar;

                                    ReinitializeItem();

                                    itemDrag.HideData();

                                    ForgeSlotItemChange();

                                    return;
                                }
                            }
                        }

                        itemDrag.HideData();
                    }
                }
                else
                {
                    itemDrag.HideData();
                }
            }
            else
            {
                itemDrag.HideData();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDetails.HideData();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!dontShowDetails)
        {
            if (shopItems == true)
            {
                if (item != null)
                {
                    itemDetails.SetItem(Item, Item.SellPrice);

                    MoveItemDetailsToSlot();
                }
            }
            else
            {
                itemDetails.SetItem(item);

                MoveItemDetailsToSlot();
            }
        }
    }

    private void MoveItemDetailsToSlot()
    {
        Vector3 newPosition = transform.position;

        newPosition.x += 55;
        newPosition.y -= 55;

        itemDetails.transform.position = newPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!dontShowDetails)
        {
            if (shopInventory.gameObject.activeSelf == true && shopItems == false)
            {
                if (shopInventory.TypeOfBuyItems != 0 &&
                   !(item is Letter) &&
                   ((shopInventory.TypeOfBuyItems == 2 && item is CraftRecipe) ||
                     shopInventory.TypeOfBuyItems == 1 && !(item is CraftRecipe)))
                    if (eventData.button == PointerEventData.InputButton.Left)
                    {
                        if (item != null && (keyboard.shiftKey.isPressed || item.Amount == 1))
                        {
                            setData.SellItem(this);
                        }
                        else
                        {
                            setData.gameObject.SetActive(true);

                            setData.SetDataToSell(this, discount);
                        }
                    }
            }
            else if (shopItems == false)
            {
                if (eventData.button == PointerEventData.InputButton.Left && (eventData.clickCount == 2 ||
                   (eventData.clickCount == 1 && keyboard.shiftKey.isPressed)))
                {
                    if (locationOfItem == true)
                    {
                        if (chestStorage.gameObject.activeSelf == true)
                        {
                            bool canTransferToPlayerInventory = chestStorage.AddItem(Item);

                            if (canTransferToPlayerInventory == true)
                            {
                                HideItem();

                                Item = null;
                            }
                            else
                            {
                                ReinitializeItem();
                            }
                        }
                    }                    
                    else
                    {
                        bool canTransferToPlayerInventory = inventory.AddItem(Item);

                        if (canTransferToPlayerInventory == true)
                        {
                            HideItem();

                            Item = null;
                        }
                        else
                        {
                            ReinitializeItem();
                        }
                    }

                    ForgeSlotItemChange();

                    quickSlots.Reinitialize();
                }
                else if (eventData.button == PointerEventData.InputButton.Right &&
                        (item is CraftRecipe ||
                         item is Consumable))
                {
                    itemUse.ShowButton(this);
                }
            }
            else
            {
                if (eventData.button == PointerEventData.InputButton.Left && item != null)
                {
                    if (keyboard.shiftKey.isPressed || item.Amount == 1)
                    {
                        setData.BuyItem(this);
                    }
                    else
                    {
                        setData.gameObject.SetActive(true);

                        setData.SetDataToBuy(this, discount);
                    }
                }
            }
        }
    }

    public void ItemUse()
    {
        if(item is CraftRecipe)
        {
            CraftRecipe craft = (CraftRecipe)item;

            if (craftCanvas.AddCraft(craft.Craft) == true)
            {
                if (item.Amount > 0)
                {
                    DecreseAmount(1);
                }
                else
                {
                    return;
                }
            }
        }
        else if(item is Consumable)
        {
            Consumable consumable = (Consumable)item;

            if (playerStats.Eat(consumable))
            {
                DecreseAmount(1);
            }
        }
    }
}
