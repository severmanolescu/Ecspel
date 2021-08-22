using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipedITem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler,
                                          IEndDragHandler, IDropHandler, IPointerMoveHandler, IPointerExitHandler
{
    [SerializeField] private ItemDetails itemDetails;

    [SerializeField] private PlayerInventory playerInventory;

    [SerializeField] private ItemDrag itemDrag;
    
    private Item item = null;

    private Image image;

    public Item GetItem()
    {
        return item;
    }

    private void Start()
    {
        Image[] auxiliarImage = gameObject.GetComponentsInChildren<Image>();

        image = auxiliarImage[1];
        image.gameObject.SetActive(false);
    }

    public void SetItem(Item item, ItemSlot previousSlot)
    {
        if(item != null)
        {
            if(this.item == null)
            {
                this.item = item;

                if(previousSlot != null)
                    previousSlot.DeleteItem();

                image.sprite = item.GetSprite();

                image.gameObject.SetActive(true);
            }
            else
            {
                Item auxItemChange = this.item;

                this.item = item;

                if (previousSlot != null)
                    previousSlot.SetItem(auxItemChange);

                image.sprite = item.GetSprite();

                image.gameObject.SetActive(true);
            }
        }
    }

    public void SetItem(Item item)
    {
        if (item != null)
        {
            if (this.item == null)
            {
                this.item = item;

                image.sprite = item.GetSprite();

                image.gameObject.SetActive(true);
            }
            else
            {
                Item auxItemChange = this.item;

                this.item = item;

                image.sprite = item.GetSprite();

                image.gameObject.SetActive(true);
            }
        }
    }

    public void DeleteItem()
    {
        item = null;

        image.gameObject.SetActive(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        itemDetails.SetItem(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemDetails.HideData();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(eventData.clickCount == 2)
        {
            if(playerInventory.AddItem(item) == true)
            {
                DeleteItem();
            }
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

    private bool VerifyItemToSlot(Item item)
    {
        if(gameObject.name == "Sword" && item is Weapon)
        {
            return true;
        }
        else if(gameObject.name == "Axe" && item is Axe)
        {
            return true;
        }
        else if (gameObject.name == "Pickaxe" && item is Pickaxe)
        {
            return true;
        }
        else if (gameObject.name == "Bow" && item is Range)
        {
            return true;
        }

        return false;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (item == null)
        {
            if (itemDrag.GetItem() != null && VerifyItemToSlot(itemDrag.GetItem()) == true)
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
            if (item != null && VerifyItemToSlot(itemDrag.GetItem()))
            {
                Item auxChangeItems = this.item;

                ItemSlot previousItem = itemDrag.GetPreviousItem().GetComponent<ItemSlot>();

                if(previousItem != null)
                {
                    SetItem(itemDrag.GetItem());

                    previousItem.SetItem(auxChangeItems);
                }
                else
                {
                    item = itemDrag.GetItem();
                }

                itemDrag.HideData();

                return;
            }
            itemDrag.HideData();
        }
    }
}
