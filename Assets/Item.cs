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

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Weapon", order = 1)]
public class Weapon: Item
{
    public float attackPower;

    public Weapon(string name, string details, int amount, int maxAmount, Sprite itemSprite, float attackPower)
        : base(name, details, amount, maxAmount, itemSprite)
    {
        this.attackPower = attackPower;
    }

    public float AttackPower { get { return attackPower; } }
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Range Weapon", order = 1)]
public class Range: Weapon
{
    public float range;

    public Range(string name, string details, int amount, int maxAmount, Sprite itemSprite, float attackPower, float range)
        : base(name, details, amount, maxAmount, itemSprite, attackPower)
    {
        this.range = range;
    }

    public float GetRange { get { return range; } }
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Axe", order = 1)]
public class Axe : Item
{
    public Axe(string name, string details, int amount, int maxAmount, Sprite itemSprite) : base(name, details, amount, maxAmount, itemSprite)
    {

    }
}

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Pickaxe", order = 1)]
public class Pickaxe : Item
{
    public Pickaxe(string name, string details, int amount, int maxAmount, Sprite itemSprite) : base(name, details, amount, maxAmount, itemSprite)
    {

    }
}
