using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocationFollow : MonoBehaviour
{
    private Quest quest;

    private QuestTrack questTrack;
    private QuestTabHandler questTab;

    private Transform playerLocation;

    private int atIndex = 0;

    private bool track = false;

    public Quest Quest { get { return quest; } set { SetPositions(value); } }
    public bool Track { set { ChangeTrack(value); } }

    private void Awake()
    {
        questTrack = GameObject.Find("Player/Canvas/QuestTrack").GetComponent<QuestTrack>();
        questTab = GameObject.Find("Player/Canvas/Field/QuestTab").GetComponent<QuestTabHandler>();

        playerLocation = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if(quest != null && quest.Positions.Count > 0)
        {
            if(atIndex < quest.Positions.Count)
            {
                if(track == true)
                {
                    questTrack.TrackQuest(quest.Positions[atIndex]);
                }

                if(Vector3.Distance(playerLocation.localPosition, quest.Positions[atIndex]) < DefaulData.maxQuestDistante)
                {
                    atIndex++;
                }
            }
            else
            {
                questTab.DeleteQuest(quest);

                if (track == true)
                {
                    questTrack.StopTrackQuest();

                    track = false;
                }

                if(quest.NextQuest != null)
                {
                    questTab.AddQuest(quest.NextQuest);
                }

                quest = null;
            }
        }
    }

    private void SetPositions(Quest quest)
    {
        this.quest = quest;

        atIndex = 0;

        track = false;
    }

    private void ChangeTrack(bool track)
    {
        this.track = track;
    }
}