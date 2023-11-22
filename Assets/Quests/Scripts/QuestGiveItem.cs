using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Quest Give Items", order = 1)]
[Serializable]
public class QuestGiveItem : Objective
{
    [SerializeField] private int npcID;

    [SerializeField] private List<ItemWithAmount> itemsToGive = new List<ItemWithAmount>();

    public int NpcID { get => npcID; }
    public List<ItemWithAmount> ItemsToGive { get => itemsToGive; }
}
