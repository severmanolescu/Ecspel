using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BinHandler : MonoBehaviour, IDropHandler
{
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
    }
}
