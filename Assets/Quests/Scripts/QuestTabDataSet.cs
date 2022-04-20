using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestTabDataSet : MonoBehaviour
{
    [SerializeField] private GameObject textGiveItems;
    [SerializeField] private Transform spawnLocationGiveItems;

    [SerializeField] private GameObject textReceiveItems;
    [SerializeField] private Transform spawnLocationReceiveItems;

    [SerializeField] private GameObject itemSlotPrefab;

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

        textGiveItems.SetActive(false);
        textReceiveItems.SetActive(false);

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

        ItemSlot[] itemSlots = spawnLocationGiveItems.GetComponentsInChildren<ItemSlot>();

        foreach (ItemSlot itemSlot in itemSlots)
        {
            Destroy(itemSlot.gameObject);
        }

        itemSlots = spawnLocationReceiveItems.GetComponentsInChildren<ItemSlot>();

        foreach (ItemSlot itemSlot in itemSlots)
        {
            Destroy(itemSlot.gameObject);
        }

        if (quest is GiveItem)
        {
            GiveItem giveItem = (GiveItem)quest;

            if(giveItem.itemsNeeds.Count > 0)
            {
                textGiveItems.SetActive(true);

                foreach (QuestItems item in giveItem.ItemsNeeds)
                {
                    Item newItem = item.Item.Copy();
                    newItem.Amount = item.Amount;

                    ItemSlot itemSlot = Instantiate(itemSlotPrefab).GetComponent<ItemSlot>();

                    itemSlot.transform.SetParent(spawnLocationGiveItems);

                    itemSlot.SetItem(newItem);

                    itemSlot.DontShowDetails = true;
                }
            }
            else
            {
                textGiveItems.SetActive(false);
            }

            if(quest.itemsReceive.Count > 0)
            {
                textReceiveItems.SetActive(true);

                foreach (QuestItems item in quest.itemsReceive)
                {
                    Item newItem = item.Item.Copy();
                    newItem.Amount = item.Amount;

                    ItemSlot itemSlot = Instantiate(itemSlotPrefab).GetComponent<ItemSlot>();

                    itemSlot.transform.SetParent(spawnLocationReceiveItems);

                    itemSlot.SetItem(newItem);

                    itemSlot.DontShowDetails = true;
                }
            }
            else
            {
                textReceiveItems.SetActive(false);
            }
        }
        else
        {
            textGiveItems.SetActive(false);
            textReceiveItems.SetActive(false);
        }

        //track.gameObject.SetActive(true);
        //if (quest is GiveItem)
        //{
        //    GiveItem giveItem = (GiveItem)quest;

        //    track.onClick.AddListener(delegate { questFollow.StopFollowQuest(); });
        //    track.onClick.AddListener(delegate { questTrack.TrackQuest(npcId.GetNpcFromId(giveItem.whoToGiveId).transform); });
        //}
        //else if(quest is GoToLocation)
        //{
        //    track.onClick.AddListener(delegate { questFollow.StartFollowQuest(quest); });
        //}
        //else if(quest is CutTrees)
        //{
        //    track.gameObject.SetActive(false);
        //}
        //else if (quest is DestroyStone)
        //{
        //    track.gameObject.SetActive(false);
        //}

    }

    public void DeleteData()
    {
        title.text = "";
        details.text = "";

        textGiveItems.SetActive(false);
        textReceiveItems.SetActive(false);

        ItemSlot[] itemSlots = spawnLocationGiveItems.GetComponentsInChildren<ItemSlot>();

        foreach (ItemSlot itemSlot in itemSlots)
        {
            Destroy(itemSlot.gameObject);
        }

        itemSlots = spawnLocationReceiveItems.GetComponentsInChildren<ItemSlot>();

        foreach (ItemSlot itemSlot in itemSlots)
        {
            Destroy(itemSlot.gameObject);
        }

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
