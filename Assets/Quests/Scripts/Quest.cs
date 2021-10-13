using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest", order = 1)]
public class Quest : ScriptableObject
{ 
    [SerializeField] private string title;

    [TextArea(1, 10)]
    [SerializeField] private string details;

    [Header("Requirement quest:")]
    [SerializeField] private GameObject whoToGive;
    [SerializeField] private List<QuestItems> itemsNeeds;

    [Header("Go to location quest:")]
    [SerializeField] private List<Vector3> positions;

    [Header("Item receive:")]
    [SerializeField] private List<QuestItems> itemsReceive;

    [Header("Next dialogue:")]
    [SerializeField] DialogueScriptableObject nextDialogue;

    [Header("Next quest:")]
    [SerializeField] Quest nextQuest;

    public string Title { get { return title; } }
    public string Details { get { return details; } }
    public GameObject WhoToGive { get { return whoToGive; } set { whoToGive = value; } }
    public List<QuestItems> ItemsNeeds { get { return itemsNeeds; } }
    public List<Vector3> Positions { get { return positions; } }
    public List<QuestItems> ItemsReceive { get { return itemsReceive; } }
    public DialogueScriptableObject NextDialogue { get { return nextDialogue; } }
    public Quest NextQuest { get { return nextQuest; } }
}
