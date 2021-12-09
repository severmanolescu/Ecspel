using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTabHandler : MonoBehaviour
{
     [SerializeField] private GameObject answarePrefab;

    private Transform spawnLocation;
    
    private List<QuestButton> questButtons = new List<QuestButton>();

    private QuestFollowHandler questFollow;

    private void Awake()
    {
        spawnLocation = transform.Find("ScrollView/Viewport/Content");
        questFollow = GameObject.Find("Player/QuestFollowObjects").GetComponent<QuestFollowHandler>();
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
        if(quest is GiveItem)
        {
            GiveItem giveItem = (GiveItem)quest;

            giveItem.WhoToGive.GetComponent<DialogueDisplay>().AddQuest(quest);
        }
        else if(quest is GoToLocation)
        {
            questFollow.SetQuestFollow(quest);
        }
        else if(quest is CutTrees)
        {
            GetComponent<QuestCutTreesHandler>().SetCutTreesQuest(quest);
        }
        else if (quest is DestroyStone)
        {
            GetComponent<QuestDestroyStoneHandle>().SetDestroyStonequest(quest);
        }
    }

    private bool VerifyQuest(Quest quest)
    {
        foreach(QuestButton questButton in questButtons)
        {
            if(questButton.Quest == quest)
            {
                return false;
            }
        }

        return true;
    }

    public void AddQuest(Quest quest)
    {
        if(quest != null && VerifyQuest(quest))
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
                if (VerifyQuest(quest))
                {
                    InstantiateButton(quest);

                    SetQuestWorld(quest);
                }
            }
        }
    }

    public void DeleteQuest(Quest quest)
    {
       foreach(QuestButton questButton in questButtons)
        {
            if(questButton.Quest == quest)
            {
                questButtons.Remove(questButton);

                Destroy(questButton.gameObject);

                return;
            }
        }
    }
}
