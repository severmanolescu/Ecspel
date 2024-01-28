using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class DialogueBetweenNPCs : MonoBehaviour
{
    [SerializeField] private float timeBeforSendDialogue;
    [SerializeField] private float timeBeforDisappear;

    [SerializeField] private List<NpcDialogue> dialogueList;

    [SerializeField] private DialogueDisplay firstNPC;
    [SerializeField] private DialogueDisplay secondNPC;

    public DialogueDisplay FirstNPC  { get => firstNPC;  set => firstNPC  = value; }
    public DialogueDisplay SecondNPC { get => secondNPC; set => secondNPC = value; }



    public void StartDialogue(int indexOfType)
    {
        if(indexOfType >= 0 && indexOfType < dialogueList.Count)
        {
            int indexOfDialogue = Random.Range(0, dialogueList[indexOfType].dialogues.Count - 1);

            List<Dialogue> dialogue = dialogueList[indexOfType].dialogues[indexOfDialogue].list;

            if (dialogue != null)
            {
                if (dialogue[0].whoRespond)
                {
                    secondNPC.StartShowDialogue(dialogue[0].dialogueText);
                    secondNPC.ShowAllText();

                    StartCoroutine(WaitBeforSecondNPC(firstNPC, dialogue[1].dialogueText));
                }
                else
                {
                    firstNPC.StartShowDialogue(dialogue[0].dialogueText);
                    firstNPC.ShowAllText();

                    StartCoroutine(WaitBeforSecondNPC(secondNPC, dialogue[1].dialogueText));
                }
            }

        }
    }

    private IEnumerator WaitBeforSecondNPC(DialogueDisplay dialogueDisplay, string dialogueText)
    {
        yield return new WaitForSeconds(timeBeforSendDialogue);

        dialogueDisplay.StartShowDialogue(dialogueText);
        dialogueDisplay.ShowAllText();

        yield return new WaitForSeconds(timeBeforDisappear);

        firstNPC. HideText(false);
        secondNPC.HideText(false);
    }
}
