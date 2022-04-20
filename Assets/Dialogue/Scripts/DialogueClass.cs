using System;
using UnityEngine;

[Serializable]
public class DialogueClass
{
    [Header("false - Player\ntrue - NPC")]
    [SerializeField] private bool whoReply;

    [TextArea(6, 6)]
    [SerializeField] private string dialogue;

    public bool WhoReply { get { return whoReply; } }
    public string Dialogue { get { return dialogue; } }
}
