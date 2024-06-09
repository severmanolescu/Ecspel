using System.Collections.Generic;
using UnityEngine;

public class RandomDialogue : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogues = new List<Dialogue>();

    [SerializeField] private List<Dialogue> cantTalk = new List<Dialogue>();

    public Dialogue GetDialogue()
    {
        if(dialogues != null && dialogues.Count > 0)
        {
            return dialogues[Random.Range(0, dialogues.Count - 1)];
        }

        return null;
    }

    public Dialogue GetCantTalk()
    {
        if(cantTalk != null && cantTalk.Count > 0)
        {
            return cantTalk[Random.Range(0, cantTalk.Count - 1)];
        }

        return null;
    }
}
