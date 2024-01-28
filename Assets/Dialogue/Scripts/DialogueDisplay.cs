using System.Collections;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private DialogueScriptableObject dialogue;

    [SerializeField] private GameObject canvas;

    private TextMeshProUGUI text;

    private DialogueChanger dialogueChanger;

    private NpcPathFinding npcPathFinding;

    private PlayerMovement playerMovement;

    private CanvasTabsOpen canvasTabs;

    private string dialogueText;

    private NPCReceiveItem npcReceiveItem;

    private RandomDialogue randomDialogue;

    private void Awake()
    {
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();

        dialogueChanger = GameObject.Find("Global/Player/Canvas/Dialogue").GetComponent<DialogueChanger>();
        canvasTabs = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();

        npcPathFinding = GetComponent<NpcPathFinding>();

        text = GetComponentInChildren<TextMeshProUGUI>();

        canvas.gameObject.SetActive(false);

        npcReceiveItem = GetComponent<NPCReceiveItem>();

        randomDialogue = GetComponent<RandomDialogue>();
    }

    private void StopWalk()
    {
        npcPathFinding.Talking = true;
    }

    private void StartWalk()
    {
        npcPathFinding.Talking = false;
    }

    public void FinishTalk()
    {
        StartWalk();

        canvasTabs.ShowDefaultUIElements();

        StopDialogue();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && collision.isTrigger == false)
        {
            FinishTalk();
        }
    }

    public void ShowDialogue()
    {
        if(dialogue != null)
        {
            dialogueChanger.ShowDialogue(dialogue, this);

            StopWalk();

            npcPathFinding.SetAnimatorDirectionToLocation(playerMovement.transform.position);

            canvasTabs.PrepareUIForDialogue();
        }
        else
        {
            DialogueScriptableObject dialogue = npcReceiveItem.CheckForQuest();

            if(dialogue != null)
            {
                this.dialogue = dialogue; 

                ShowDialogue();
            }
            else
            {
                this.dialogue = randomDialogue.GetDialogue();
            }

            npcPathFinding.SetAnimatorDirectionToLocation(playerMovement.transform.position);

            canvasTabs.PrepareUIForDialogue();
        }
    }

    public void HideText(bool hide)
    {
        text.text = "";

        canvas.gameObject.SetActive(hide);
    }

    private IEnumerator WaitForDialogueDisplay()
    {
        if (dialogueText.CompareTo(string.Empty) != 0)
        {
            HideText(true);

            for (int dialogueStringIndex = 0; dialogueStringIndex < dialogueText.Length; dialogueStringIndex++)
            {
                text.text = text.text + dialogueText[dialogueStringIndex];

                yield return new WaitForSeconds(0.1f);
            }
        }

        FinishDialogue();
    }

    public void StartShowDialogue(string dialogueText)
    {
        StopAllCoroutines();

        this.dialogueText = dialogueText;

        StartCoroutine(WaitForDialogueDisplay());
    }

    public void StopDialogue()
    {
        StopAllCoroutines();

        HideText(false);
    }

    public void NextDialogue()
    {
        if (dialogue != null && dialogue.NextDialogue != null)
        {
            dialogue = dialogue.NextDialogue;
        }
        else
        {
            dialogue = null;
        }
    }

    public void ShowAllText()
    {
        StopAllCoroutines();

        canvas.gameObject.SetActive(true);

        text.text = dialogueText;
    }

    private void FinishDialogue()
    {
        dialogueChanger.NPCDialogueFinish();
    }
}
