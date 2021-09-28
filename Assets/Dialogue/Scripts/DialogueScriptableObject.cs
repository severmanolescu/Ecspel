using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/New Dialogue", order = 1)]
public class DialogueScriptableObject : ScriptableObject
{
    [SerializeField] private new string name;
    [SerializeField] private List<DialogueClass> dialogueRespons;
    [SerializeField] private List<DialogueanswersClass> dialogueAnswers;

    public string GetName()
    {
        return name;
    }

    public List<DialogueClass> GetDialogueRespons()
    {
        return dialogueRespons;
    }

    public List<DialogueanswersClass> GetDialogueAnswers()
    {
        return dialogueAnswers;
    }

}
