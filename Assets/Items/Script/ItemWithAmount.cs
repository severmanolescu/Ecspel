using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemWithAmount
{
    [SerializeField] private Item item;
    [SerializeField] private int amount;

    public ItemWithAmount(Item item, int amount)
    {
        this.Item = item;
        this.Amount = amount;
    }

    public Item Item { get => item; set => item = value; }
    public int Amount { get => amount; set => amount = value; }
}
