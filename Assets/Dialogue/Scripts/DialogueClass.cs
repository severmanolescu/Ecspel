using System;
using UnityEngine;

[Serializable]
public class DialogueClass
{
    [Header("false - Player\ntrue - NPC")]
    [SerializeField] private bool whoReply;

    [SerializeField] private string dialogue;

    [SerializeField] private DialogueScriptableObject nextDialogue;

    public bool GetWhoReply()
    {
        return whoReply;
    }

    public string GetDialogue()
    {
        return dialogue;
    }

    public DialogueScriptableObject GetNextDialogue()
    {
        return nextDialogue;
    }
}
