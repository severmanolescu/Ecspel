using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Range Weapon", order = 1)]
public class Range : Weapon
{
    public float range;

    public Range(string name, string details, int amount, int maxAmount, Sprite itemSprite, float attackPower, float range)
        : base(name, details, amount, maxAmount, itemSprite, attackPower)
    {
        this.range = range;
    }

    public float GetRange { get { return range; } }
}