using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    private DialogueChanger dialogueChanger;

    public DialogueScriptableObject dialogueScriptable;

    private void Awake()
    {
        dialogueChanger = gameObject.GetComponent<DialogueChanger>();
    }

    private void Update()
    {
        dialogueChanger.SetDialogue(dialogueScriptable);

        Destroy(this);
    }
}
