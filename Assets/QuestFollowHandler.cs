using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestFollowHandler : MonoBehaviour
{
    private GameObject prefab;

    private List<QuestLocationFollow> questLocations = new List<QuestLocationFollow>();

    private void Awake()
    {
        prefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Quests/Prefabs/QuestFollowPrefab.prefab", typeof(GameObject));
    }

    public QuestLocationFollow SetQuestFollow(Quest quest)
    {
        GameObject instantiateObject = Instantiate(prefab, transform);

        QuestLocationFollow questLocation = instantiateObject.GetComponent<QuestLocationFollow>();

        questLocation.Quest = quest;

        questLocations.Add(questLocation);

        return questLocation;
    }

    public void StartFollowQuest(Quest quest)
    {
        StopFollowQuest();

        foreach (QuestLocationFollow questLocation in questLocations)
        {
            if(questLocation.Quest == quest)
            {
                questLocation.Track = true;

                return;
            }
        }
    }

    public void StopFollowQuest()
    {
        foreach(QuestLocationFollow questLocation in questLocations)
        {
            questLocation.Track = false;
        }
    }
}
