using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestShow : MonoBehaviour
{
    private TextMeshProUGUI title;
    private TextMeshProUGUI details;

    private void Awake()
    {
        title = transform.Find("Title").gameObject.GetComponent<TextMeshProUGUI>();
        details = transform.Find("Details").gameObject.GetComponent<TextMeshProUGUI>();
    }

    public void SetQuest(Quest quest)
    {
        if(quest != null)
        {
            title.text = quest.Title;
            details.text = quest.Details;
        }
    }

    public void HideQuest()
    {
        title.text = "";
        details.text = "";

        gameObject.SetActive(false);
    }
}
