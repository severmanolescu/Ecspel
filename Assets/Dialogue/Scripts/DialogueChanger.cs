using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueChanger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;

    [SerializeField] private GameObject background;

    private DialogueScriptableObject dialogue;

    private DialogueDisplay NPCDialogue;

    private QuestTabHandler questTab;

    private int dialogueIndex = 0;

    private bool firstSpacePress = false;

    private SetDialogueToPlayer setDialogueToPlayer;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();

        setDialogueToPlayer = GameObject.Find("Global").GetComponent<SetDialogueToPlayer>();

        questTab = GameObject.Find("Global/Player/Canvas/QuestTab").GetComponent<QuestTabHandler>();

        background.SetActive(false);
    }

    public void ShowDialogue(DialogueScriptableObject dialogue, DialogueDisplay dialogueDisplay = null)
    {
        if (dialogue != null)
        {
            firstSpacePress = false;

            playerMovement.Dialogue = true;

            if (dialogueDisplay != null && background.activeSelf == false)
            {
                this.NPCDialogue = dialogueDisplay;
            }

            this.dialogue = dialogue;

            dialogueIndex = 0;

            if (dialogue.DialogueRespons[0].whoRespond)
            {
                NPCDialogue.StartShowDialogue(dialogue.DialogueRespons[0].dialogueText);

                HideDialogue();
            }
            else
            {
                dialogueIndex = 0;

                dialogueText.text = "";

                background.SetActive(true);

                StopAllCoroutines();
                StartCoroutine(DialogueDisplay());
            } 
        }
    }

    public void NPCDialogueFinish()
    {
        firstSpacePress = true;
    }

    public void StopDialogue()
    {
        StopAllCoroutines();

        background.SetActive(false);

        if (NPCDialogue != null)
        {
            NPCDialogue.FinishTalk();
        }

        if (dialogue != null &&
            dialogue.Quest != null &&
            dialogue.Quest.Count > 0)
        {
            AddQuests();
        }

        dialogue = null;

        setDialogueToPlayer.DialogueEnd();
    }

    private IEnumerator DialogueDisplay()
    {
        background.SetActive(true);

        if (dialogue != null)
        {
            if (dialogueIndex < dialogue.DialogueRespons.Count)
            {
                dialogueText.text = "";

                firstSpacePress = false;

                for (int dialogueStringIndex = 0; dialogueStringIndex < dialogue.DialogueRespons[dialogueIndex].dialogueText.Length; dialogueStringIndex++)
                {
                    dialogueText.text = dialogueText.text + dialogue.DialogueRespons[dialogueIndex].dialogueText[dialogueStringIndex];

                    yield return new WaitForSeconds(0.1f);
                }

                firstSpacePress = true;
            }
            else if (dialogue.Quest != null && dialogue.Quest.Count > 0)
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
            dialogueText.text = dialogue.DialogueRespons[dialogueIndex].dialogueText;
        }
    }

    private void AddQuests()
    {
        questTab.AddQuest(dialogue.Quest);
    }

    private void HideDialogue()
    {
        background.SetActive(false);
    }

    private void Update()
    {
        if (dialogue != null)
        {
            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (firstSpacePress == false)
                {
                    if (dialogue.DialogueRespons[dialogueIndex].whoRespond)
                    {
                        NPCDialogue.ShowAllText();
                    }
                    else
                    {
                        StopAllCoroutines();

                        ShowAllText();
                    }

                    firstSpacePress = true;
                }
                else
                {
                    dialogueIndex++;

                    if (dialogueIndex >= dialogue.DialogueRespons.Count)
                    {
                        StopDialogue();
                        NPCDialogue.NextDialogue();

                        HideDialogue();

                        playerMovement.Dialogue = false;
                    }
                    else
                    {
                        if (dialogue.DialogueRespons[dialogueIndex].whoRespond)
                        {
                            NPCDialogue.StartShowDialogue(dialogue.DialogueRespons[dialogueIndex].dialogueText);

                            HideDialogue();
                        }
                        else
                        {
                            NPCDialogue.StopDialogue();

                            StartCoroutine(DialogueDisplay());
                        }

                        firstSpacePress = false;
                    }
                }
            }
        }
    }

}
