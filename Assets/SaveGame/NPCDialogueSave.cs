using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueSave : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogueClasses = new List<Dialogue>();

    [SerializeField] private List<DialogueDisplay> npcDialogue;

    public List<DialogueDisplay> NpcDialogue { get => npcDialogue; }

    public int GetDialogueId(Dialogue dialogue)
    {
        return dialogueClasses.IndexOf(dialogue);
    }

    public Dialogue GetDialogueFromId(int id)
    {
        if (id >= 0 && id < dialogueClasses.Count)
        {
            return dialogueClasses[id];
        }

        return null;
    }

    public List<int> GetNpcsDialogue()
    {
        List<int> dialogues = new List<int>();

        foreach (DialogueDisplay dialogue in NpcDialogue)
        {
            // dialogues.Add(GetDialogueId(dialogue.Dialogue));
        }

        return dialogues;
    }

    public void SetNpcsDialogue(List<int> dialogue)
    {
        for (int indexOfDialogue = 0; indexOfDialogue < dialogue.Count; indexOfDialogue++)
        {
            // NpcDialogue[indexOfDialogue].Dialogue = GetDialogueFromId(dialogue[indexOfDialogue]);
        }
    }
}
