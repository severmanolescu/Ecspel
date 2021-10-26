using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Axe", order = 1)]
public class Axe : Item
{
    public Axe(string name, string details, int amount, int maxAmount, Sprite itemSprite) : base(name, details, amount, maxAmount, itemSprite)
    {

    }
}
