using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChanger : MonoBehaviour
{
    private DialogueScriptableObject dialogueScriptable = null;

    private DialogueHandler dialogueHandler;
    private AnswersHandler answersHandler;

    private List<DialogueClass> dialogueRespons = null;

    private bool firstDialogue = false;

    private bool stop = true;

    private int dialogueIndex = 0;

    private void Awake()
    {
         dialogueHandler = gameObject.GetComponentInChildren<DialogueHandler>();
         answersHandler  = gameObject.GetComponentInChildren<AnswersHandler>();
    }

    private void Start()
    {
        dialogueHandler.gameObject.SetActive(false);
        answersHandler.gameObject.SetActive(false);

        answersHandler.SetDialogueChanger(this);
    }

    private void Update()
    {
        if (!stop && dialogueScriptable != null && dialogueIndex <= dialogueRespons.Count)
        {
            if(firstDialogue == true)
            {
                dialogueHandler.SetDialogue(dialogueRespons[0].GetDialogue());

                dialogueIndex = 1;

                firstDialogue = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(dialogueRespons[dialogueIndex - 1].GetNextDialogue() != null)
                {
                    SetDialogue(dialogueRespons[dialogueIndex - 1].GetNextDialogue());

                    stop = true;

                    return;
                }

                if(dialogueIndex == dialogueRespons.Count)
                {
                    dialogueHandler.gameObject.SetActive(false);

                    if(dialogueScriptable.GetDialogueAnswers() != null)
                    {
                        answersHandler.SetAnswers(dialogueScriptable.GetDialogueAnswers());

                        answersHandler.gameObject.SetActive(true);
                        dialogueHandler.gameObject.SetActive(false);

                        stop = true;
                    }

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

            dialogueHandler.gameObject.SetActive(true);

            answersHandler.DeleteAll();
            answersHandler.gameObject.SetActive(false);

            firstDialogue = true;
            stop = false;
        }
    }
}
