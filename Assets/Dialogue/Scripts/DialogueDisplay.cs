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

    private List<Quest> quests = new List<Quest>();

    public List<Quest> Quest { get { return quests; } }

    public DialogueScriptableObject Dialogue { get { return dialogueScriptable; } set { dialogueScriptable = value; ChangeQuestMarkState(); } }

    private void Awake()
    {
        dialogueHandler = gameObject.GetComponentInChildren<DialogueHandler>();
        answersHandler = gameObject.GetComponentInChildren<AnswersHandler>();
        questMark = gameObject.transform.Find("QuestMark").gameObject;
    }

    public void Start()
    {
        dialogueHandler.gameObject.SetActive(false);
        answersHandler.gameObject.SetActive(false);

        ChangeQuestMarkState();

        if (dialogueScriptable != null)
        {
            foreach (Quest quest in dialogueScriptable.Quest)
            {  
                if(quest is GiveItem)
                {
                    GiveItem giveItem = (GiveItem)quest;

                    giveItem.WhoToGive = this.gameObject;

                    if (giveItem.nextDialogue != null && giveItem.NextQuest is GiveItem)
                    {
                        GiveItem giveItem1 = (GiveItem)giveItem.NextQuest;

                        giveItem1.WhoToGive = this.gameObject;
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

            DeleteDialogue();
        }
    }

    private void ChangeQuestMarkState()
    {
        if (dialogueScriptable != null)
        {
            if (dialogueScriptable.HaveQuest == true)
            {
                questMark.SetActive(true);
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
