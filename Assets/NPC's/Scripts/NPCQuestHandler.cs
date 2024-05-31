using System.Collections.Generic;
using UnityEngine;

public class NPCQuestHandler : MonoBehaviour
{
    [SerializeField] private List<Dialogue> notHaveItems = new List<Dialogue>();

    [SerializeField] private List<Quest> quests = new List<Quest>();
    
    private PlayerInventory playerInventory;

    private QuestTabHandler questTab;

    private void Awake()
    {
        playerInventory = GameObject.Find("Global/Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();
        questTab = GameObject.Find("Global/Player/Canvas/QuestTab").GetComponent<QuestTabHandler>();
    }

    public void AddQuest(Quest quest)
    {
        if(quest != null && !quests.Contains(quest))
        {
            quests.Add(quest);
        }
    }

    private Dialogue GetRandomDialogue()
    {
        return notHaveItems[Random.Range(0, notHaveItems.Count - 1)];
    }

    private void AddPlayerReward(List<ItemWithAmount> items)
    {
        foreach (ItemWithAmount item in items)
        {
            Item newItem = item.Item.Copy();
            newItem.Amount = item.Amount;

            playerInventory.AddItem(newItem);
        }
    }

    private void AddPlayerReward(Quest quest)
    {
        AddPlayerReward(quest.ReceiveItems);
    }

    public void CompleteQuest(Quest quest)
    {
        questTab.DeleteQuest(quest);

        if(quest.NextQuest != null)
        {
            questTab.AddQuest(quest.NextQuest);
        }
    }

    public void CompleteQuest(Quest quest, ObjectiveGoTalk goTalk)
    {
        AddPlayerReward(goTalk.ReceiveItems);

        CompleteQuest(quest);
    }


    public void CompleteQuest(Quest quest, ObjectiveGiveItem questGiveItem)
    {
        foreach (ItemWithAmount item in questGiveItem.ItemsToGive)
        {
            Item newItem = item.Item.Copy();
            newItem.Amount = item.Amount;

            playerInventory.DeleteItem(newItem);
        }

        AddPlayerReward(quest);

        CompleteQuest(quest);
    }

    private bool CheckForGiveItem(ObjectiveGiveItem giveItem)
    {
        foreach (ItemWithAmount item in giveItem.ItemsToGive)
        {
            if (!playerInventory.SearchInventory(item.Item, item.Amount))
            {
                return false;
            }
        }

        return true;
    }

    public Dialogue CheckForQuest()
    {
        if(quests != null && quests.Count > 0)
        {
            foreach(Quest quest in quests)
            {
                switch (quest.QuestObjective)
                {
                    case ObjectiveGiveItem:
                        {
                            ObjectiveGiveItem questGive = (ObjectiveGiveItem)quest.QuestObjective;

                            if (CheckForGiveItem(questGive))
                            {
                                CompleteQuest(quest, questGive);

                                return quest.NextDialogue;
                            }

                            return GetRandomDialogue();
                        }
                    case ObjectiveGoTalk:
                        {
                            ObjectiveGoTalk goTalk = (ObjectiveGoTalk)quest.QuestObjective;

                            CompleteQuest(quest, goTalk);

                            return quest.NextDialogue;
                        }
                }  
            }
        }

        return null;
    }
}
