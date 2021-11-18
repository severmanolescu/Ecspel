using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Placeable", order = 1)]
public class Placeable : Item
{
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeY;

    [SerializeField] private GameObject prefab;

    public Placeable(string name, string details, int amount, int maxAmount, Sprite itemSprite, int sizeX, int sizeY, GameObject prefab)
        : base(name, details, amount, maxAmount, itemSprite)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.Prefab = prefab;
    }

    public int SizeX { get { return sizeX; } }
    public int SizeY { get { return sizeY; } }

    public GameObject Prefab { get => prefab; set => prefab = value; }
}