using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestStorageInitial : MonoBehaviour
{
    [SerializeField] private List<ItemWithAmount> items = new List<ItemWithAmount>();

    private void Start()
    {
        ChestStorage chestStorage = GetComponent<ChestStorage>();

        foreach(ItemWithAmount item in items)
        {
            Item itemCopy = item.Item.Copy();
            itemCopy.Amount = item.Amount;

            chestStorage.AddItem(itemCopy);
        }

        Destroy(this);
    }
}
