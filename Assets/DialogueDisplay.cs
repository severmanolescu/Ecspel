using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogueScriptable;

    private DialogueChanger dialogueChanger;

    private DialogueHandler dialogueHandler;
    private AnswersHandler answersHandler;

    private void Awake()
    {
        dialogueHandler = gameObject.GetComponentInChildren<DialogueHandler>();
        answersHandler = gameObject.GetComponentInChildren<AnswersHandler>();
    }

    public void Start()
    {
        dialogueHandler.gameObject.SetActive(false);
        answersHandler.gameObject.SetActive(false);
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

    public DialogueScriptableObject GetDialogue()
    {
        return dialogueScriptable;
    }

}
