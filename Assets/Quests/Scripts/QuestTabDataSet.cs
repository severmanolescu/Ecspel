using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestTabDataSet : MonoBehaviour
{
    [Header("Quest details")]
    [SerializeField] private TextMeshProUGUI questTitleText;
    [SerializeField] private TextMeshProUGUI questDetailText;

    [Header("Quest objective")]
    [SerializeField] private GameObject textQuestLog;
    [SerializeField] private Transform spawnLocationQuestLog;
    [SerializeField] private GameObject questLogPrefab;

    [Header("Quest rewards")]
    [SerializeField] private GameObject textRewardItems;
    [SerializeField] private Transform spawnLocationRewardItems;
    [SerializeField] private GameObject rewardItemPrefab;

    [Header("Quest icon near name")]
    [SerializeField] private GameObject questIcon;

    [SerializeField] private GameObject itemSlotPrefab;

    private void Awake()
    {
        DeleteData();
    }

    public void SetData(Quest quest)
    {
        questTitleText.text = quest.Title;
        questDetailText.text = quest.Details;

        questDetailText.gameObject.SetActive(true);
        questTitleText.gameObject.SetActive(true);

        textQuestLog.SetActive(true);
        textRewardItems.SetActive(true);
        questIcon.SetActive(true);

        DestroyOldQuestData();

        PlaceQuestRewards(quest.ReceiveItems);

        PlaceQuestLog(quest.QuestObjectives);
    }

    private void PlaceQuestLog(List<Objective> objectives)
    {
        if (objectives != null && objectives.Count > 0)
        {
            foreach (Objective objective in objectives)
            {
                if (objective != null)
                {
                    GameObject newLog = Instantiate(questLogPrefab, spawnLocationQuestLog);

                    newLog.GetComponentInChildren<TextMeshProUGUI>().text = objective.name;

                    Image logComplete = newLog.GetComponentInChildren<Image>();

                    logComplete.gameObject.SetActive(objective.Completed);

                    if (objective.Completed == false)
                    {
                        return;
                    }
                }
            }
        }
    }

    private void PlaceQuestRewards(List<ItemWithAmount> items)
    {
        if (items != null && items.Count > 0)
        {
            foreach (ItemWithAmount item in items)
            {
                QuestRewardDataSet questReward = Instantiate(rewardItemPrefab, spawnLocationRewardItems.transform).GetComponent<QuestRewardDataSet>();

                questReward.SetData(item);
            }
        }
    }

    private void DestroyOldQuestData()
    {
        TextMeshProUGUI[] questDetailsList = spawnLocationQuestLog.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI itemSlot in questDetailsList)
        {
            Destroy(itemSlot.transform.parent.gameObject);
        }

        questDetailsList = spawnLocationRewardItems.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI itemSlot in questDetailsList)
        {
            Destroy(itemSlot.transform.parent.gameObject);
        }
    }

    public void DeleteData()
    {
        questTitleText.text = "";
        questDetailText.text = "";

        textQuestLog.SetActive(false);
        textRewardItems.SetActive(false);
        questIcon.SetActive(false);

        questDetailText.gameObject.SetActive(false);
        questTitleText.gameObject.SetActive(false);

        DestroyOldQuestData();
    }
}