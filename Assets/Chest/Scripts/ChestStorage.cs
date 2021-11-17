using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestStorage : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    [SerializeField] private  int chestMaxSlots;

    public List<Item> Items { get { return items; } }

    public int ChestMaxSlots { get => chestMaxSlots; set => chestMaxSlots = value; }

    public void SetItems(List<Item> items)
    {
        this.items = items;
    }

    public void AddItem(Item item)
    {
        if(!items.Contains(item) && items.Count + 1 < chestMaxSlots)
        {
            items.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
    }

}
