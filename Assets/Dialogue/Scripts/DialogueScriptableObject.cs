using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New Dialogue", order = 1)]
[Serializable]
public class DialogueScriptableObject : ScriptableObject
{
    [Header("Player and NPC respons:")]
    [SerializeField] private List<DialogueClass> dialogueRespons;

    [Header("Player answers for dialogue:")]
    [SerializeField] private List<DialogueAnswersClass> dialogueAnswers;

    [Header("Next dialogue to play:")]
    [SerializeField] private DialogueScriptableObject nextDialogue;

    [Header("Dialogue quests:")]
    [SerializeField] private List<Quest> quests;

    public List<DialogueClass> DialogueRespons { get { return dialogueRespons; } }
    public List<DialogueAnswersClass> DialogueAnswers { get { return dialogueAnswers; } }
    public DialogueScriptableObject NextDialogue { get { return nextDialogue; } set { nextDialogue = value; } }
    public List<Quest> Quest { get { return quests; } }

    public DialogueScriptableObject Copy()
    {
        return (DialogueScriptableObject)this.MemberwiseClone();
    }
}
