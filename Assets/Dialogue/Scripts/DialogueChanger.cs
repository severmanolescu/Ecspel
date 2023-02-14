using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private AnswersHandler answersHandler;

    private DialogueScriptableObject dialogue;

    private int dialogueIndex = 0;

    private bool firstSpacePress = false;

    private DialogueDisplay dialogueDisplay;

    private void Awake()
    {
        gameObject.SetActive(false);

        answersHandler.DialogueChanger = this;
    }

    public void ShowDialogue(DialogueScriptableObject dialogue, DialogueDisplay dialogueDisplay = null)
    {
        if (dialogue != null)
        {
            if(dialogueDisplay != null && gameObject.activeSelf == false)
            {
                this.dialogueDisplay = dialogueDisplay;
            }
            

            this.dialogue = dialogue;

            dialogueIndex = 0;

            dialogueText.text = "";

            gameObject.SetActive(true);

            StopAllCoroutines();
            StartCoroutine(DialogueDisplay());
        }
    }

    public void StopDialogue()
    {
        StopAllCoroutines();

        gameObject.SetActive(false);

        dialogue = null;
    }

    private IEnumerator DialogueDisplay()
    {
        if (dialogue != null)
        {
            if (dialogueIndex < dialogue.DialogueRespons.Count)
            {
                dialogueText.text = "";

                firstSpacePress = false;

                for (int dialogueStringIndex = 0; dialogueStringIndex < dialogue.DialogueRespons[dialogueIndex].Length; dialogueStringIndex++)
                {
                    dialogueText.text = dialogueText.text + dialogue.DialogueRespons[dialogueIndex][dialogueStringIndex];

                    yield return new WaitForSeconds(0.1f);
                }

                firstSpacePress = true;  
            }            
        }

        CheckIfPlaceAnswers();
    }

    private void ShowAllText()
    {
        if (dialogueIndex < dialogue.DialogueRespons.Count)
        {
            dialogueText.text = dialogue.DialogueRespons[dialogueIndex];

            CheckIfPlaceAnswers();
        }
    }

    private void CheckIfPlaceAnswers()
    {
        if (dialogueIndex == dialogue.DialogueRespons.Count - 1)
        {
            if( dialogue.DialogueAnswers != null &&
                dialogue.DialogueAnswers.Count > 0)
            {
                answersHandler.ShowAnswers(dialogue);
            }
            else
            {
                if (dialogueDisplay != null)
                {
                    dialogueDisplay.FinishWalk();
                }

                if( dialogue.Quest != null &&
                    dialogue.Quest.Count > 0)
                {
                    AddQuests();
                }

                StopDialogue();
            }
        }
    }

    private void AddQuests()
    {

    }

    private void Update()
    {
        if (dialogue != null)
        {
            if(Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if(firstSpacePress == false)
                {
                    firstSpacePress = true;

                    StopAllCoroutines();

                    ShowAllText();
                }
                else
                {
                    dialogueIndex++;

                    StartCoroutine(DialogueDisplay());
                }
            }
        }
    }

}
