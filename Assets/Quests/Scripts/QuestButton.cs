using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestButton : MonoBehaviour
{
    public Quest quest;

    private Button button;
    private TextMeshProUGUI text;

    public QuestShow questShow;

    public Quest Quest { get { return quest; } }

    private void Awake()
    {
        button = gameObject.GetComponent<Button>();
        text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetQuest(Quest quest, QuestShow questShow)
    {
        if (quest != null)
        {
            this.quest = quest;

            this.questShow = questShow;

            button.onClick.AddListener(delegate { questShow.SetQuest(quest); });

            text.text = quest.Title;
        }
    }
}
