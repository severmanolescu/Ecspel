using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestTabHandler : MonoBehaviour
{
    [SerializeField] private GameObject questButtonPrefab;

    [SerializeField] private Transform spawLocation;

    [SerializeField] private List<Quest> testQuestHandler = new List<Quest>();

    private List<QuestButton> activeQuests = new List<QuestButton>();

    private PlayerInventory playerInventory;

    private GetQuest getQuest;

    private QuestToWorldHandler questToWorld;

    private SpawnItem spawnItem;

    private Transform playerPosition;

    private void Awake()
    {
        DeleteAllQuest();

        gameObject.SetActive(false);

        getQuest = GameObject.Find("Global").GetComponent<GetQuest>();

        questToWorld = GameObject.Find("Global").GetComponent<QuestToWorldHandler>();

        questToWorld.QuestTab = this;

        playerPosition = GameObject.Find("Global/Player").transform;

        spawnItem = GameObject.Find("Global").GetComponent<SpawnItem>();

        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();
    }

    private void InstantiateButton(Quest quest)
    {
        GameObject @object = Instantiate(questButtonPrefab, spawLocation);

        @object.transform.localScale = questButtonPrefab.transform.localScale;

        activeQuests.Add(@object.GetComponent<QuestButton>());

        @object.GetComponent<QuestButton>().SetData(quest, GetComponent<QuestTabDataSet>());
    }

    private bool VerifyQuest(Quest quest)
    {
        if (quest != null && quest.Title != string.Empty)
        {
            foreach (QuestButton questButton in activeQuests)
            {
                if (questButton.Quest == quest)
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

    private Quest ResetQuestData(Quest quest)
    {
        quest.QuestObjective.Completed = false;

        return quest;
    }
    public void AddQuest(Quest quest)
    {
        if (quest != null && VerifyQuest(quest))
        {
            Quest questCopy = quest.Copy();

            ResetQuestData(questCopy);

            InstantiateButton(questCopy);

            questToWorld.SetQuestToWorld(questCopy);
        }
    }

    public void AddQuest(List<Quest> questList)
    {
        if (questList != null)  
        {
            foreach (Quest quest in questList)
            {
                AddQuest(quest);
            }
        }
    }

    public void DeleteQuest(Quest quest)
    {
        foreach (QuestButton questButton in activeQuests)
        {
            if (questButton.Quest == quest)
            {
                activeQuests.Remove(questButton);

                Destroy(questButton.gameObject);

                return;
            }
        }
    }

    public List<Quest> GetAllQuests()
    {
        List<Quest> quests = new List<Quest>();

        foreach (QuestButton questButton in activeQuests)
        {
            if (questButton.Quest != null)
            {
                quests.Add(questButton.Quest);
            }
        }

        return quests;
    }

    public List<int> GetAllQuestID()
    {
        List<int> questIDs = new List<int>();

        foreach (QuestButton questButton in activeQuests)
        {
            if (questButton != null && questButton.Quest != null)
            {
                questIDs.Add(getQuest.GetQuestID(questButton.Quest));
            }
        }

        return questIDs;
    }

    private void DeleteAllQuest()
    {
        foreach (QuestButton questButton in activeQuests)
        {
            Destroy(questButton.gameObject);
        }

        activeQuests.Clear();

        DeleteQuestsFromLocation(spawLocation);
    }

    private void DeleteQuestsFromLocation(Transform location)
    {
        Button[] oldQuests = location.GetComponentsInChildren<Button>();

        foreach (Button quest in oldQuests)
        {
            if (quest != location)
            {
                Destroy(quest.gameObject);
            }
        }
    }

    public void AddItemsToInventory(List<ItemWithAmount> items)
    {
        foreach (ItemWithAmount item in items)
        {
            Item itemCopy = item.Item.Copy();
            itemCopy.Amount = item.Amount;

            int leftAmount = playerInventory.AddItem(itemCopy);

            itemCopy.Amount = leftAmount;

            spawnItem.SpawnItems(itemCopy, playerPosition.position);
        }
    }

    private void Start()
    {
        foreach(Quest quest in testQuestHandler)
        {
            AddQuest(quest);
        }
    }
}