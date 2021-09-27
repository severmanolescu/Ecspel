using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChanger : MonoBehaviour
{
    private DialogueScriptableObject dialogueScriptable = null;

    private DialogueHandler dialogueHandler;

    private List<DialogueClass> dialogueRespons = null;
    private List<DialogueanswersClass> dialogueAnswers = null;

    private bool firstDialogue = false;

    private int dialogueIndex = 0;

    private void Awake()
    {
         dialogueHandler = gameObject.GetComponentInChildren<DialogueHandler>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (dialogueScriptable != null && dialogueIndex <= dialogueRespons.Count)
        {
            if(firstDialogue == true)
            {
                gameObject.SetActive(true);

                dialogueHandler.SetDialogue(dialogueRespons[0].GetDialogue());

                dialogueIndex = 1;

                firstDialogue = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(dialogueRespons[dialogueIndex - 1].GetNextDialogue() != null)
                {
                    SetDialogue(dialogueRespons[dialogueIndex - 1].GetNextDialogue());

                    return;
                }

                if(dialogueIndex == dialogueRespons.Count)
                {
                    gameObject.SetActive(false);

                    return;
                }

                if(dialogueRespons[dialogueIndex] != null)
                {
                    dialogueHandler.SetDialogue(dialogueRespons[dialogueIndex].GetDialogue());

                    dialogueIndex++;
                }
            }

        }
    }

    public void SetDialogue(DialogueScriptableObject dialogueScriptable)
    {
        if(dialogueScriptable != null)
        {
            this.dialogueScriptable = dialogueScriptable;

            dialogueRespons = dialogueScriptable.GetDialogueRespons();
            dialogueAnswers = dialogueScriptable.GetDialogueAnswers();

            firstDialogue = true;
        }

        
    }
}
