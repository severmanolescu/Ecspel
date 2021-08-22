using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, 
                                       IEndDragHandler, IDropHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private ItemDrag itemDrag;
    [SerializeField] private ItemDetails itemDetails;
    [SerializeField] private EquipItem equipItem;

    private Image itemSprite;
    private TextMeshProUGUI amount;

    public Item item = null;

    private void Start()
    {
        Image[] itemsSprite = gameObject.GetComponentsInChildren<Image>();

        itemSprite = itemsSprite[1];
        amount = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        item = null;

        amount.gameObject.SetActive(false);

        itemSprite.gameObject.SetActive(false);
    }

    private void ShowItem()
    {
        itemSprite.gameObject.SetActive(true);

        itemSprite.sprite = item.GetSprite();

        if(item.GetAmount() > 1)
        {
            amount.text = item.GetAmount().ToString();

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

    public Item GetItem()
    {
        return item;
    }    

    public void ReinitializeItem()
    {
        if(item.GetAmount() > 1)
        {
            amount.text = item.GetAmount().ToString();

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

    private bool VerifyItem()
    {
        if (item != null && itemDrag.GetItem() != null)
        {
            if (item is Weapon && itemDrag.GetItem() is Weapon)
            {
                return true;
            }
            if (item is Axe && itemDrag.GetItem() is Axe)
            {
                return true;
            }
            if (item is Pickaxe && itemDrag.GetItem() is Pickaxe)
            {
                return true;
            }
            if (item is Range && itemDrag.GetItem() is Range)
            {
                return true;
            }
        }

        return false;
    }

    private void ChangeSlotToEquiped()
    {
        Item auxSwapItems = item;

        SetItem(itemDrag.GetItem());

        itemDrag.GetPreviousItem().GetComponent<EquipedITem>().SetItem(auxSwapItems);

        itemDrag.HideData();
    }

    private void ChangeSlotToSlot()
    {
        Item auxSwapItems = item;

        SetItem(itemDrag.GetItem());

        itemDrag.GetPreviousItem().GetComponent<ItemSlot>().SetItem(auxSwapItems);

        itemDrag.HideData();

    }

    private void ChangeEquipedItem()
    {
        if(itemDrag.GetPreviousItem().GetComponent<ItemSlot>() != null)
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
        if(eventData.clickCount == 2 && item != null)
        {
            equipItem.Equip(item, this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemDrag.SetData(item, this.gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if(VerifyItem())
        {
            ChangeEquipedItem();

            return;
        }

        if(item == null)
        {
            if(itemDrag.GetItem() != null)
            {
                SetItem(itemDrag.GetItem());

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
            if(item.GetName() == itemDrag.GetItem().GetName())
            {
                if(item.GetAmount() <= item.GetMaximAmount())
                {
                    int auxiliar = item.GetMaximAmount() - item.GetAmount();

                    if(itemDrag.GetItem().GetAmount() <= auxiliar)
                    {
                        item.ChangeAmount(item.GetAmount() + itemDrag.GetItem().GetAmount());

                        itemDrag.DeleteData();

                        ReinitializeItem();

                        return;
                    }
                    else
                    {
                        item.ChangeAmount(item.GetMaximAmount());

                        itemDrag.GetItem().ChangeAmount(itemDrag.GetItem().GetAmount() - auxiliar);

                        ReinitializeItem();

                        itemDrag.HideData();

                        return;
                    }
                }
            }

            itemDrag.HideData();
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        itemDetails.SetItem(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDetails.HideData();
    }
}
