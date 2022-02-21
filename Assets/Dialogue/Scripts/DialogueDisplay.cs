using System.Collections;
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
    }

    public void Start()
    {
        dialogueHandler.gameObject.SetActive(false);
        answersHandler.gameObject.SetActive(false);

        ChangeQuestMarkState();

        ChangeQuestWhoToGive();
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

        npcPathFinding.CanWalk = false;

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
            if (dialogueScriptable.HaveQuest == true)
            {
                questMark.SetActive(true);

                ChangeQuestWhoToGive();
            }
            else
            {
                questMark.SetActive(false);
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

            npcPathFinding.CanWalk = true;
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
