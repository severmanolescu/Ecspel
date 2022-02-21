using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New Dialogue", order = 1)]
[Serializable]
public class DialogueScriptableObject : ScriptableObject
{
    [Header("NPC name: ")]
    [SerializeField] private new string name;

    [Header("If NPC have a quest: ")]
    [SerializeField] private bool haveQuest;

    [Header("Player and NPC respons:")]
    [SerializeField] private List<DialogueClass> dialogueRespons;

    [Header("Player answers for dialogue:")]
    [SerializeField] private List<DialogueAnswersClass> dialogueAnswers;

    [Header("Next dialogue to play:")]
    [SerializeField] private DialogueScriptableObject nextDialogue;

    [Header("Dialogue quests:")]
    [SerializeField] private List<Quest> quests;

    [Header("Dialogue id:")]
    [SerializeField] private int dialogueID;

    public string Name { get { return name; } }
    public bool HaveQuest { get { return haveQuest; } }
    public List<DialogueClass> DialogueRespons { get { return dialogueRespons; } }
    public List<DialogueAnswersClass> DialogueAnswers { get { return dialogueAnswers; } }
    public DialogueScriptableObject NextDialogue { get { return nextDialogue; } }
    public List<Quest> Quest { get { return quests; } }

    public int DialogueID { get => dialogueID; set => dialogueID = value; }
}
