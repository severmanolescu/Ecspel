using System.Collections.Generic;
using UnityEngine;

/* Versions:
 * 1.0: Return item based on item number, if the item is on the received position than return it otherwise search for the item in the list
 */

public class GetItemFromNO : MonoBehaviour
{
    [Header("all items, the order does not matter.")]
    [SerializeField] private List<Item> items = new();

    public Item ItemFromNo(int itemNo)
    {
        if (itemNo >= 0)
        {
            if (itemNo < items.Count)
            {
                if (items[itemNo].ItemNO == itemNo)
                {
                    return items[itemNo];
                }
            }

            foreach (Item item in items)
            {
                if (item.ItemNO == itemNo)
                {
                    return item;
                }
            }
        }

        return null;
    }
}
