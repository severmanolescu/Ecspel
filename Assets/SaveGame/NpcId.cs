using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcId : MonoBehaviour
{
    private List<DialogueDisplay> npcDialogue;

    private void Awake()
    {
        npcDialogue = GetComponent<NPCDialogueSave>().NpcDialogue;
    }

    public int GetNpcId(DialogueDisplay npcdialogue)
    {
        return npcDialogue.IndexOf(npcdialogue);
    }

    public DialogueDisplay GetNpcFromId(int id)
    {
        if (id >= 0 && id <= npcDialogue.Count - 1)
        {
            return npcDialogue[id];
        }
        return null;
    }
}
