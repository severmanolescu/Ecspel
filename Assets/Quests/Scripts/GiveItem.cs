using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest Give Item", order = 1)]
public class GiveItem : Quest
{
    [Header("Requirement quest:")]
    public int whoToGiveId = -1;
    public List<QuestItems> itemsNeeds;

    public int WhoToGive { get { return whoToGiveId; } set { whoToGiveId = value; } }
    public List<QuestItems> ItemsNeeds { get { return itemsNeeds; } }
}