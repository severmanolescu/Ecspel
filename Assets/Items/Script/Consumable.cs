using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Consumable", order = 1)]
[Serializable]
public class Consumable : Item
{
    [SerializeField] private int health;
    [SerializeField] private int stamina;

    public Consumable(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, int health, int stamina)
    : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.health = health;
        this.stamina = stamina;
    }

    public int Health { get { return health; } }
    public float Stamina { get { return stamina; } }
}

