using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, 
                                       IEndDragHandler, IDropHandler, IPointerExitHandler, IPointerEnterHandler, IPointerClickHandler
{
    private ItemDrag itemDrag;
    private ItemDetails itemDetails;

    private Image itemSprite;
    private TextMeshProUGUI amount;

    private ItemSprites itemSprites;

    private PlayerInventory inventory;

    private ChestStorageHandler chestStorage;

    private QuickSlotsChanger quickSlots;

    //true  - player inventory
    //false - chest storage
    private bool locationOfItem;

    private Item item = null;

    public Item Item { get { return item; } set { item = value; } }

    private void Awake()
    {
        itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();

        quickSlots = GameObject.Find("Global/Player/Canvas/Field/QuickSlots").GetComponent<QuickSlotsChanger>();

        Image[] itemsSprite = gameObject.GetComponentsInChildren<Image>();

        itemSprite = itemsSprite[1];
        amount = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        item = null;

        amount.gameObject.SetActive(false);

        itemSprite.gameObject.SetActive(false);

        itemDrag = GameObject.Find("Player/Canvas/ItemDrag").GetComponent<ItemDrag>();
        itemDetails = GameObject.Find("Player/Canvas/ItemDetails").GetComponent<ItemDetails>();

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
    }

    private void ShowItem()
    {
        itemSprite.gameObject.SetActive(true);

        itemSprite.sprite = itemSprites.GetItemSprite(item.ItemNO);

        if (item.Amount > 1)
        {
            amount.text = item.Amount.ToString();

            amount.gameObject.SetActive(true);
        }
        else
        {
            amount.gameObject.SetActive(false);
        }
    }

    private void HideItem()
    {
        itemSprite.gameObject.SetActive(false);
        itemSprite.sprite = null;

        amount.SetText("0");
        amount.gameObject.SetActive(false);
    }

    public bool ExistItem()
    {
        if(item != null)
        {
            return true;
        }

        return false;
    }

    public void SetItem(Item item)
    {
        if (item != null)
        {
            this.item = item;

            ShowItem();
        }
    }    

    public void ReinitializeItem()
    {
        if(item != null && item.Amount > 1)
        {
            amount.text = item.Amount.ToString();

            amount.gameObject.SetActive(true);
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
    }

    public void DeleteItem()
    {
        item = null;

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
    }

    private bool VerifyItem()
    {
        if (item != null && itemDrag.Item != null)
        {
            if (item is Weapon && itemDrag.Item is Weapon)
            {
                return true;
            }
            if (item is Axe && itemDrag.Item is Axe)
            {
                return true;
            }
            if (item is Pickaxe && itemDrag.Item is Pickaxe)
            {
                return true;
            }
            if (item is Range && itemDrag.Item is Range)
            {
                return true;
            }
        }

        return false;
    }

    private void ChangeSlotToEquiped()
    {
        Item auxSwapItems = item;

        SetItem(itemDrag.Item);

        itemDrag.PreviousItem.GetComponent<EquipedITem>().SetItem(auxSwapItems);

        itemDrag.HideData();
    }

    private void ChangeSlotToSlot()
    {
        Item auxSwapItems = item;

        SetItem(itemDrag.Item);

        itemDrag.PreviousItem.GetComponent<ItemSlot>().SetItem(auxSwapItems);

        itemDrag.HideData();

    }

    private void ChangeEquipedItem()
    {
        if(itemDrag.PreviousItem.GetComponent<ItemSlot>() != null)
        {
            ChangeSlotToSlot();
        }
        else
        {
            ChangeSlotToEquiped();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
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

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if(itemDrag.PreviousItem != this.gameObject)
        {
            if (VerifyItem())
            {
                ChangeEquipedItem();

                return;
            }

            if (item == null)
            {
                if (itemDrag.Item != null)
                {
                    SetItem(itemDrag.Item);

                    itemDrag.DeleteData();

                    return;
                }
                else
                {
                    itemDrag.HideData();
                }
            }
            else
            {
                if (item.Name == itemDrag.Item.Name)
                {
                    if (item.Amount <= item.MaxAmount)
                    {
                        int auxiliar = item.MaxAmount - item.Amount;

                        if (itemDrag.Item.Amount <= auxiliar)
                        {
                            item.Amount += itemDrag.Item.Amount;

                            itemDrag.DeleteData();

                            ReinitializeItem();

                            return;
                        }
                        else
                        {
                            item.Amount = item.MaxAmount;

                            itemDrag.Item.Amount -= auxiliar;

                            ReinitializeItem();

                            itemDrag.HideData();

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

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDetails.HideData();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        itemDetails.SetItem(item);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == 0 && (eventData.clickCount == 2 ||
           (eventData.clickCount == 1 && Input.GetKey(KeyCode.LeftShift))))
        {
            if(locationOfItem == true)
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

            quickSlots.Reinitialize();
        }
    }
}
