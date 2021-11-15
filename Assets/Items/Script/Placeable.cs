using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Placeable", order = 1)]
public class Placeable : Item
{
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeY;

    public Placeable(string name, string details, int amount, int maxAmount, Sprite itemSprite, int sizeX, int sizeY)
        : base(name, details, amount, maxAmount, itemSprite)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
    }

    public int SizeX { get { return sizeX; } }
    public int SizeY { get { return sizeY; } }
}