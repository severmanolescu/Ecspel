using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SetDialogueToPlayer : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject initialDialogue;

    private DialogueChanger dialogueChanger;

    private PlayerMovement playerMovement;

    private DialoguePlayerEnterInTrigger dialoguePlayerEnter;

    private void Awake()
    {
        dialogueChanger = GameObject.Find("Player/Canvas/Dialogue").GetComponent<DialogueChanger>();

        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
    }

    public void SetDialogue(DialogueScriptableObject dialogue, DialoguePlayerEnterInTrigger dialoguePlayerEnter = null)
    {
        playerMovement.Dialogue = true;

        dialogueChanger.SetDialogue(dialogue, this);

        this.dialoguePlayerEnter = dialoguePlayerEnter;
    }

    public void DialogueEnd()
    {
        playerMovement.Dialogue = false;

        if(dialoguePlayerEnter != null)
        {
            dialoguePlayerEnter.DialogueEnd();
        }
    }
}
