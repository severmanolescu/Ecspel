using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest", order = 1)]
public class Quest : ScriptableObject
{ 
    [SerializeField] private string title;

    [TextArea(1, 10)]
    [SerializeField] private string details;

    [Header("Item requirement quest:")]
    [SerializeField] private GameObject whoToGive;
    [SerializeField] private List<QuestItems> itemsNeeds;

    [Header("Item receive:")]
    [SerializeField] private List<QuestItems> itemsReceive;

    [Header("Next Dialogue:")]
    [SerializeField] DialogueScriptableObject nextDialogue;

    public string Title { get { return title; } }
    public string Details { get { return details; } }
    public GameObject WhoToGive { get { return whoToGive; } set { whoToGive = value; } }
    public List<QuestItems> ItemsNeeds { get { return itemsNeeds; } }
    public List<QuestItems> ItemsReceive { get { return itemsReceive; } }
    public DialogueScriptableObject NextDialogue { get { return nextDialogue; } }
}
