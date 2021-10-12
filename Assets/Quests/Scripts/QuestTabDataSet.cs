using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTabDataSet : MonoBehaviour
{
    private QuestLocationFollow questLocation;

    private QuestTrack questTrack;

    private TextMeshProUGUI title;

    private TextMeshProUGUI details;

    private Button track;

    private void Awake()
    {
        TextMeshProUGUI[] texts = gameObject.GetComponentsInChildren<TextMeshProUGUI>();

        title = texts[0];
        details = texts[1];

        track = gameObject.GetComponentInChildren<Button>();

        questTrack = GameObject.Find("Player/Canvas/QuestTrack").GetComponent<QuestTrack>();
        questLocation = GameObject.Find("Player").GetComponent<QuestLocationFollow>();

        track.gameObject.SetActive(false);
    }

    public void SetData(Quest quest)
    {
        title.text = quest.Title;
        details.text = quest.Details;

        track.gameObject.SetActive(true);
        if (quest.ItemsNeeds.Count > 0)
        {
            track.onClick.AddListener(delegate { questTrack.TrackQuest(quest.WhoToGive.transform.position); });
        }
        else if(quest.Positions.Count > 0)
        {
            track.onClick.AddListener(delegate { questLocation.ChangeTrack(true); });
        }

    }

    public void DeleteData()
    {
        title.text = "";
        details.text = "";

        track.gameObject.SetActive(false);
    }

    public void SetQuestTrack()
    {
        Button button = gameObject.GetComponent<Button>();

        ColorBlock colorBlock = button.colors;

        colorBlock.normalColor = Color.white;

        button.colors = colorBlock;
    }

    public void DeselectQuestTrack()
    {
        Button button = gameObject.GetComponent<Button>();

        Color color;

        if (ColorUtility.TryParseHtmlString("#8E8E8E", out color))
        {
            ColorBlock colorBlock = button.colors;

            colorBlock.normalColor = color;

            button.colors = colorBlock;

        }
    }
}
