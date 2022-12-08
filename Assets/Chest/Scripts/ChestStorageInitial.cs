using System.Collections.Generic;
using UnityEngine;

public class ChestStorageInitial : MonoBehaviour
{
    [SerializeField] private List<ItemWithAmount> items = new List<ItemWithAmount>();

    public List<ItemWithAmount> Items { get => items; set => items = value; }

    private void Start()
    {
        ChestStorage chestStorage = GetComponent<ChestStorage>();

        foreach (ItemWithAmount item in Items)
        {
            Item itemCopy = item.Item.Copy();
            itemCopy.Amount = item.Amount;

            chestStorage.AddItem(itemCopy);
        }

        Destroy(this);
    }

    public void RemoveAll()
    {
        items.Clear();
    }
}
