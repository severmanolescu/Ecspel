using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, 
                                       IEndDragHandler, IDropHandler, IPointerExitHandler, IPointerEnterHandler
{
    private ItemDrag itemDrag;
    private ItemDetails itemDetails;

    private Image itemSprite;
    private TextMeshProUGUI amount;

    private Item item = null;

    public Item Item { get { return item; } }

    private void Awake()
    {
        Image[] itemsSprite = gameObject.GetComponentsInChildren<Image>();

        itemSprite = itemsSprite[1];
        amount = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        item = null;

        amount.gameObject.SetActive(false);

        itemSprite.gameObject.SetActive(false);

        itemDrag = GameObject.Find("Player/Canvas/ItemDrag").GetComponent<ItemDrag>();
        itemDetails = GameObject.Find("Player/Canvas/ItemDetails").GetComponent<ItemDetails>();
    }

    private void ShowItem()
    {
        itemSprite.gameObject.SetActive(true);

        itemSprite.sprite = item.Sprite;

        if(item.Amount > 1)
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
        if(item.Amount > 1)
        {
            amount.text = item.Amount.ToString();

            amount.gameObject.SetActive(true);
        }
        else
        {
            amount.gameObject.SetActive(false);
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
}
