using System.Collections.Generic;
using UnityEngine;

public class ItemUse : Item
{
    [SerializeField] private Sprite itemUseLateral;
    [SerializeField] private Sprite itemUseBack;
    [SerializeField] private List<Sprite> itemUseFront;

    public ItemUse(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, Sprite itemUseLateral, Sprite itemUseBack, List<Sprite> itemUseFront)
        : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.itemUseLateral = itemUseLateral;
        this.itemUseBack = itemUseBack;
        this.itemUseFront = itemUseFront;
    }

    public Sprite ItemUseLateral { get => itemUseLateral; set => itemUseLateral = value; }
    public Sprite ItemUseBack { get => itemUseBack; set => itemUseBack = value; }
    public List<Sprite> ItemUseFront { get => itemUseFront; set => itemUseFront = value; }
}
