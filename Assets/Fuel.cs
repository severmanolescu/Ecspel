using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Fuel", order = 1)]
[System.Serializable]
public class Fuel : Item
{
    [SerializeField] private int duration;

    public Fuel(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, int duration)
    : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.duration = duration;
    }

    public int Duration { get => duration; }
}
