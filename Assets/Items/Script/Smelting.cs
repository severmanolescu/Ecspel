using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Smelting", order = 1)]
[System.Serializable]
public class Smelting : Item
{
    [SerializeField] private int duration;

    [SerializeField] private Item nextItem;

    public Smelting(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, int duration, Item nextItem)
    : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.duration = duration;

        this.nextItem = nextItem;
    }

    public int Duration { get => duration;}
    public Item NextItem { get => nextItem; }
}
