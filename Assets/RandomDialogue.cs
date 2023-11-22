using System.Collections.Generic;
using UnityEngine;

public class RandomDialogue : MonoBehaviour
{
    [SerializeField] private List<DialogueScriptableObject> dialogues = new List<DialogueScriptableObject>();

    public DialogueScriptableObject GetDialogue()
    {
        return dialogues[Random.Range(0, dialogues.Count - 1)];
    }
}
