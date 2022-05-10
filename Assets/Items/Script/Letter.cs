using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/New Letter", order = 1)]
[System.Serializable]
public class Letter : Item
{
    [TextArea(5, 5)]
    [SerializeField] private string title;
    [TextArea(5, 30)]
    [SerializeField] private string textDetails;

    public Letter(string name, string details, int amount, int maxAmount, int itemSprite, int sellPrice, string title, string textDetails)
    : base(name, details, amount, maxAmount, itemSprite, sellPrice)
    {
        this.title = title;
        this.textDetails = textDetails;
    }

    public string Title { get => title; set => title = value; }
    public string TextDetails { get => textDetails; set => textDetails = value; }
}
