using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogueScriptable;

    private GameObject questMark;

    private DialogueChanger dialogueChanger;

    private DialogueHandler dialogueHandler;
    private AnswersHandler answersHandler;

    private NpcPathFinding npcPathFinding;

    private ChangeDialogue changeDialogue;

    private int lastIdledirection = -1;

    private NpcId npcId;

    private List<Quest> quests = new List<Quest>();

    public List<Quest> Quest { get { return quests; } }

    public DialogueScriptableObject Dialogue { get { return dialogueScriptable; } set { dialogueScriptable = value; ChangeQuestMarkState(); } }

    private void Awake()
    {
        dialogueHandler = gameObject.GetComponentInChildren<DialogueHandler>();
        answersHandler = gameObject.GetComponentInChildren<AnswersHandler>();
        questMark = gameObject.transform.Find("QuestMark").gameObject;

        npcId = GameObject.Find("Global").GetComponent<NpcId>();

        npcPathFinding = GetComponent<NpcPathFinding>();

        changeDialogue = GetComponent<ChangeDialogue>();
    }

    public void Start()
    {
        dialogueHandler.gameObject.SetActive(false);
        answersHandler.gameObject.SetActive(false);

        ChangeQuestMarkState();

        ChangeQuestWhoToGive();
    }

    private void ChangeDialogue(DialogueScriptableObject dialogue)
    {
        if(changeDialogue != null)
        {
            if(changeDialogue.VerifyNewDialogueInList(dialogue) == true)
            {
                dialogueScriptable = dialogue;
            }
            else
            {
                dialogueScriptable = null;
            }
        }
        else
        {
            dialogueScriptable = dialogue;
        }
    }

    private void ChangeQuestWhoToGive()
    {
        if (dialogueScriptable != null)
        {
            foreach (Quest quest in dialogueScriptable.Quest)
            {
                if (quest is GiveItem)
                {
                    GiveItem giveItem = (GiveItem)quest;

                    giveItem.WhoToGive = npcId.GetNpcId(this);

                    if (giveItem.nextDialogue != null && giveItem.NextQuest is GiveItem)
                    {
                        GiveItem giveItem1 = (GiveItem)giveItem.NextQuest;

                        giveItem1.WhoToGive = npcId.GetNpcId(this);
                    }
                }

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialogueChanger = collision.GetComponentInChildren<DialogueChanger>();

            dialogueChanger.SetNCP(this);

            DeleteDialogue();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (dialogueChanger != null)
            {
                dialogueChanger.DeleteNPC();

                dialogueChanger = null;
            }

            CanWalkAgain();

            DeleteDialogue();
        }
    }

    public void ChangeIdleAnimationToPlayerPosition(Vector3 position)
    {
        lastIdledirection = npcPathFinding.GetIdleDirection();

        npcPathFinding.Talking = true;

        if(transform.position.x >= position.x)
        {
            npcPathFinding.MoveIdleAnimation(0);
        }
        else if(transform.position.x < position.x)
        {
            npcPathFinding.MoveIdleAnimation(2);
        }
    }

    private void ChangeQuestMarkState()
    {
        if (dialogueScriptable != null)
        {
            if (dialogueScriptable.Quest.Count > 0)
            {
                questMark.SetActive(true);

                ChangeQuestWhoToGive();
            }
            else
            {
                if (questMark != null)
                {
                    questMark.SetActive(false);
                }
            }
        }
        else
        {
            questMark.SetActive(false);
        }
    }

    public void SetDialogue(string dialogue)
    {
        dialogueHandler.gameObject.SetActive(true);

        dialogueHandler.SetDialogue(dialogue);
    }

    public void ShowAllText()
    {
        if(dialogueHandler != null && dialogueHandler.gameObject.activeSelf == true)
        {
            dialogueHandler.ShowAllText();
        }
    }

    public void DeleteDialogue()
    {
        dialogueHandler.StopDialogue();

        dialogueHandler.gameObject.SetActive(false);
    }

    public void CanWalkAgain()
    {
        if (lastIdledirection != -1)
        {
            npcPathFinding.MoveIdleAnimation(lastIdledirection);

            lastIdledirection = -1;

            npcPathFinding.Talking = false;
        }
    }

    public void AddDialogueToStart()
    {
        if (changeDialogue != null)
        {
            dialogueScriptable = changeDialogue.GetDialogue();
        }
    }

    public void AddQuest(Quest quest)
    {
        if(quest != null)
        {
            quests.Add(quest);
        }
    }

    public void DeleteQuest(Quest quest)
    {
        if(quests != null && quest != null)
        {
            quests.Remove(quest);
        }
    }
}
