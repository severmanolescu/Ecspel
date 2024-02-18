using System.Collections.Generic;
using UnityEngine;

public class RandomDialogue : MonoBehaviour
{
    [SerializeField] private List<DialogueScriptableObject> dialogues = new List<DialogueScriptableObject>();

    [SerializeField] private List<DialogueScriptableObject> cantTalk = new List<DialogueScriptableObject>();

    public DialogueScriptableObject GetDialogue()
    {
        return dialogues[Random.Range(0, dialogues.Count - 1)];
    }

    public DialogueScriptableObject GetCantTalk()
    {
        return cantTalk[Random.Range(0, cantTalk.Count - 1)];
    }
}
