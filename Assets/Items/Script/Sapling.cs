using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Sapling", order = 1)]
[System.Serializable]
public class Sapling : Placeable
{
    [SerializeField] private int dayToGrow;

    [SerializeField] private Sprite seed;
    [SerializeField] private Sprite sapling;
    [SerializeField] private List<Sprite> levels;
    [SerializeField] private GameObject almostMature;
    [SerializeField] private GameObject mature;

    public Sapling(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, int sizeX, int sizeY, int dayToGrow, int startX, int startY)
        : base(name, details, amount, maxAmount, itemSprite, sellPrice, sizeX, sizeY, null, startX, startY)
    {
        this.dayToGrow = dayToGrow;
    }

    public int DayToGrow { get { return dayToGrow; } }

    public Sprite Seed { get => seed; set => seed = value; }
    public Sprite SaplingSprite { get => sapling; set => sapling = value; }
    public List<Sprite> Levels { get => levels; set => levels = value; }
    public GameObject AlmostMature { get => almostMature; set => almostMature = value; }
    public GameObject Mature { get => mature; set => mature = value; }
}