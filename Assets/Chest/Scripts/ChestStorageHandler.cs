using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestStorageHandler : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotParent;

    private List<ItemSlot> itemSlots = new List<ItemSlot>();

    public void SetChestStorage(List<Item> items, int maxSlots)
    {
        ReinitializeStorage();

        int slots = items.Count;

        foreach(Item item in items)
        {
            InstantiateSlot(item);
        }

        for(int emptySlots = slots; emptySlots <= maxSlots; emptySlots++)
        {
            InstantiateSlot(null);
        }
    }

    private void InstantiateSlot(Item item)
    {
        Item newItem = null;

        if(item != null)
        {
            newItem = item.Copy();
        }

        ItemSlot itemSlot = Instantiate(slotPrefab).GetComponent<ItemSlot>();

        itemSlot.transform.SetParent(slotParent);

        itemSlot.SetItem(newItem);

        itemSlots.Add(itemSlot);
    }

    private void ReinitializeStorage()
    {
        foreach(ItemSlot itemSlot in itemSlots)
        {
            Destroy(itemSlot.gameObject);
        }

        itemSlots.Clear();
    }

    public List<Item> GetChestStorage()
    {
        List<Item> items = new List<Item>();

        foreach(ItemSlot itemSlot in itemSlots)
        {
            items.Add(itemSlot.Item);
        }

        ReinitializeStorage();

        return items;
    }
}
