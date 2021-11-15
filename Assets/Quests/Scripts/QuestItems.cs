using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class QuestItems
{
    [SerializeField] private Item item;

    [SerializeField] private int amount;

    [SerializeField] private int minDrop;
    [SerializeField] private int maxDrop;

    public Item Item { get { return item; } }
    public int Amount { get { return amount; } }
    public int Drop { get { return (int)UnityEngine.Random.Range(minDrop, maxDrop); } }
}
