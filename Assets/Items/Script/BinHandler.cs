using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BinHandler : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IDropHandler
{
    [SerializeField] private Sprite openBin;
    [SerializeField] private Sprite closeBin;

    [SerializeField] private ItemDrag itemDrag;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (itemDrag.PreviousSlot != null && itemDrag.PreviousSlot.GetComponent<ItemSlot>().PlayerInventory == true)
        {
            if (itemDrag.PreviousSlot.GetComponent<ItemSlot>().Item.ImportantItem == false)
            {
                itemDrag.PreviousSlot.GetComponent<ItemSlot>().DeleteItem();

                itemDrag.HideData();
            }
            else
            {
                itemDrag.HideData();
            }
        }

        image.sprite = closeBin;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(itemDrag.PreviousSlot != null && itemDrag.PreviousSlot.GetComponent<ItemSlot>().PlayerInventory == true)
        {
            image.sprite = openBin;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = closeBin;
    }
}
