using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropWrong : MonoBehaviour, IDropHandler
{
    [SerializeField] private ItemDrag itemDrag;

    public void OnDrop(PointerEventData eventData)
    {
        itemDrag.HideData();
    }
}
