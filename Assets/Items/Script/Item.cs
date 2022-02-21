using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item", order = 1)]
[Serializable]
public class Item : ScriptableObject
{
    [SerializeField] private new string name;

    [SerializeField] private string details;

    [SerializeField] private int sellPrice;

    [SerializeField] private int amount;

    [SerializeField] private int maxAmount;

    [SerializeField] private int itemNO;

    public Item(string name, string details, int amount, int maxAmount, int itemNO, int sellPrice)
    {
        this.name = name;
        this.details = details;

        this.amount = amount;
        this.maxAmount = maxAmount;
        this.itemNO = itemNO;
        SellPrice = sellPrice;
    }

    public string Name { get { return name; } }
    public string Details { get { return details; } }
    public int Amount { get { return amount; } set { amount = value; } }
    public int ItemNO { get { return itemNO; } }
    public int MaxAmount { get { return maxAmount; } }

    public int SellPrice { get => sellPrice; set => sellPrice = value; }

    public Item Copy()
    {
        return (Item)this.MemberwiseClone();
    }
}
