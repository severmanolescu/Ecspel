using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Sickle", order = 1)]
[System.Serializable]
public class Sickle : Item
{
    [SerializeField] private float stamina;
    [SerializeField] private float attack;

    public Sickle(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, float stamina, int attack)
    : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.stamina = stamina;
        this.Attack = attack;
    }

    public float Stamina { get => stamina; set => stamina = value; }
    public float Attack { get => attack; set => attack = value; }
}
