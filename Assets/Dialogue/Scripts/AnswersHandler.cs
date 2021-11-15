using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public class AnswersHandler : MonoBehaviour
{
    private GameObject answerPrefab; 

    private List<GameObject> answers = new List<GameObject>();

    private DialogueChanger dialogueChanger;

    private PlayerInventory playerInventory;

    private QuestTabHandler questTab;

    private QuestTrack questTrack;

    private void Awake()
    {
        answerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Dialogue/Prefabs/AnswerPrefab.prefab", typeof(GameObject));

        playerInventory = GameObject.Find("Player/Canvas/Field/Inventory/PlayerInventory").GetComponent<PlayerInventory>();

        questTab = GameObject.Find("Player/Canvas/Field/QuestTab").GetComponent<QuestTabHandler>();

        questTrack = GameObject.Find("Player/Canvas/QuestTrack").GetComponent<QuestTrack>();
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

            if (answer.NextDialogue == null)
            {
                ansferInstance.GetComponent<Button>().onClick.AddListener(DeleteAll);
            }
            else
            {
                ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { dialogueChanger.SetDialogue(answer.NextDialogue); });
            }

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
        GameObject ansferInstance = Instantiate(answerPrefab, gameObject.transform);

        ansferInstance.transform.localScale = answerPrefab.transform.localScale;

        ansferInstance.GetComponentInChildren<TextMeshProUGUI>().text = quest.Title;

        GiveItem giveItem = (GiveItem)quest;

        ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { playerInventory.DeteleItems(giveItem.ItemsNeeds); });
        ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { playerInventory.AddItem(quest.ItemsReceive); });
        ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { giveItem.WhoToGive.GetComponent<DialogueDisplay>().DeleteQuest(quest); });
        ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { dialogueChanger.SetDialogue(quest.NextDialogue); });
        ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { questTab.DeleteQuest(quest); });
        ansferInstance.GetComponent<Button>().onClick.AddListener(delegate { questTrack.StopTrackQuest(); });

        ansferInstance.GetComponent<Button>().interactable = found;

        answers.Add(ansferInstance);
    }

    public void SetAnswers(List<Quest> quests)
    {
        bool allFoundInInventory = false;

        foreach (Quest quest in quests)
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
    }
}
