using System.Collections.Generic;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogue;

    private DialogueChanger dialogueChanger;

    private void Awake()
    {
        dialogueChanger = GameObject.Find("Global/Player/Canvas/Dialogue").GetComponent<DialogueChanger>();
    }

    private void StopWalk()
    {

    }

    private void StartWalk()
    {

    }

    public void FinishWalk()
    {
        StartWalk();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision != null && collision.CompareTag("Player")) 
        {
            StopWalk();

            dialogueChanger.ShowDialogue(dialogue, this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            StartWalk();

            dialogueChanger.StopDialogue();
        }
    }
}
