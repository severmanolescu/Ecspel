using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestButton : MonoBehaviour
{
    private Quest quest = null;

    public Quest Quest { get { return quest; } }

    public void SetData(Quest quest, QuestTabDataSet questTabData)
    {
        this.quest = quest;

        gameObject.GetComponent<Button>().onClick.AddListener(delegate { questTabData.SetData(quest); });

        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = quest.Title;
    }
}