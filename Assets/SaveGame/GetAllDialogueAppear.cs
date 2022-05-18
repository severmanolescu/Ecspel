using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetAllDialogueAppear : MonoBehaviour
{
    [SerializeField] private GameObject locationOfDialogue;
    [SerializeField] private GetObjectReference getObject;
    [SerializeField] private GetLocationGrid getLocation;

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

            if (startTime != null)
            {
                newDialogueSave.IdToAnotherObject = getObject.GetObjectId(startTime.ObjectWhereToStart);
            }
            else
            {
                StartWalkToNPC startWalk = dialogue.GetComponent<StartWalkToNPC>();

                if (startWalk != null)
                {
                    newDialogueSave.IdToAnotherObject = getObject.GetObjectId(startWalk.NPC);

                    List<NpcPathSave> paths = new List<NpcPathSave>();

                    foreach (NpcTimeSchedule npcTime in startWalk.NpcTimeSchedules)
                    {
                        paths.Add(new NpcPathSave(npcTime.Position.x,
                                                  npcTime.Position.y,
                                                  getLocation.GetNoFromLocation(npcTime.LocationGrid),
                                                  npcTime.IdleDirection,
                                                  npcTime.Seconds,
                                                  npcTime.Hours,
                                                  npcTime.Minutes));
                    }

                    newDialogueSave.Path = paths;

                    newDialogueSave.SecondsToDestroy = startWalk.Seconds;
                    newDialogueSave.StopPlayerForMoving = startWalk.StopPlayerFromMoving;
                }
                else
                {
                    newDialogueSave.IdToAnotherObject = -1;
                    newDialogueSave.Path = null;
                }
            }

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

            Debug.Log(dialogue.DialogueID);

            if(dialogueObject != null)
            {
                GameObject newObject = Instantiate(dialogueObject);

                newObject.name = newObject.name.Replace("(Clone)", "");

                newObject.transform.position = new Vector3(dialogue.PositionX, dialogue.PositionY);

                newObject.transform.parent = locationOfDialogue.transform;

                newObject.GetComponent<DialoguePlayerEnterInTrigger>().DialogueId = dialogue.DialogueID;

                if (dialogue.IdToAnotherObject != -1)
                {
                    StartTimeDegradation startTime = newObject.GetComponent<StartTimeDegradation>();

                    if (startTime != null)
                    {
                        startTime.ObjectWhereToStart = getObject.GetObjectFromId(dialogue.IdToAnotherObject);
                    }
                    else
                    {
                        StartWalkToNPC startWalk = newObject.GetComponent<StartWalkToNPC>();

                        if (startWalk != null)
                        {
                            startWalk.NPC = getObject.GetObjectFromId(dialogue.IdToAnotherObject);

                            startWalk.NpcTimeSchedules.Clear();

                            foreach(NpcPathSave npcPath in dialogue.Path)
                            {
                                NpcTimeSchedule npcTime = new NpcTimeSchedule();

                                npcTime.LocationGrid = getLocation.GetLocationFromNo(npcPath.LocationID);
                                npcTime.Position = new Vector3(npcPath.LocationX, npcPath.LocationY);
                                npcTime.Seconds = npcPath.WaitForSeconds;
                                npcTime.Hours = npcPath.WaitForHour;
                                npcTime.Minutes = npcPath.WaitForMinute;

                                startWalk.NpcTimeSchedules.Add(npcTime);
                            }
                        }
                    }
                }
            }
        }
    }
}
