using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Pickaxe", order = 1)]
[System.Serializable]
public class Pickaxe : ItemUse
{
    [SerializeField] private float damage;
    [SerializeField] private int level;

    [SerializeField] private float stamina;

    public Pickaxe(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, Sprite itemUseLateral, Sprite itemUseBack, List<Sprite> itemUseFront, float damage, int level, float stamina)
    : base(name, details, amount, maxAmount, itemSprite, sellPrice, itemUseLateral, itemUseBack, itemUseFront)
    {
        this.damage = damage;
        this.level = level;
        this.stamina = stamina;
    }

    public float Damage { get { return damage; } }
    public int Level { get { return level; } }

    public float Stamina { get => stamina; set => stamina = value; }
}