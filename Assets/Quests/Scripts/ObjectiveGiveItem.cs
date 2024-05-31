using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Objective Give Items", order = 1)]
[Serializable]
public class ObjectiveGiveItem : Objective
{
    [SerializeField] private int npcID;

    [SerializeField] private List<ItemWithAmount> itemsToGive = new List<ItemWithAmount>();

    public int NpcID { get => npcID; }
    public List<ItemWithAmount> ItemsToGive { get => itemsToGive; }
}
