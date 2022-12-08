using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Weapon", order = 1)]
[System.Serializable]
public class Weapon : Item
{
    [SerializeField] private float attackPower;

    public Weapon(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, float attackPower)
        : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.attackPower = attackPower;
    }

    public float AttackPower { get { return attackPower; } }
}
