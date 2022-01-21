using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetQuest : MonoBehaviour
{
    [SerializeField] private List<Quest> quests = new List<Quest>();

    public int GetQuestID(Quest quest)
    {
        return quests.IndexOf(quest);
    }

    public Quest GetQuestFromID(int id)
    {
        return quests[id];
    }
}
