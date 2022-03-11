using System;

[Serializable]
public class ItemWithRandomAmount
{
    public Item item;
    public int minAmount;
    public int maxAmount;

    public Item Item { get => item; set => item = value; }
    public int MinAmount { get => minAmount; set => minAmount = value; }
    public int MaxAmount { get => maxAmount; set => maxAmount = value; }

    public Item GetItem()
    {
        Item newItem = item.Copy();

        newItem.Amount = UnityEngine.Random.Range(minAmount, maxAmount);

        return newItem;
    }
}
