using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Sapling", order = 1)]
public class Sapling : Placeable
{
    public int dayToGrow;

    public Sapling(string name, string details, int amount, int maxAmount, Sprite itemSprite, int sizeX, int sizeY, int dayToGrow)
        : base(name, details, amount, maxAmount, itemSprite, sizeX, sizeY)
    {
        this.dayToGrow = dayToGrow;
    }

    public int DayToGrow { get { return dayToGrow; } }
}