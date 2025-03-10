using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Placeable", order = 1)]
[System.Serializable]
public class Placeable : Item
{
    [SerializeField] private int sizeX;
    [SerializeField] private int sizeY;

    [SerializeField] private int startX = 0;
    [SerializeField] private int startY = 0;

    [SerializeField] private bool positionInCenter = false;

    [SerializeField] private GameObject prefab;

    public Placeable(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, int sizeX, int sizeY, GameObject prefab, int startX, int startY, bool positionInCenter)
        : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.prefab = prefab;
        this.startX = startX;
        this.startY = startY;
        this.positionInCenter = positionInCenter;
    }

    public Placeable(int sizeX, int sizeY, int startX, int startY, bool positionInCenter = false)
        : base("", "", 0, 0, 0, 0)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.startX = startX;
        this.startY = startY;
        this.positionInCenter = positionInCenter;
    }

    public int SizeX { get { return sizeX; } }
    public int SizeY { get { return sizeY; } }

    public GameObject Prefab { get => prefab; }
    public int StartX { get => startX; }
    public int StartY { get => startY; }
    public bool PositionInCenter { get => positionInCenter; set => positionInCenter = value; }
}