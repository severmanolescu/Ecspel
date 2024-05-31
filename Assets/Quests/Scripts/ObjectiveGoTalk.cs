using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Quest", menuName = "Quest/Objective Go Talk", order = 2)]
public class ObjectiveGoTalk : Objective
{
    [SerializeField] private int npcID;

    [SerializeField] private List<ItemWithAmount> receiveItems;

    public int NpcID { get => npcID; }
    public List<ItemWithAmount> ReceiveItems { get => receiveItems; }
}
