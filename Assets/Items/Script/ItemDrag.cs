using UnityEngine;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour
{
    private Item item;

    private GameObject previousSlot;

    private ItemSprites itemSprites;

    private Image itemImage;

    private bool halfOfAmount;

    private bool startDrag = false;

    public Item Item { get { return item; } }
    public GameObject PreviousItem { get { return previousSlot; } }

    private void Awake()
    {
        itemSprites = GameObject.Find("Global").GetComponent<ItemSprites>();

        itemImage = gameObject.GetComponentInChildren<Image>();

        itemImage.gameObject.SetActive(false);
    }

    public void SetData(Item item, GameObject previousSlot)
    {
        if(item != null)
        {
            if(this.item == null)
            {
                this.item = item;
                this.previousSlot = previousSlot;

                itemImage.sprite = itemSprites.GetItemSprite(item.ItemNO);

                itemImage.gameObject.SetActive(true);

                transform.position = Input.mousePosition;

                halfOfAmount = false;
            }
        }
    }

    public void SetDataHalf(Item item, GameObject previousSlot)
    {
        if (item != null)
        {
            if (this.item == null)
            {
                this.item = item.Copy();
                this.item.Amount = (int)Mathf.Ceil(this.item.Amount / 2f);

                this.previousSlot = previousSlot;

                itemImage.sprite = itemSprites.GetItemSprite(item.ItemNO);

                itemImage.gameObject.SetActive(true);

                transform.position = Input.mousePosition;

                halfOfAmount = true;
            }
        }
    }

    public void HideData()
    {
        if (previousSlot != null)
        {
            ItemSlot auxCheckSlot = previousSlot.GetComponent<ItemSlot>();

            if (auxCheckSlot != null)
            {
                auxCheckSlot.ReinitializeItem();
            }

            item = null;
            previousSlot = null;

            itemImage.gameObject.SetActive(false);
        }
    }

    public void ChangeItemHalfAmount()
    {
        ItemSlot auxCheckSlot = previousSlot.GetComponent<ItemSlot>();

        if (auxCheckSlot != null)
        {
            auxCheckSlot.Item.Amount -= item.Amount;

            auxCheckSlot.ReinitializeItem();
        }

        item = null;
        previousSlot = null;

        itemImage.gameObject.SetActive(false);
    }

    public void DeleteData()
    {
        ItemSlot auxCheckSlot = previousSlot.GetComponent<ItemSlot>();

        if (auxCheckSlot != null)
        {
            if (halfOfAmount == true)
            {
                ChangeItemHalfAmount();
            }
            else
            {
                auxCheckSlot.DeleteItem();
            }
        }
        else
        {
            EquipedITem auxCheckEquiped = previousSlot.GetComponent<EquipedITem>();

            if (auxCheckEquiped != null)
            {
                auxCheckEquiped.DeleteItem();
            }
            else
            {

                auxCheckSlot.DeleteItem();
            }
        }

        item = null;
        previousSlot = null;

        itemImage.gameObject.SetActive(false);
    }

    public void Update()
    {
        if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            startDrag = true;

            transform.position = Input.mousePosition;
        }
        else if(startDrag)
        {
            startDrag = false;

            HideData();
        }
    }
}