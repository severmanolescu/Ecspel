using UnityEngine;
using System;

[Serializable]
public class Item
{
    public string name;

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

    public string GetName()
    {
        return name;
    }

    public string GetDetails()
    {
        return details;
    }

    public int GetAmount()
    {
        return amount;
    }

    public Sprite GetSprite()
    {
        return itemSprite;
    }

    public int GetMaximAmount()
    {
        return maxAmount;
    }

    public void ChangeAmount(int amount)
    {
        this.amount = amount;
    }

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

    public float GetAttackPower()
    {
        return attackPower;
    }
}

public class Range: Weapon
{
    protected float range;

    public Range(string name, string details, int amount, int maxAmount, Sprite itemSprite, float attackPower, float range)
        : base(name, details, amount, maxAmount, itemSprite, attackPower)
    {
        this.range = range;
    }

    public float GetRange()
    {
        return range;
    }
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
