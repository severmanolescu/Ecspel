using System;
using UnityEngine;

[Serializable]
public class DialogueanswersClass
{
    [SerializeField] private string answer;
    [SerializeField] private DialogueScriptableObject nextDialogue;

    public string GetAnswer()
    {
        return answer;
    }

    public DialogueScriptableObject GetNextDialogue()
    {
        return nextDialogue;
    }
}
