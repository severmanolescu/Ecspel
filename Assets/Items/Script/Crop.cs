using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Crop", order = 1)]
[Serializable]
public class Crop : Placeable
{
    [SerializeField] private int dayToGrow;

    [SerializeField] private List<Sprite> levels;
    [SerializeField] private List<GameObject> level;

    [SerializeField] private Sprite destroy;

    [SerializeField] private Item crop;

    [SerializeField] private int minDrop;
    [SerializeField] private int maxDrop;

    [SerializeField] private bool centerX = true;
    [SerializeField] private bool centerY = true;

    [SerializeField] private bool refil;

    [SerializeField] private int refilDecreseSpriteIndexStart;

    [SerializeField] private bool tallCrop;

    public Crop(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, int sizeX, int sizeY, int startX, int startY, bool positionInCenter, int dayToGrow, Item crop, int minDrop, int maxDrop, bool centerX, bool centerY, bool refil, int refilDecreseSpriteIndexStart, bool tallCrop = false)
        : base(name, details, amount, maxAmount, itemSprite, sellPrice, sizeX, sizeY, null, startX, startY, positionInCenter)
    {
        this.dayToGrow = dayToGrow;
        this.crop = crop;
        this.minDrop = minDrop;
        this.maxDrop = maxDrop;
        this.centerX = centerX;
        this.centerY = centerY;
        this.refil = refil;
        this.refilDecreseSpriteIndexStart = refilDecreseSpriteIndexStart;
        this.tallCrop = tallCrop;
    }

    public int DayToGrow { get { return dayToGrow; } }
    public List<Sprite> Levels { get => levels; set => levels = value; }
    public Sprite Destroy1 { get => destroy; set => destroy = value; }
    public Item CropItem { get => crop; set => crop = value; }
    public int MinDrop { get => minDrop; set => minDrop = value; }
    public int MaxDrop { get => maxDrop; set => maxDrop = value; }
    public bool CenterX { get => centerX; set => centerX = value; }
    public bool CenterY { get => centerY; set => centerY = value; }
    public bool Refil { get => refil; set => refil = value; }
    public int RefilDecreseSpriteIndexStart { get => refilDecreseSpriteIndexStart; set => refilDecreseSpriteIndexStart = value; }
    public bool TallCrop { get => tallCrop; set => tallCrop = value; }
}