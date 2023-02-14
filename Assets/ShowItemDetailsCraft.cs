using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowItemDetailsCraft : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [Header("0 - Receive\n1 - Item1\n2 - Item2\n3 - Item3\n4 - Item4")]
    [SerializeField] private int itemNo;

    private CraftSetData craftSetData;

    private void Awake()
    {
        craftSetData = GetComponentInParent<CraftSetData>();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        craftSetData.HideItemDetails();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        craftSetData.MoveItemDetailsToSlot(itemNo);
    }
}
