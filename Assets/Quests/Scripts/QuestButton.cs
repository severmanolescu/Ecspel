using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestButton : MonoBehaviour
{
    private Quest quest;

    private QuestTabDataSet questTabData;

    public Quest Quest { get { return quest; } }

    public void SetData(Quest quest, QuestTabDataSet questTabData)
    {
        this.quest = quest;
        this.questTabData = questTabData;

        gameObject.GetComponent<Button>().onClick.AddListener(delegate { this.questTabData.SetData(quest); });

        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = quest.Title;
    }
}
