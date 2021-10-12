using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestLocationFollow : MonoBehaviour
{
    private Quest quest;

    private QuestTrack questTrack;
    private QuestTabHandler questTab;

    private int atIndex = 0;

    private bool track = false;

    private void Awake()
    {
        questTrack = GameObject.Find("Player/Canvas/QuestTrack").GetComponent<QuestTrack>();
        questTab = GameObject.Find("Player/Canvas/Field/QuestTab").GetComponent<QuestTabHandler>();
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

                if(Vector3.Distance(transform.localPosition, quest.Positions[atIndex]) < DefaulData.maxQuestDistante)
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

    public void SetPositions(Quest quest)
    {
        this.quest = quest;

        atIndex = 0;

        track = false;
    }

    public void ChangeTrack(bool track)
    {
        this.track = track;
    }
}
