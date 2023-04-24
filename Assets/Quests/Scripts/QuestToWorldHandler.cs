using UnityEngine;

public class QuestToWorldHandler : MonoBehaviour
{
    [SerializeField] private Transform spawnLocation;

    private QuestTabHandler questTab;

    public QuestTabHandler QuestTab { get => questTab; set => questTab = value; }

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

    private void SetObjectiveToWorld(Objective objective, Quest quest)
    {
        if (objective != null)
        {
            switch (objective)
            {
                case ObjectiveGoTo:
                    {
                        SetGoToObjective(objective, quest);

                        break;
                    }
            }
        }
    }

    public void SetQuestToWorld(Quest quest)
    {
        if (quest != null)
        {
            foreach (Objective objective in quest.QuestObjectives)
            {
                if (objective != null && objective.Completed == false)
                {
                    SetObjectiveToWorld(objective, quest);

                    return;
                }
            }

            questTab.CompletQuest(quest);
        }
    }
}
