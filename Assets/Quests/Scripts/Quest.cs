using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/Quest", order = 1)]
[Serializable]
public class Quest : ScriptableObject
{
    [SerializeField] private string title;

    [TextArea(4, 40)]
    [SerializeField] private string details;

    [SerializeField] private Objective questObjective;

    [SerializeField] private List<ItemWithAmount> receiveItems = new();

    [Header("Next dialogue:")]
    [SerializeField] private Dialogue nextDialogue;

    [Header("Next quest:")]
    [SerializeField] private Quest nextQuest;

    public string Title { get { return title; } }
    public string Details { get { return details; } }

    public Dialogue NextDialogue { get { return nextDialogue; } }
    public Quest NextQuest { get { return nextQuest; } }

    public Objective QuestObjective { get => questObjective; set => questObjective = value; }
    public List<ItemWithAmount> ReceiveItems { get => receiveItems; set => receiveItems = value; }

    public Quest Copy()
    {
        return (Quest)this.MemberwiseClone();
    }
}