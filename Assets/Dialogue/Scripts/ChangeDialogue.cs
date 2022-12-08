using System.Collections.Generic;
using UnityEngine;

public class ChangeDialogue : MonoBehaviour
{
    [SerializeField] private List<DialogueChoose> dialogueChoose = new();

    private DialogueDisplay dialogueDisplay;

    private DayTimerHandler dayTimerHandler;

    private void Awake()
    {
        dialogueDisplay = GetComponent<DialogueDisplay>();

        dayTimerHandler = GameObject.Find("Global/DayTimer").GetComponent<DayTimerHandler>();
    }

    private bool VerifyDialogueWithGameData(DialogueChoose dialogue)
    {
        if (dialogue.hourOfStart != 0 && dialogue.hourOfEnd != 0)
        {
            if (dialogue.hourOfStart > dialogue.hourOfEnd)
            {
                if (dayTimerHandler.Hours > dialogue.hourOfStart && dayTimerHandler.Hours < dialogue.hourOfEnd)
                {
                    return false;
                }
            }

            if (dayTimerHandler.Hours < dialogue.hourOfStart || dayTimerHandler.Hours > dialogue.hourOfEnd)
            {
                return false;
            }
        }

        switch (dialogue.weatherType)
        {
            case -1: return true;
            case 0:
                {
                    if (dayTimerHandler.Raining == false && dayTimerHandler.Fog == false)
                    {
                        return true;
                    }

                    return false;
                }
            case 1:
                {
                    if (dayTimerHandler.Raining == true && dayTimerHandler.Fog == false)
                    {
                        return true;
                    }

                    return false;
                }
            case 2:
                {
                    if (dayTimerHandler.Raining == false && dayTimerHandler.Fog == true)
                    {
                        return true;
                    }

                    return false;
                }
            case 3:
                {
                    if (dayTimerHandler.Raining == true && dayTimerHandler.Fog == true)
                    {
                        return true;
                    }

                    return false;
                }
        }

        return false;
    }

    private DialogueChoose GetDialogueByGameData()
    {
        if (dialogueChoose == null || dialogueChoose.Count == 0)
        {
            return null;
        }

        int indexOfDialogue = Random.Range(0, dialogueChoose.Count - 1);

        DialogueChoose choose = dialogueChoose[indexOfDialogue];

        while (VerifyDialogueWithGameData(choose) == false)
        {
            indexOfDialogue = Random.Range(0, dialogueChoose.Count - 1);

            choose = dialogueChoose[indexOfDialogue];
        }

        return choose;
    }

    private DialogueScriptableObject ChangeNewDialogueNextDialogue(DialogueScriptableObject dialogue)
    {
        if (dialogue.NextDialogue == null)
        {
            if (dialogueDisplay.Dialogue != null)
            {
                if (VerifyNewDialogueInList(dialogueDisplay.Dialogue))
                {
                    dialogue.NextDialogue = dialogueDisplay.Dialogue;
                }
                else
                {
                    dialogue.NextDialogue = dialogueDisplay.Dialogue.NextDialogue;
                }
            }
        }

        return dialogue;
    }

    public DialogueScriptableObject GetDialogue()
    {
        if (dialogueDisplay != null)
        {
            if (dialogueDisplay.Dialogue == null)
            {
                return GetDialogueByGameData().dialogue.Copy();
            }
            else
            {
                return ChangeNewDialogueNextDialogue(GetDialogueByGameData().dialogue.Copy());
            }
        }

        return null;
    }

    public bool VerifyNewDialogueInList(DialogueScriptableObject dialogue)
    {
        if (dialogue == null)
        {
            return true;
        }

        if (dialogueChoose != null)
        {
            foreach (DialogueChoose listDialogue in dialogueChoose)
            {
                if (listDialogue.dialogue == dialogue)
                {
                    return false;
                }
            }

            return true;
        }
        else
        {
            return false;
        }
    }
}
