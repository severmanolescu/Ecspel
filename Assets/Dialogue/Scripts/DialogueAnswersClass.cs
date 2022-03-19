using System;
using UnityEngine;

[Serializable]
public class DialogueAnswersClass
{
    [TextArea(3, 3)]
    [SerializeField] private string answer;
    [SerializeField] private DialogueScriptableObject nextDialogue;

    public string Answer { get { return answer; } }
    public DialogueScriptableObject NextDialogue { get { return nextDialogue; } }
}
