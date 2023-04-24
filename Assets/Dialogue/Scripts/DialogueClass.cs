using System;
using UnityEngine;

[Serializable]
public class DialogueClass
{
    [TextArea(6, 6)]
    [SerializeField] private string dialogue;

    public string Dialogue { get { return dialogue; } }
}
