using System.Collections.Generic;
using UnityEngine;

public class RandomDialogue : MonoBehaviour
{
    [SerializeField] private List<Dialogue> dialogues = new List<Dialogue>();

    [SerializeField] private List<Dialogue> cantTalk = new List<Dialogue>();

    public Dialogue GetDialogue()
    {
        return dialogues[Random.Range(0, dialogues.Count - 1)];
    }

    public Dialogue GetCantTalk()
    {
        return cantTalk[Random.Range(0, cantTalk.Count - 1)];
    }
}
