using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Pickaxe", order = 1)]
public class Pickaxe : Item
{
    public Pickaxe(string name, string details, int amount, int maxAmount, Sprite itemSprite) : base(name, details, amount, maxAmount, itemSprite)
    {

    }
}