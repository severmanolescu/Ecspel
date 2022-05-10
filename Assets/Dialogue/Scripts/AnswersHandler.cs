using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnswersHandler : MonoBehaviour
{
    [SerializeField] private GameObject answerPrefab; 

    private List<GameObject> answers = new List<GameObject>();

    private DialogueChanger dialogueChanger;

    private PlayerInventory playerInventory;

    private QuestTabHandler questTab;

    private QuestTrack questTrack;

    private NpcId npcId;

    private ChangeDataFromQuest changeDataFromQuest;

    private void Awake()
    {
        playerInventory = GameObject.Find("Player/Canvas/PlayerItems").GetComponent<PlayerInventory>();

        questTab = GameObject.Find("Player/Canvas/QuestTab").GetComponent<QuestTabHandler>();

        questTrack = GameObject.Find("Player/Canvas/QuestTrack").GetComponent<QuestTrack>();

        npcId = GameObject.Find("Global").GetComponent<NpcId>();

        changeDataFromQuest = GameObject.Find("Global").GetComponent<ChangeDataFromQuest>();
    }

    public void SetDialogueChanger(DialogueChanger dialogueChanger)
    {
        this.dialogueChanger = dialogueChanger;
    }

    public void DeleteAll()
    {
        foreach (GameObject answer in answers)
        {
            Destroy(answer);
        }

        answers.Clear();
    }

    private void PlaceAnswers(List<DialogueAnswersClass> dialogueAnswers)
    {
        foreach (DialogueAnswersClass answer in dialogueAnswers)
        {
            GameObject ansferInstance = Instantiate(answerPrefab, gameObject.transform);

            ansferInstance.transform.localScale = answerPrefab.transform.localScale;

            ansferInstance.GetComponentInChildren<TextMeshProUGUI>().text = answer.Answer;

            ansferInstance.GetComponent<SetDataToAnsware>().HideReward();

            if (answer.NextDialogue == null)
            {
                ansferInstance.GetComponent<Button>().onClick.AddListener(DeleteAll);
            }

            ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { dialogueChanger.SetDialogue(answer.NextDialogue); });

            answers.Add(ansferInstance);
        }
    }

    public void SetAnswers(List<DialogueAnswersClass> dialogueAnswers)
    {
        if (dialogueAnswers != null)
        {
            DeleteAll();

            PlaceAnswers(dialogueAnswers);
        }
    }

    private void PlaceAnswers(Quest quest, bool found)
    {
        if (quest != null)
        {
            if (quest is GiveItem)
            {
                GameObject ansferInstance = Instantiate(answerPrefab, gameObject.transform);

                ansferInstance.transform.localScale = answerPrefab.transform.localScale;

                ansferInstance.GetComponentInChildren<TextMeshProUGUI>().text = quest.Title;

                GiveItem giveItem = (GiveItem)quest;

                ansferInstance.GetComponent<SetDataToAnsware>().ChangeReward(giveItem);

                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { playerInventory.DeteleItems(giveItem.ItemsNeeds); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { playerInventory.AddItem(quest.ItemsReceive); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { npcId.GetNpcFromId(giveItem.whoToGiveId).DeleteQuest(quest); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { dialogueChanger.SetDialogue(quest.NextDialogue); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { questTab.DeleteQuest(quest); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { questTrack.StopTrackQuest(); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { ChangeBonusesFromQuest(quest); });

                ansferInstance.GetComponent<Button>().interactable = found;

                answers.Add(ansferInstance);
            }
            else if(quest is QuestTalk)
            {
                GameObject ansferInstance = Instantiate(answerPrefab, gameObject.transform);

                QuestTalk questTalk = (QuestTalk)quest;

                ansferInstance.transform.localScale = answerPrefab.transform.localScale;

                ansferInstance.GetComponentInChildren<TextMeshProUGUI>().text = quest.Title;

                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { dialogueChanger.SetDialogue(quest.NextDialogue); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { questTab.DeleteQuest(quest); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { questTrack.StopTrackQuest(); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { ChangeBonusesFromQuest(quest); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { playerInventory.AddItem(quest.ItemsReceive); });
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { npcId.GetNpcFromId(questTalk.NpcId).DeleteQuest(quest); });

                answers.Add(ansferInstance);
            }
        }
    }

    private void ChangeBonusesFromQuest(Quest quest)
    {
        changeDataFromQuest.ChangeDataQuest(quest.IndexOfBonus, quest.Value);
    }

    public void SetAnswers(List<Quest> quests)
    {
        bool allFoundInInventory = false;

        foreach (Quest quest in quests)
        {
            if (quest is GiveItem)
            {
                GiveItem giveItem = (GiveItem)quest;

                foreach (QuestItems questItems in giveItem.ItemsNeeds)
                {
                    if (playerInventory.SearchInventory(questItems.Item, questItems.Amount))
                    {
                        allFoundInInventory = true;
                    }
                }

                PlaceAnswers(quest, allFoundInInventory);

                allFoundInInventory = false;
            }
            else if(quest is QuestTalk)
            {
                PlaceAnswers(quest, false);
            }
        }
    }
}
