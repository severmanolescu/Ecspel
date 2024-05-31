using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New Dialogue", order = 1)]
[Serializable]
public class Dialogue : ScriptableObject
{
    [Header("Player and NPC respons:")]
    [SerializeField] private List<DialogueResponse> dialogueRespons;

    [Header("Next dialogue to play:")]
    [SerializeField] private Dialogue nextDialogue;

    [Header("Dialogue quests:")]
    [SerializeField] private List<Quest> quests;

    public List<DialogueResponse> DialogueRespons { get { return dialogueRespons; } }
    public Dialogue NextDialogue { get { return nextDialogue; } }
    public List<Quest> Quest { get { return quests; } }

    public Dialogue Copy()
    {
        return (Dialogue)this.MemberwiseClone();
    }
}
