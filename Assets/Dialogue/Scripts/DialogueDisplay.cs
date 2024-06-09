using System.Collections;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class DialogueDisplay : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    [SerializeField] private GameObject canvas;

    [SerializeField] private bool turnToPlayer = true;

    [SerializeField] private List<Event> events = new List<Event>();

    private TextMeshProUGUI text;

    private DialogueChanger dialogueChanger;

    private NpcPathFinding npcPathFinding;

    private PlayerMovement playerMovement;

    private CanvasTabsOpen playerCanvas;

    private string dialogueText;

    private NPCQuestHandler npcReceiveItem;

    private RandomDialogue randomDialogue;

    private DialogueBetweenNPCs dialogueBetweenNPCs;

    private bool canTalk = true;

    public bool CanTalk { get => canTalk; set => canTalk = value; }
    public DialogueBetweenNPCs DialogueBetweenNPCs { get => dialogueBetweenNPCs; set => dialogueBetweenNPCs = value; }

    private void Awake()
    {
        playerMovement = GameObject.Find("Global/Player").GetComponent<PlayerMovement>();

        dialogueChanger = GameObject.Find("Global/Player/Canvas/Dialogue").GetComponent<DialogueChanger>();
        playerCanvas    = GameObject.Find("Global/Player/Canvas").GetComponent<CanvasTabsOpen>();

        npcPathFinding = GetComponent<NpcPathFinding>();

        text = GetComponentInChildren<TextMeshProUGUI>();

        canvas.gameObject.SetActive(false);

        npcReceiveItem = GetComponent<NPCQuestHandler>();

        randomDialogue = GetComponent<RandomDialogue>();
    }

    private void StopWalk()
    {
        if (npcPathFinding != null)
        {
            npcPathFinding.Talking = true;
        }
    }

    private void StartWalk()
    {
        if (npcReceiveItem != null)
        {
            npcPathFinding.Talking = false;
        }
    }

    public void FinishTalk()
    {
        StartWalk();

        playerCanvas.ShowDefaultUIElements();

        TriggerEvents();

        StopDialogue();
    }

    private void TriggerEvents()
    {
        if (events != null && events.Count > 0)
        {
            foreach(Event @event in events)
            {
                @event.CanTrigger = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player") && collision.isTrigger == false)
        {
            FinishTalk();
        }
    }

    private void MoveAnimatorToThePlayer()
    {
        if(turnToPlayer)
        {
            npcPathFinding.SetAnimatorDirectionToLocation(playerMovement.transform.position);
        }
    }

    public void ShowDialogue()
    {
        if (canTalk)
        {
            if(dialogueBetweenNPCs != null)
            {
                dialogueBetweenNPCs.PlayerWantsToTalk();
            }

            if (dialogue != null)
            {
                dialogueChanger.ShowDialogue(dialogue, this);

                StopWalk();

                MoveAnimatorToThePlayer();

                playerCanvas.PrepareUIForDialogue();
            }
            else
            {
                Dialogue dialogue = npcReceiveItem.CheckForQuest();

                if (dialogue != null)
                {
                    this.dialogue = dialogue;

                    ShowDialogue();
                }
                else
                {
                    this.dialogue = randomDialogue.GetDialogue();
                }

                MoveAnimatorToThePlayer();

                playerCanvas.PrepareUIForDialogue();
            }
        }
        else
        {
            SetCantTalkDialogue();
        }
    }

    private void SetCantTalkDialogue()
    {
        if(dialogueBetweenNPCs != null)
        {
            dialogueBetweenNPCs.PlayerWantsToTalk();
        }

        dialogueChanger.ShowDialogue(randomDialogue.GetCantTalk(), this);
    }

    private void FinishCantTalk()
    {
        if(dialogueBetweenNPCs != null)
        {
            dialogueBetweenNPCs.PlayerStopToTalk();

            playerCanvas.ShowDefaultUIElements();
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

                yield return new WaitForSeconds(0.05f);
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

        FinishCantTalk();

        ResetCanvas();

        if(dialogue != null)
        {
            dialogue = dialogue.NextDialogue;
        }
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

        ResetCanvas();
    }

    private void MoveCanvasRight()
    {
        RectTransform transform = canvas.GetComponent<RectTransform>();

        transform.localPosition = new Vector3(Mathf.Abs(transform.localPosition.x), transform.localPosition.y, 0);

        transform.rotation = Quaternion.identity;
        text.GetComponent<RectTransform>().rotation = Quaternion.identity;
    }

    private void MoveCanvasLeft()
    {
        ResetCanvas();

        RectTransform transform = canvas.GetComponent<RectTransform>();
        RectTransform textTransform = text.GetComponent<RectTransform>();

        transform.localPosition = new Vector3(-Mathf.Abs(transform.localPosition.x), transform.localPosition.y, 0);

        textTransform.rotation = Quaternion.Euler(0f, -180f, 0f);
        transform.rotation     = Quaternion.Euler(0f, -180f, 0f);
    }

    public void MoveCanvasForObject(Transform objectTransform)
    {
        if(objectTransform.position.x > transform.position.x)
        {
            MoveCanvasLeft();
        }
        else
        {
            MoveCanvasRight();
        }
    }

    public void ResetCanvas()
    {
        MoveCanvasRight();
    }
}
