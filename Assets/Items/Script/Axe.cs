using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Axe", order = 1)]
[Serializable]
public class Axe : Item
{
    [SerializeField] private float damage;
    [SerializeField] private int level;

    [SerializeField] private float stamina;

    public Axe(string name, string details, int amount, int maxAmount, int itemSprite, float damage, int level, float stamina)
    : base(name, details, amount, maxAmount, itemSprite)
    {
        this.damage = damage;
        this.level = level;
        this.stamina = stamina;
    }

    public float Damage { get { return damage; } }
    public int Level { get { return level; } }
    public float Stamina { get { return stamina; } }
}
