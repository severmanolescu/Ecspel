using System.Collections.Generic;
using UnityEngine;

public class ChestStorage : MonoBehaviour
{
    private List<Item> items = new List<Item>();

    [SerializeField] private int chestMaxSlots;

    public List<Item> Items { get { return items; } }

    public int ChestMaxSlots { get => chestMaxSlots; set => chestMaxSlots = value; }

    public void SetItems(List<Item> items)
    {
        this.items = items;
    }

    public void AddItem(Item item)
    {
        if (items.Count < chestMaxSlots)
        {
            items.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

    public void RemoveAllItems()
    {
        ChestStorageInitial chestStorageInitial = GetComponent<ChestStorageInitial>();

        if (chestStorageInitial != null)
        {
            chestStorageInitial.RemoveAll();
        }

        items.Clear();
    }
}
