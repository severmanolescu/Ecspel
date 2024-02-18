using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBetweenNPCs : MonoBehaviour
{
    [SerializeField] private float timeBeforSendDialogue;
    [SerializeField] private float timeBeforDisappear;

    [SerializeField] private List<NpcDialogue> dialogueList;

    [SerializeField] private DialogueDisplay firstNPC;
    [SerializeField] private DialogueDisplay secondNPC;

    [SerializeField] private bool talkWithPlayer = false;

    public DialogueDisplay FirstNPC  { get => firstNPC;  set => SetFirstNPC(value); }
    public DialogueDisplay SecondNPC { get => secondNPC; set => SetSecondNPC(value); }

    private void SetFirstNPC(DialogueDisplay newNpc)
    {
        if(newNpc == null)
        {
            HideTheUI();

            if(firstNPC != null)
            {
                firstNPC.DialogueBetweenNPCs = null;

                firstNPC = null;
            }
        }
        else
        {
            firstNPC = newNpc;

            if (firstNPC != null)
            {
                firstNPC.DialogueBetweenNPCs = this;
            }
        }
    }

    private void SetSecondNPC(DialogueDisplay newNpc)
    {
        if (newNpc == null)
        {
            HideTheUI();

            if(secondNPC != null)
            {
                BuilderHelperAI builderHelper = secondNPC.GetComponent<BuilderHelperAI>();

                if (builderHelper != null)
                {
                    builderHelper.ChangeWorkState(false);
                }

                secondNPC.DialogueBetweenNPCs = null;

                secondNPC = null;
            }
        }
        else
        {
            secondNPC = newNpc;

            if (secondNPC != null)
            {
                secondNPC.DialogueBetweenNPCs = this;
            }
        }
    }

    public void StartDialogue(int indexOfType)
    {
        if(!talkWithPlayer &&
            indexOfType >= 0 && indexOfType < dialogueList.Count &&
            firstNPC != null && secondNPC != null)
        {
            StopAllCoroutines();

            HideTheUI();

            firstNPC. MoveCanvasForObject(secondNPC.transform);
            secondNPC.MoveCanvasForObject(firstNPC.transform);

            int indexOfDialogue = Random.Range(0, dialogueList[indexOfType].Dialogues.Count - 1);

            DialogueNPCs dialogue = dialogueList[indexOfType].Dialogues[indexOfDialogue];

            if (dialogue != null)
            {
                if (!dialogue.DialogueRespons[0].whoRespond)
                {
                    secondNPC.StartShowDialogue(dialogue.DialogueRespons[0].dialogueText);
                    secondNPC.ShowAllText();

                    StartCoroutine(WaitBeforSecondNPC(firstNPC, dialogue.DialogueRespons[1].dialogueText));
                }
                else
                {
                    firstNPC.StartShowDialogue(dialogue.DialogueRespons[0].dialogueText);
                    firstNPC.ShowAllText();

                    StartCoroutine(WaitBeforSecondNPC(secondNPC, dialogue.DialogueRespons[1].dialogueText));
                }
            }
        }
    }

    private void HideTheUI()
    {
        if(firstNPC != null)
        {
            firstNPC.HideText(false);

            firstNPC.ResetCanvas();
        }
        if(secondNPC != null)
        {
            secondNPC.HideText(false);

            secondNPC.ResetCanvas();
        }
    }

    private IEnumerator WaitBeforSecondNPC(DialogueDisplay dialogueDisplay, string dialogueText)
    {
        yield return new WaitForSeconds(timeBeforSendDialogue);

        dialogueDisplay.StartShowDialogue(dialogueText);
        dialogueDisplay.ShowAllText();

        yield return new WaitForSeconds(timeBeforDisappear);

        HideTheUI();   
    }

    public void PlayerStopToTalk()
    {
        talkWithPlayer = false;
    }

    public void PlayerWantsToTalk()
    {
        talkWithPlayer = true;

        StopAllCoroutines();

        firstNPC.HideText(false);
        secondNPC.HideText(false);

        firstNPC.ResetCanvas();
        secondNPC.ResetCanvas();
    }
}
