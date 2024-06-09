using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForQuests : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;

    [SerializeField] private List<Event> eventsSetToTrue  = new List<Event>();
    [SerializeField] private List<Event> eventsSetToFalse = new List<Event>();

    private PlayerAchievements playerAchievements;

    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerAchievements = GameObject.Find("Global/Player").GetComponent<PlayerAchievements>();

        playerMovement = playerAchievements.GetComponent<PlayerMovement>();
    }

    private void SetEvents()
    {
        foreach (Event @event in eventsSetToTrue)
        {
            if(@event != null)
            {
                @event.CanTrigger = true;
            }
        }

        foreach (Event @event in eventsSetToFalse)
        {
            if(@event != null)
            {
                @event.CanTrigger = false;
            }
        }
    }

    private void Update()
    {
        if ( playerAchievements.QuestCount >= 4 &&
             playerMovement.Dialogue == false   &&
             dialogue != null)
        {
            GameObject.Find("Global").GetComponent<SetDialogueToPlayer>().SetDialogue(dialogue);

            SetEvents();

            Destroy(gameObject);
        }
    }
}
