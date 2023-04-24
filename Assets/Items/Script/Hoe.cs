using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Hoe", order = 1)]
[Serializable]
public class Hoe : ItemUse
{
    [SerializeField] private float stamina;

    public Hoe(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, Sprite itemUseLateral, Sprite itemUseBack, List<Sprite> itemUseFront, float stamina)
    : base(name, details, amount, maxAmount, itemSprite, sellPrice, itemUseLateral, itemUseBack, itemUseFront)
    {
        this.stamina = stamina;
    }

    public float Stamina { get => stamina; set => stamina = value; }
}