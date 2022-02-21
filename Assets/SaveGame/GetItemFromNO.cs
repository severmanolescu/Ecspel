using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemFromNO : MonoBehaviour
{
    [SerializeField] private List<Item> items = new List<Item>();

    public Item ItemFromNo(int itemNo)
    {
        if(itemNo > 0 && itemNo <= items.Count)
        {
            foreach(Item item in items)
            {
                if(item.ItemNO == itemNo)
                {
                    return item;
                }
            }
        }

        return null;
    }
}
