using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChanger : MonoBehaviour
{
    private DialogueScriptableObject dialogueScriptable = null;

    private DialogueHandler dialogueHandler;
    private AnswersHandler answersHandler;

    private DialogueDisplay NPCDialogue;

    private List<DialogueClass> dialogueRespons = null;

    private QuestTabHandler questTab;

    private bool firstDialogue = false;

    private bool stop = true;

    private int dialogueIndex = 0;

    private void Awake()
    {
        dialogueHandler = gameObject.GetComponentInChildren<DialogueHandler>();
        answersHandler  = gameObject.GetComponentInChildren<AnswersHandler>();
        questTab = GameObject.Find("Player/Canvas/QuestTab").GetComponent<QuestTabHandler>();
    }

    private void Start()
    {
        dialogueHandler.gameObject.SetActive(false);
        answersHandler.gameObject.SetActive(false);

        answersHandler.SetDialogueChanger(this);
    }

    private void SetDialogue(DialogueClass dialogue)
    {
        if(dialogue.WhoReply == false)
        { 
            dialogueHandler.gameObject.SetActive(true);

            dialogueHandler.SetDialogue(dialogue.Dialogue);

            NPCDialogue.DeleteDialogue();
        }
        else
        {
            NPCDialogue.SetDialogue(dialogue.Dialogue);

            dialogueHandler.gameObject.SetActive(false);
        }
    }

    private void SetQuest()
    {
        answersHandler.SetAnswers(NPCDialogue.Quest);

        answersHandler.gameObject.SetActive(true);
        dialogueHandler.gameObject.SetActive(false);

        NPCDialogue.DeleteDialogue();
    }

    private void DialogueEnd()
    {
        dialogueHandler.gameObject.SetActive(false);

        if (dialogueScriptable.DialogueAnswers.Count >= 1)
        {
            answersHandler.SetAnswers(dialogueScriptable.DialogueAnswers);

            answersHandler.gameObject.SetActive(true);
            dialogueHandler.gameObject.SetActive(false);

            NPCDialogue.DeleteDialogue();
        }

        if(NPCDialogue.Quest.Count > 0)
        {
            SetQuest();
        }
        else if (dialogueScriptable.NextDialogue != null)
        {
            NPCDialogue.Dialogue = dialogueScriptable.NextDialogue;
        }
        else if(dialogueScriptable.NextDialogue == null)
        {
            NPCDialogue.Dialogue = null;
        }

        if(dialogueScriptable.Quest.Count > 0)
        {
            questTab.AddQuest(dialogueScriptable.Quest);
        } 

        stop = true;
    }

    private void Update()
    {
        if (!stop && dialogueScriptable != null && dialogueIndex <= dialogueRespons.Count)
        {
            if (dialogueRespons.Count == 0)
            {
                DialogueEnd();
            }

            if (firstDialogue == true)
            {
                dialogueHandler.SetDialogue(dialogueRespons[0].Dialogue);
                SetDialogue(dialogueRespons[0]);

                dialogueIndex = 1;

                firstDialogue = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(dialogueRespons[dialogueIndex - 1].NextDialogue != null)
                {
                    SetDialogue(dialogueRespons[dialogueIndex - 1]);

                    stop = true;

                    return;
                }

                if(dialogueIndex == dialogueRespons.Count)
                {
                    DialogueEnd();

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
                SetDialogue(NPCDialogue.Dialogue);
            }
        }
    }

    public void SetDialogue(DialogueScriptableObject dialogueScriptable)
    {
        if (dialogueScriptable != null)
        {
            this.dialogueScriptable = dialogueScriptable;

            dialogueRespons = dialogueScriptable.DialogueRespons;

            dialogueHandler.gameObject.SetActive(true);

            answersHandler.DeleteAll();
            answersHandler.gameObject.SetActive(false);

            dialogueIndex = 0;

            firstDialogue = true;
            stop = false;
        }
        else if (NPCDialogue.Quest != null)
        {
            answersHandler.DeleteAll();

            SetQuest(); 
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
