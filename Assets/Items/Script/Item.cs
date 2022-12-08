using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item", order = 1)]
[Serializable]
public class Item : ScriptableObject
{
    [SerializeField] private new string name;

    [TextArea(5, 5)]
    [SerializeField] private string details;

    [SerializeField] private int sellPrice;

    [SerializeField] private int amount;

    [SerializeField] private int maxAmount;

    [SerializeField] private int itemNO;

    [SerializeField] private Sprite itemSprite;

    [SerializeField] private bool importantItem = false;

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
    public bool ImportantItem { get => importantItem; set => importantItem = value; }
    public Sprite ItemSprite { get => itemSprite; set => itemSprite = value; }

    public Item Copy()
    {
        return (Item)this.MemberwiseClone();
    }
}
