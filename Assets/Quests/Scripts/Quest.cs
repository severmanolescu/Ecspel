using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "Quest/New Quest", order = 1)]
[Serializable]
public class Quest : ScriptableObject
{
    [SerializeField] private string title;

    [TextArea(4, 40)]
    [SerializeField] private string details;

    [SerializeField] private List<Objective> questObjectives = new List<Objective>();

    [SerializeField] private List<ItemWithAmount> receiveItems = new();

    [Header("Next dialogue:")]
    [SerializeField] private DialogueScriptableObject nextDialogue;

    [Header("Next quest:")]
    [SerializeField] private Quest nextQuest;

    public Quest(string title, string details, List<Objective> questObjectives, List<ItemWithAmount> receiveItems, DialogueScriptableObject nextDialogue, Quest nextQuest)
    {
        this.title = title;
        this.details = details;
        this.questObjectives = questObjectives;
        this.receiveItems = receiveItems;
        this.nextDialogue = nextDialogue;
        this.nextQuest = nextQuest;
    }

    public string Title { get { return title; } }
    public string Details { get { return details; } }

    public DialogueScriptableObject NextDialogue { get { return nextDialogue; } }
    public Quest NextQuest { get { return nextQuest; } }

    public List<Objective> QuestObjectives { get => questObjectives; set => questObjectives = value; }
    public List<ItemWithAmount> ReceiveItems { get => receiveItems; set => receiveItems = value; }

    public Quest Copy()
    {
        Quest questCopy = (Quest)this.MemberwiseClone();

        List<Objective> objectiveCopy = new List<Objective>();

        foreach (Objective objective in QuestObjectives)
        {
            objectiveCopy.Add(objective.Copy());
        }

        questCopy.QuestObjectives = objectiveCopy;

        return questCopy;
    }
}