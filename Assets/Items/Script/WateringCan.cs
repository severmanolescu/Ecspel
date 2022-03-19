using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Watering can", order = 1)]
[System.Serializable]
public class WateringCan : Item
{
    [SerializeField] private float stamina;
    [SerializeField] private int noOfUses;
    [SerializeField] private int remainWater;

    public WateringCan(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, float stamina, int noOfUses)
    : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.stamina = stamina;
        this.NoOfUses = noOfUses;

        remainWater = noOfUses;
    }

    public float Stamina { get => stamina; set => stamina = value; }
    public int NoOfUses { get => noOfUses; set => noOfUses = value; }
    public int RemainWater { get => remainWater; set => remainWater = value; }
}