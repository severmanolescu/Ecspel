using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTabHandler : MonoBehaviour
{
    [SerializeField] private GameObject questPrefab;

    private List<Quest> quests = new List<Quest>();

    private GameObject questSpawn;

    private List<QuestButton> questButtons = new List<QuestButton>();

    private QuestShow questShow;

    private void Awake()
    {
        questSpawn = transform.Find("ScrollView/Viewport/Content").gameObject;

        questShow = gameObject.GetComponent<QuestShow>();
    }

    private void AddQuestToTab(Quest quest)
    {
        GameObject button = Instantiate(questPrefab);
        button.transform.SetParent(questSpawn.transform);
        button.transform.localScale = questPrefab.transform.localScale;

        QuestButton questButton = button.GetComponent<QuestButton>();

        questButton.SetQuest(quest, questShow);

        questButtons.Add(questButton);
    }

    private void DeleteQuestFromTab(Quest quest)
    {

    }

    public void DeleteQuest(Quest quest)
    {
        if(quest != null && quests.Contains(quest))
        {
            quests.Remove(quest);

            DeleteQuestFromTab(quest);
        }
    }

    public void AddQuest(Quest quest)
    {
        if(quest != null && !quests.Contains(quest))
        {
            quests.Add(quest);

            AddQuestToTab(quest);
        }
    }

}
