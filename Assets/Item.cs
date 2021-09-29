using UnityEngine;
using System;

[Serializable]
public class Item
{
    private string name;

    protected string details;

    protected int amount;

    protected int maxAmount;

    protected Sprite itemSprite;

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

public class Weapon: Item
{
    protected float attackPower;

    public Weapon(string name, string details, int amount, int maxAmount, Sprite itemSprite, float attackPower)
        : base(name, details, amount, maxAmount, itemSprite)
    {
        this.attackPower = attackPower;
    }

    public float AttackPower { get { return attackPower; } }
}

public class Range: Weapon
{
    protected float range;

    public Range(string name, string details, int amount, int maxAmount, Sprite itemSprite, float attackPower, float range)
        : base(name, details, amount, maxAmount, itemSprite, attackPower)
    {
        this.range = range;
    }

    public float GetRange { get { return range; } }
}

public class Axe : Item
{
    public Axe(string name, string details, int amount, int maxAmount, Sprite itemSprite) : base(name, details, amount, maxAmount, itemSprite)
    {

    }
}

public class Pickaxe : Item
{
    public Pickaxe(string name, string details, int amount, int maxAmount, Sprite itemSprite) : base(name, details, amount, maxAmount, itemSprite)
    {

    }
}
