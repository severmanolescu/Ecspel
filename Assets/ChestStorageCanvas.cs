using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestStorageCanvas : MonoBehaviour
{
    private ItemSlot[] chestItems;

    private void Awake()
    {
        chestItems = gameObject.GetComponentsInChildren<ItemSlot>();
    }

    public void SetItems(List<Item> items)
    {
        if (chestItems != null && items != null)
        {
            foreach (ItemSlot item in chestItems)
            {
                item.DeleteItem();
            }

            int i = -1;

            foreach (Item item in items)
            {
                chestItems[++i].SetItem(item);
            }
        }
    }

    public List<Item> ReturnItemsList()
    {
        List<Item> items = new List<Item>();

        foreach(ItemSlot item in chestItems)
        {
            items.Add(item.Item);
        }

        foreach (ItemSlot item in chestItems)
        {
            item.DeleteItem();
        }

        return items;
    }
}
