using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class QuestItems
{
    [SerializeField] private Item item;
    [SerializeField] private Weapon weapon;
    [SerializeField] private Range range;
    [SerializeField] private Axe axe;
    [SerializeField] private Pickaxe pickaxe;

    [SerializeField] private int amount;

    [SerializeField] private int minDrop;
    [SerializeField] private int maxDrop;

    public Item Item { get { return ValidItem(); } }
    public int Amount { get { return amount; } }
    public int Drop { get { return (int)UnityEngine.Random.Range(minDrop, maxDrop); } }

    private Item ValidItem()
    {
        if(item != null)
        {
            return item;
        }    
        else if(weapon != null)
        {
            return item;
        }
        else if(range != null)
        {
            return range;
        }
        else if(axe != null)
        {
            return axe;
        }
        else if(pickaxe != null)
        {
            return pickaxe;
        }
        else
        {
            return null;
        }
    }
}
