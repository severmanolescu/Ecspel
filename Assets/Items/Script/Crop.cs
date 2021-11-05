using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Crop", order = 1)]
public class Crop : Placeable
{
    public int dayToGrow;

    public List<Sprite> levels;

    public Sprite destroy;

    public Item crop;

    public int minDrop;
    public int maxDrop;

    public bool centerX = true;
    public bool centerY = true;

    public bool refil;
    public int refilDecreseSpriteIndexStart;

    public Crop(string name, string details, int amount, int maxAmount, Sprite itemSprite, int sizeX, int sizeY, int dayToGrow, Item crop, int minDrop, int maxDrop, bool centerX, bool centerY, bool refil, int refilDecreseSpriteIndexStart)
        : base(name, details, amount, maxAmount, itemSprite, sizeX, sizeY)
    {
        this.dayToGrow = dayToGrow;
        this.crop = crop;
        this.minDrop = minDrop;
        this.maxDrop = maxDrop;
        this.centerX = centerX;
        this.centerY = centerY;
        this.refil = refil;
        this.refilDecreseSpriteIndexStart = refilDecreseSpriteIndexStart;
    }

    public int DayToGrow { get { return dayToGrow; } }
}