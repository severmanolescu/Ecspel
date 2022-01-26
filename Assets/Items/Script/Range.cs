using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Range Weapon", order = 1)]
[System.Serializable]
public class Range : Weapon
{
    [SerializeField] private float range;

    public Range(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, float attackPower, float range)
        : base(name, details, amount, maxAmount, itemSprite, sellPrice, attackPower)
    {
        this.range = range;
    }

    public float GetRange { get { return range; } }
}