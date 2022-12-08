using System.Collections.Generic;
using UnityEngine;

public class QuestTabHandler : MonoBehaviour
{
    [SerializeField] private GameObject answarePrefab;

    private Transform spawnLocation;

    private List<QuestButton> questButtons = new List<QuestButton>();

    private QuestFollowHandler questFollow;

    private GetQuest getQuest;

    private NpcId npcId;

    private void Awake()
    {
        getQuest = GameObject.Find("Global").GetComponent<GetQuest>();

        spawnLocation = transform.Find("ScrollView/Viewport/Content");
        questFollow = GameObject.Find("Player/QuestFollowObjects").GetComponent<QuestFollowHandler>();

        npcId = GameObject.Find("Global").GetComponent<NpcId>();
    }

    private void InstantiateButton(Quest quest)
    {
        GameObject @object = Instantiate(answarePrefab, spawnLocation.transform);

        @object.transform.localScale = answarePrefab.transform.localScale;

        questButtons.Add(@object.GetComponent<QuestButton>());

        @object.GetComponent<QuestButton>().SetData(quest, gameObject.GetComponent<QuestTabDataSet>());
    }

    public void SetQuestWorld(Quest quest)
    {
        if (quest is GiveItem)
        {
            GiveItem giveItem = (GiveItem)quest;

            npcId.GetNpcFromId(giveItem.whoToGiveId).AddQuest(quest);
        }
        else if (quest is GoToLocation)
        {
            questFollow.SetQuestFollow(quest);
        }
        else if (quest is CutTrees)
        {
            GetComponent<QuestCutTreesHandler>().SetCutTreesQuest(quest);
        }
        else if (quest is DestroyStone)
        {
            GetComponent<QuestDestroyStoneHandle>().SetDestroyStonequest(quest);
        }
        else if (quest is KillEnemy)
        {
            GetComponent<QuestKillEnemyHandle>().SetKillEnemyQuest(quest);
        }
        else if (quest is QuestTalk)
        {
            QuestTalk questTalk = (QuestTalk)quest;

            npcId.GetNpcFromId(questTalk.npcId).AddQuest(quest);
        }
    }

    private bool VerifyQuest(Quest quest)
    {
        if (quest != null && quest.Title != string.Empty)
        {
            foreach (QuestButton questButton in questButtons)
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

    public void AddQuest(Quest quest)
    {
        if (quest != null && VerifyQuest(quest))
        {
            InstantiateButton(quest);

            SetQuestWorld(quest);
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
        foreach (QuestButton questButton in questButtons)
        {
            if (questButton.Quest == quest)
            {
                questButtons.Remove(questButton);

                Destroy(questButton.gameObject);

                return;
            }
        }
    }

    public List<Quest> GetAllQuests()
    {
        List<Quest> quests = new List<Quest>();

        foreach (QuestButton questButton in questButtons)
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

        foreach (QuestButton questButton in questButtons)
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
        foreach (QuestButton questButton in questButtons)
        {
            Destroy(questButton.gameObject);
        }

        questButtons.Clear();
    }

    public void SetQuestsWithID(List<int> questsId)
    {
        DeleteAllQuest();

        foreach (int id in questsId)
        {
            AddQuest(getQuest.GetQuestFromID(id));
        }
    }
}
