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
    [SerializeField] private TextMeshProUGUI objectiveText;
    [SerializeField] private TextMeshProUGUI objectiveLog;

    [Header("Quest rewards")]
    [SerializeField] private GameObject textRewardItems;
    [SerializeField] private Transform spawnLocationRewardItems;
    [SerializeField] private GameObject rewardItemPrefab;

    [Header("Quest icon near name")]
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

        objectiveText.gameObject.SetActive(true);
        objectiveLog.gameObject.SetActive(true);
        textRewardItems.SetActive(true);

        DestroyOldQuestData();

        PlaceQuestRewards(quest.ReceiveItems);

        objectiveText.text = quest.QuestObjective.ObjectiveName;
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
        TextMeshProUGUI[] questDetailsList = spawnLocationRewardItems.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (TextMeshProUGUI itemSlot in questDetailsList)
        {
            Destroy(itemSlot.transform.parent.gameObject);
        }
    }

    public void DeleteData()
    {
        questTitleText.text = "";
        questDetailText.text = "";

        objectiveText.gameObject.SetActive(false);
        objectiveLog.gameObject.SetActive(false);
        textRewardItems.SetActive(false);

        questDetailText.gameObject.SetActive(false);
        questTitleText.gameObject.SetActive(false);

        DestroyOldQuestData();
    }
}