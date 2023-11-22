using UnityEngine;

public class QuestToWorldHandler : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;

    private QuestTabHandler questTab;

    private NpcId npcId;

    public QuestTabHandler QuestTab { get => questTab; set => questTab = value; }

    private void Awake()
    {
        npcId = GameObject.Find("Global").GetComponent<NpcId>();
    }

    private void SetGoToObjective(Objective objective, Quest quest)
    {
        ObjectiveGoTo objectiveGoTo = (ObjectiveGoTo)objective;

        if (objectiveGoTo != null)
        {
            GameObject questWorld = new GameObject();

            questWorld.transform.SetParent(spawnLocation);

            questWorld.transform.localPosition = objectiveGoTo.GoToPoint;
            questWorld.name = objectiveGoTo.name;

            CircleCollider2D circleCollider = questWorld.AddComponent<CircleCollider2D>();

            circleCollider.radius = objectiveGoTo.CircleRadius;

            circleCollider.isTrigger = true;

            GoToObjectiveHandler goToObjective = questWorld.AddComponent<GoToObjectiveHandler>();

            goToObjective.SetData(objectiveGoTo, quest);
        }
    }

    private void SetObjectiveGiveItem(Quest quest)
    {
        QuestGiveItem questGive = (QuestGiveItem)quest.QuestObjective;

        if(questGive != null)
        {
            DialogueDisplay npcDialog = npcId.GetNpcFromId(questGive.NpcID);

            if (npcDialog != null)
            {
                npcDialog.GetComponent<NPCReceiveItem>().AddQuest(quest);
            }
        }
    }

    private void SetObjectiveToWorld(Quest quest)
    {
        if (quest.QuestObjective != null)
        {
            switch (quest.QuestObjective)
            {
                case ObjectiveGoTo:
                    {
                        SetGoToObjective(quest.QuestObjective, quest);

                        break;
                    }
                case QuestGiveItem:
                    {
                        SetObjectiveGiveItem(quest);

                        break;
                    }
            }
        }
    }

    public void SetQuestToWorld(Quest quest)
    {
        if (quest != null)
        {
            SetObjectiveToWorld(quest);

            //questTab.CompletQuest(quest);
        }
    }
}
