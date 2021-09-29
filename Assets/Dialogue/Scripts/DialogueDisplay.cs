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

    public DialogueScriptableObject Dialogue { get { return dialogueScriptable; } set { dialogueScriptable = value; ChangeQuestMarkState(); } }

    private void Awake()
    {
        dialogueHandler = gameObject.GetComponentInChildren<DialogueHandler>();
        answersHandler = gameObject.GetComponentInChildren<AnswersHandler>();
        questMark = gameObject.transform.Find("QuestMark").gameObject;
    }

    private void ChangeQuestMarkState()
    {
        if (dialogueScriptable != null)
        {
            if(dialogueScriptable.HaveQuest == true)
            {
                questMark.SetActive(true);
            }
            else
            {
                questMark.SetActive(false);
            }
        }
    }

    public void Start()
    {
        dialogueHandler.gameObject.SetActive(false);
        answersHandler.gameObject.SetActive(false);

        ChangeQuestMarkState();
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
            dialogueChanger.DeleteNPC();

            dialogueChanger = null;

            DeleteDialogue();
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

}
