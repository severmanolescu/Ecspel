using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;

    private DialogueScriptableObject dialogue;

    private DialogueDisplay dialogueDisplay;

    private QuestTabHandler questTab;

    private int dialogueIndex = 0;

    private bool firstSpacePress = false;

    private bool answersPlaced = false;

    private void Awake()
    {
        questTab = GameObject.Find("Global/Player/Canvas/QuestTab").GetComponent<QuestTabHandler>();

        gameObject.SetActive(false);
    }

    public void ShowDialogue(DialogueScriptableObject dialogue, DialogueDisplay dialogueDisplay = null)
    {
        if (dialogue != null)
        {
            if (dialogueDisplay != null && gameObject.activeSelf == false)
            {
                this.dialogueDisplay = dialogueDisplay;

                answersPlaced = false;
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

        if (dialogueDisplay != null)
        {
            dialogueDisplay.FinishTalk();
        }

        if (dialogue != null &&
            dialogue.Quest != null &&
            dialogue.Quest.Count > 0)
        {
            AddQuests();
        }

        dialogue = null;

        answersPlaced = false;
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
            else if (answersPlaced == false || (dialogue.Quest != null && dialogue.Quest.Count > 0))
            {
                StopDialogue();
            }
        }
        else
        {
            StopDialogue();
        }
    }

    private void ShowAllText()
    {
        if (dialogueIndex < dialogue.DialogueRespons.Count)
        {
            dialogueText.text = dialogue.DialogueRespons[dialogueIndex];
        }
    }

    private void AddQuests()
    {
        questTab.AddQuest(dialogue.Quest);
    }

    private void Update()
    {
        if (dialogue != null)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (firstSpacePress == false)
                {
                    StopAllCoroutines();

                    ShowAllText();

                    firstSpacePress = true;
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
