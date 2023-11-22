using System.Collections.Generic;
using UnityEngine;

public class NPCReceiveItem : MonoBehaviour
{
    [SerializeField] private List<DialogueScriptableObject> notHaveItems = new List<DialogueScriptableObject>();

    public List<Quest> quests = new List<Quest>();
    
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

    private DialogueScriptableObject GetRandomDialogue()
    {
        return notHaveItems[Random.Range(0, notHaveItems.Count - 1)];
    }

    public void CompleteQuest(Quest quest, QuestGiveItem questGiveItem)
    {
        foreach (ItemWithAmount item in questGiveItem.ItemsToGive)
        {
            Item newItem = item.Item.Copy();
            newItem.Amount = item.Amount;

            playerInventory.DeleteItem(newItem);
        }

        foreach (ItemWithAmount item in quest.ReceiveItems)
        {
            Item newItem = item.Item.Copy();
            newItem.Amount = item.Amount;

            playerInventory.AddItem(newItem);
        }

        questTab.DeleteQuest(quest);
    }

    public DialogueScriptableObject CheckForQuest()
    {
        if(quests != null && quests.Count > 0)
        {
            foreach(Quest quest in quests)
            {
                bool haveItems = true;

                QuestGiveItem questGive = (QuestGiveItem)quest.QuestObjective;

                foreach(ItemWithAmount item in questGive.ItemsToGive)
                {
                    if(!playerInventory.SearchInventory(item.Item, item.Amount))
                    {
                        haveItems = false;

                        break;
                    }
                }

                if(haveItems)
                {
                    CompleteQuest(quest, questGive);

                    return quest.NextDialogue;
                }
            }

            return GetRandomDialogue();
        }

        return null;
    }
}
