using UnityEngine;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour
{
    private Item item;

    private GameObject previousSlot;

    private ItemSprites itemSprites;

    private Image itemImage;

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

    public void DeleteData()
    {
        ItemSlot auxCheckSlot = previousSlot.GetComponent<ItemSlot>();

        if (auxCheckSlot != null)
        {
            auxCheckSlot.DeleteItem();
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
        if(Input.GetMouseButton(0))
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