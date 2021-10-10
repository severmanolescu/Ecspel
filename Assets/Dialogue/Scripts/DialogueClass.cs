using System;
using UnityEngine;

[Serializable]
public class DialogueClass
{
    [Header("false - Player\ntrue - NPC")]
    [SerializeField] private bool whoReply;

    [SerializeField] private string dialogue;

    [SerializeField] private DialogueScriptableObject nextDialogue;

    public bool WhoReply { get { return whoReply; } }
    public string Dialogue { get { return dialogue; } }
    public DialogueScriptableObject NextDialogue { get { return nextDialogue; } }
}
