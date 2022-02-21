using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllDialogueAppear : MonoBehaviour
{
    [SerializeField] private GameObject locationOfDialogue;
    [SerializeField] private GetObjectReference getObject;

    public List<DialogueAppearSave> GetAllDialogue()
    {
        List<DialogueAppearSave> result = new List<DialogueAppearSave>();

        DialoguePlayerEnterInTrigger[] dialogues = locationOfDialogue.GetComponentsInChildren<DialoguePlayerEnterInTrigger>();

        foreach(DialoguePlayerEnterInTrigger dialogue in dialogues)
        {
            DialogueAppearSave newDialogueSave = new();

            newDialogueSave.DialogueID = dialogue.DialogueId;
            newDialogueSave.PositionX  = dialogue.transform.position.x;
            newDialogueSave.PositionY  = dialogue.transform.position.y;

            StartTimeDegradation startTime = dialogue.GetComponent<StartTimeDegradation>();

            if(startTime != null)
            {
                newDialogueSave.IdToAnotherObject = getObject.GetObjectId(startTime.ObjectWhereToStart);
            }
            else
            {
                newDialogueSave.IdToAnotherObject = -1;
            }

            Debug.Log(newDialogueSave.IdToAnotherObject);

            result.Add(newDialogueSave);
        }

        return result;
    }

    public void SetDialogueToWorld(List<DialogueAppearSave> dialogues)
    {
        DialoguePlayerEnterInTrigger[] dialoguesInGame = locationOfDialogue.GetComponentsInChildren<DialoguePlayerEnterInTrigger>();

        foreach(DialoguePlayerEnterInTrigger dialoguePlayerEnterInTrigger in dialoguesInGame)
        {
            Destroy(dialoguePlayerEnterInTrigger.gameObject);
        }

        foreach(DialogueAppearSave dialogue in dialogues)
        {
            GameObject dialogueObject = getObject.GetObjectFromId(dialogue.DialogueID);

            if(dialogueObject != null)
            {
                GameObject newObject = Instantiate(dialogueObject);

                newObject.transform.position = new Vector3(dialogue.PositionX, dialogue.PositionY);

                newObject.transform.parent = locationOfDialogue.transform;

                if (dialogue.IdToAnotherObject != -1)
                {
                    newObject.GetComponent<StartTimeDegradation>().ObjectWhereToStart = getObject.GetObjectFromId(dialogue.IdToAnotherObject);
                }
            }
        }
    }
}
