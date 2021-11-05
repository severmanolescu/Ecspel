using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Pickaxe", order = 1)]
public class Pickaxe : Item
{
    public float damage;
    public int level;

    public Pickaxe(string name, string details, int amount, int maxAmount, Sprite itemSprite, float damage, int level)
    : base(name, details, amount, maxAmount, itemSprite)
    {
        this.damage = damage;
        this.level = level;
    }

    public float Damage { get { return damage; } }
    public int Level { get { return level; } }

}