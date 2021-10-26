using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Item", order = 1)]
[Serializable]
public class Item : ScriptableObject
{
    public new string name;

    public string details;

    public int amount;

    public int maxAmount;

    public Sprite itemSprite;

    public Item(string name, string details,  int amount, int maxAmount, Sprite itemSprite)
    {
        this.name = name;
        this.details = details;

        this.amount = amount;
        this.maxAmount = maxAmount;
        this.itemSprite = itemSprite;
    }

    public string Name { get { return name; } }
    public string Details { get { return details; } }
    public int Amount { get { return amount; } set { amount = value; } }
    public Sprite Sprite { get { return itemSprite; } }
    public int MaxAmount { get { return maxAmount; } }

    public Item Copy()
    {
        return (Item)this.MemberwiseClone();
    }
}
