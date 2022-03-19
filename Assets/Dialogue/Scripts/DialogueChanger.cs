using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueChanger : MonoBehaviour
{
    private DialogueScriptableObject dialogueScriptable = null;

    private DialogueHandler dialogueHandler;
    private AnswersHandler answersHandler;

    private DialogueDisplay NPCDialogue;

    private List<DialogueClass> dialogueRespons = null;

    private QuestTabHandler questTab;

    private NpcId npcId;

    private bool firstDialogue = false;

    private bool stop = true;

    private int dialogueIndex = 0;

    private Transform playerLocation;

    private Keyboard keyboard;

    private SetDialogueToPlayer setDialogueToPlayer = null;

    private void Awake()
    {
        dialogueHandler = gameObject.GetComponentInChildren<DialogueHandler>();

        answersHandler  = gameObject.GetComponentInChildren<AnswersHandler>();

        questTab = GameObject.Find("Player/Canvas/QuestTab").GetComponent<QuestTabHandler>();

        npcId = GameObject.Find("Global").GetComponent<NpcId>();

        keyboard = InputSystem.GetDevice<Keyboard>();

        playerLocation = GameObject.Find("Global/Player").transform;
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
            dialogueHandler.SetDialogue(dialogue.Dialogue);

            if (NPCDialogue != null)
            {
                NPCDialogue.DeleteDialogue();
            }

            dialogueHandler.gameObject.SetActive(true);
        }
        else if(NPCDialogue != null)
        {
            NPCDialogue.SetDialogue(dialogue.Dialogue);

            dialogueHandler.gameObject.SetActive(false);
        }
    }

    private void SetQuest()
    {
        SetQuestWhoToGive();

        answersHandler.SetAnswers(NPCDialogue.Quest);

        answersHandler.gameObject.SetActive(true);
        dialogueHandler.gameObject.SetActive(false);

        NPCDialogue.DeleteDialogue();
    }

    private void SetQuestWhoToGive()
    {
        if (dialogueScriptable != null)
        {
            foreach (Quest quest in dialogueScriptable.Quest)
            {
                if (quest != null && quest is GiveItem)
                {
                    GiveItem giveItem = (GiveItem)quest;

                    if (giveItem != null && giveItem.WhoToGive == -1)
                    {
                        giveItem.WhoToGive = npcId.GetNpcId(NPCDialogue.GetComponent<DialogueDisplay>());
                    }
                }
            }
        }
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

        if(NPCDialogue != null && NPCDialogue.Quest.Count > 0)
        {
            SetQuest();
        }
        else if (NPCDialogue != null && dialogueScriptable.NextDialogue != null)
        {
            NPCDialogue.Dialogue = dialogueScriptable.NextDialogue;
        }
        else if(NPCDialogue != null && dialogueScriptable.NextDialogue == null)
        {
            NPCDialogue.Dialogue = null;
        }

        if(NPCDialogue != null && dialogueScriptable.Quest.Count > 0)
        {
            SetQuestWhoToGive();

            questTab.AddQuest(dialogueScriptable.Quest);

            NPCDialogue.DeleteDialogue();
        } 

        if(setDialogueToPlayer != null)
        {
            setDialogueToPlayer.DialogueEnd();
        }

        setDialogueToPlayer = null;

        stop = true;
    }

    private void Update()
    {
        if (!stop && dialogueScriptable != null && dialogueIndex <= dialogueRespons.Count)
        {
            if (dialogueRespons.Count == 0)
            {
                DialogueEnd();

                NPCDialogue.CanWalkAgain();
            }

            if (firstDialogue == true)
            {
                if (dialogueRespons.Count > 0)
                {
                    dialogueHandler.SetDialogue(dialogueRespons[0].Dialogue);
                    SetDialogue(dialogueRespons[0]);

                    dialogueIndex = 1;

                    firstDialogue = false;
                }
                else
                {
                    DialogueEnd();

                    return;
                }
            }

            if (keyboard.spaceKey.wasPressedThisFrame)
            {
                if(dialogueRespons[dialogueIndex - 1].NextDialogue != null)
                {
                    Debug.Log("asd");

                    NPCDialogue.DeleteDialogue();

                    DeleteDialogue();

                    SetDialogue(dialogueRespons[dialogueIndex - 1].NextDialogue);

                    return;
                }

                if(dialogueIndex >= dialogueRespons.Count)
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
            if (keyboard.fKey.wasPressedThisFrame)
            {
                if (stop)
                {
                    NPCDialogue.ChangeIdleAnimationToPlayerPosition(playerLocation.position);

                    SetDialogue(NPCDialogue.Dialogue);
                }
                else
                {
                    DialogueEnd();
                }
            }
        }
    }

    public void SetDialogue(DialogueScriptableObject dialogueScriptable, SetDialogueToPlayer setDialogueToPlayer = null)
    {
        this.setDialogueToPlayer = setDialogueToPlayer;

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

        setDialogueToPlayer = null;
    }

    public void SetNCP(DialogueDisplay NPCDialogue)
    {
        this.NPCDialogue = NPCDialogue;
    }

    public void DeleteNPC()
    {
        NPCDialogue.DeleteDialogue();

        NPCDialogue = null;

        DeleteDialogue();
    }
}
