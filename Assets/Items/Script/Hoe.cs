using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Hoe", order = 1)]
public class Hoe : Item
{
    [SerializeField] private float stamina;

    public Hoe(string name, string details, int amount, int maxAmount, Sprite itemSprite, float stamina)
    : base(name, details, amount, maxAmount, itemSprite)
    {
        this.stamina = stamina;
    }

    public float Stamina { get => stamina; set => stamina = value; }
}