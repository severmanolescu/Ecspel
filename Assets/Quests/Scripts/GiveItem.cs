using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest Give Item", order = 1)]
public class GiveItem : Quest
{
    [Header("Requirement quest:")]
    public GameObject whoToGive;
    public List<QuestItems> itemsNeeds;

    public GameObject WhoToGive { get { return whoToGive; } set { whoToGive = value; } }
    public List<QuestItems> ItemsNeeds { get { return itemsNeeds; } }
}