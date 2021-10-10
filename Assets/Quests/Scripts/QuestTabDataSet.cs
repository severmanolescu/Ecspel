using UnityEngine;
using TMPro;

public class QuestTabDataSet : MonoBehaviour
{
    private TextMeshProUGUI title;

    private TextMeshProUGUI details;

    private void Awake()
    {
        TextMeshProUGUI[] texts = gameObject.GetComponentsInChildren<TextMeshProUGUI>();

        title = texts[0];
        details = texts[1];
    }

    public void SetData(Quest quest)
    {
        title.text = quest.Title;
        details.text = quest.Details;
    }

    public void DeleteData()
    {
        title.text = "";
        details.text = "";
    }
}
