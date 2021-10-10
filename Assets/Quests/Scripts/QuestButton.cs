using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestButton : MonoBehaviour
{
    private Quest quest;

    private TextMeshProUGUI buttonText;

    private Button button;

    private QuestTabDataSet questTabData;

    public Quest Quest { get { return quest; } }

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();

        buttonText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetData(Quest quest, QuestTabDataSet questTabData)
    {
        this.quest = quest;
        this.questTabData = questTabData;

        buttonText.text = quest.Title;

        button.onClick.AddListener(delegate { questTabData.SetData(quest); });
    }
}
