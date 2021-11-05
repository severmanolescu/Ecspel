using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Hoe", order = 1)]
public class Hoe : Item
{
    public Hoe(string name, string details, int amount, int maxAmount, Sprite itemSprite)
    : base(name, details, amount, maxAmount, itemSprite)
    {
    }
}