using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Placeable", order = 1)]
[System.Serializable]
public class Placeable : Item
{
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeY;

    [SerializeField] private int startX = 0;
    [SerializeField] private int startY = 0;

    [SerializeField] private GameObject prefab;

    public Placeable(string name, string details, int amount, int maxAmount, int itemSprite, int sizeX, int sizeY, GameObject prefab, int startX, int startY)
        : base(name, details, amount, maxAmount, itemSprite)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.prefab = prefab;
        this.startX = startX;
        this.startY = startY;
    }

    public int SizeX { get { return sizeX; } }
    public int SizeY { get { return sizeY; } }

    public GameObject Prefab { get => prefab;}
    public int StartX { get => startX; }
    public int StartY { get => startY; }
}