using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTabDataSet : MonoBehaviour
{
    private QuestFollowHandler questFollow;

    private QuestTrack questTrack;

    private TextMeshProUGUI title;

    private TextMeshProUGUI details;

    private Button track;

    private NpcId npcId;

    private void Awake()
    {
        TextMeshProUGUI[] texts = gameObject.GetComponentsInChildren<TextMeshProUGUI>();

        title = texts[0];
        details = texts[1];

        track = gameObject.GetComponentInChildren<Button>();

        questTrack = GameObject.Find("Player/Canvas/QuestTrack").GetComponent<QuestTrack>();
        questFollow = GameObject.Find("Player/QuestFollowObjects").GetComponent<QuestFollowHandler>();

        npcId = GameObject.Find("Global").GetComponent<NpcId>();

        track.gameObject.SetActive(false);
    }

    public void SetData(Quest quest)
    {
        title.text = quest.Title;
        details.text = quest.Details;

        track.gameObject.SetActive(true);
        if (quest is GiveItem)
        {
            GiveItem giveItem = (GiveItem)quest;

            track.onClick.AddListener(delegate { questFollow.StopFollowQuest(); });
            track.onClick.AddListener(delegate { questTrack.TrackQuest(npcId.GetNpcFromId(giveItem.whoToGiveId).transform); });
        }
        else if(quest is GoToLocation)
        {
            track.onClick.AddListener(delegate { questFollow.StartFollowQuest(quest); });
        }
        else if(quest is CutTrees)
        {
            track.gameObject.SetActive(false);
        }
        else if (quest is DestroyStone)
        {
            track.gameObject.SetActive(false);
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
