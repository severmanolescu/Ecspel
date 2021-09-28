using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChanger : MonoBehaviour
{
    private DialogueScriptableObject dialogueScriptable = null;

    private DialogueHandler dialogueHandler;
    private AnswersHandler answersHandler;

    public DialogueDisplay NPCDialogue;

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

    private void SetDialogue(DialogueClass dialogue)
    {
        if(dialogue.GetWhoReply() == false)
        { 
            dialogueHandler.gameObject.SetActive(true);

            dialogueHandler.SetDialogue(dialogue.GetDialogue());

            NPCDialogue.DeleteDialogue();
        }
        else
        {
            NPCDialogue.SetDialogue(dialogue.GetDialogue());

            dialogueHandler.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!stop && dialogueScriptable != null && dialogueIndex <= dialogueRespons.Count)
        {
            if(firstDialogue == true)
            {
                dialogueHandler.SetDialogue(dialogueRespons[0].GetDialogue());
                SetDialogue(dialogueRespons[0]);

                dialogueIndex = 1;

                firstDialogue = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(dialogueRespons[dialogueIndex - 1].GetNextDialogue() != null)
                {
                    SetDialogue(dialogueRespons[dialogueIndex - 1]);

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

                        NPCDialogue.DeleteDialogue();

                        stop = true;
                    }

                    return;
                }

                if(dialogueRespons[dialogueIndex] != null)
                {
                    SetDialogue(dialogueRespons[dialogueIndex]);

                    dialogueIndex++;
                }
            }
        }

        else if(NPCDialogue != null)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SetDialogue(NPCDialogue.GetDialogue());
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

            dialogueIndex = 0;

            firstDialogue = true;
            stop = false;
        }
    }

    private void DeleteDialogue()
    {
        firstDialogue = true;

        stop = true;

        dialogueHandler.StopDialogue();
        answersHandler.DeleteAll();

        dialogueHandler.gameObject.SetActive(false);
        answersHandler.gameObject.SetActive(false);
    }

    public void SetNCP(DialogueDisplay NPCDialogue)
    {
        this.NPCDialogue = NPCDialogue;
    }

    public void DeleteNPC()
    {
        NPCDialogue = null;

        DeleteDialogue();
    }
}
