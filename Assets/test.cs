using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public List<Item> items;

    public void Start()
    {
        foreach(Item item in items)
        {
            Item copy = item.Copy();

            copy.Amount = 2;

            GetComponent<PlayerInventory>().AddItem(copy);
        }
    }
}
