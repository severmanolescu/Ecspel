using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest", order = 1)]
[Serializable]
public class Quest : ScriptableObject
{ 
    public string title;

    [TextArea(1, 10)]
    public string details;

    [Header("Item receive:")]
    public List<QuestItems> itemsReceive;

    [Header("Next dialogue:")]
    public DialogueScriptableObject nextDialogue;

    [Header("Next quest:")]
    public Quest nextQuest;

    public string Title { get { return title; } }
    public string Details { get { return details; } }
   
    public List<QuestItems> ItemsReceive { get { return itemsReceive; } }
    public DialogueScriptableObject NextDialogue { get { return nextDialogue; } }
    public Quest NextQuest { get { return nextQuest; } }
}
