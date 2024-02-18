using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New Dialogue", order = 1)]
[Serializable]
public class DialogueScriptableObject : ScriptableObject
{
    [Header("Player and NPC respons:")]
    [SerializeField] private List<Dialogue> dialogueRespons;

    [Header("Next dialogue to play:")]
    [SerializeField] private DialogueScriptableObject nextDialogue;

    [Header("Dialogue quests:")]
    [SerializeField] private List<Quest> quests;

    [Header("If this is dialogue is first")]
    [SerializeField] private bool startDialogue;

    public List<Dialogue> DialogueRespons { get { return dialogueRespons; } }
    public DialogueScriptableObject NextDialogue { get { return nextDialogue; } }
    public List<Quest> Quest { get { return quests; } }

    public bool StartDialogue { get => startDialogue; set => startDialogue = value; }

    public DialogueScriptableObject Copy()
    {
        return (DialogueScriptableObject)this.MemberwiseClone();
    }
}
