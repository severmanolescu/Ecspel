using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueSave : MonoBehaviour
{
    [SerializeField] private List<DialogueScriptableObject> dialogueClasses = new List<DialogueScriptableObject>();

    [SerializeField] private List<DialogueDisplay> npcDialogue;

    public List<DialogueDisplay> NpcDialogue { get => npcDialogue; }

    private int GetDialogueId(DialogueScriptableObject dialogue)
    {
        return dialogue.DialogueID;
    }

    public DialogueScriptableObject GetDialogueFromId(int id)
    {
        if(id != -1 && id < dialogueClasses.Count)
        {
            foreach(DialogueScriptableObject dialogue in dialogueClasses)
            {
                if(dialogue.DialogueID == id)
                {
                    return dialogue;
                }
            }
        }

        return null;
    }

    public List<int> GetNpcsDialogue()
    {
        List<int> dialogues = new List<int>();

        foreach(DialogueDisplay dialogue in NpcDialogue)
        {
            dialogues.Add(GetDialogueId(dialogue.Dialogue));
        }

        return dialogues;
    }

    public void SetNpcsDialogue(List<int> dialogue)
    {
        for(int indexOfDialogue = 0; indexOfDialogue < dialogue.Count; indexOfDialogue++)
        {
            NpcDialogue[indexOfDialogue].Dialogue = GetDialogueFromId(dialogue[indexOfDialogue]);
        }
    }
}
