using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopItems
{
    [SerializeField] private int fromDay;

    [SerializeField] private List<ItemWithAmount> items;

    public int FromLevel { get => fromDay; set => fromDay = value; }
    public List<ItemWithAmount> Items { get => items; set => items = value; }
}
