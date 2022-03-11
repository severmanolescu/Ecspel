using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Item/New Recipe", order = 1)]
[System.Serializable]
public class CraftRecipe : Item
{
    [SerializeField] private Craft recipe;

    public CraftRecipe(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, Craft craftRecipe)
    : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.recipe = craftRecipe;
    }

    public Craft Recipe { get => recipe; }
}
