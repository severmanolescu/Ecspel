using UnityEngine;

public class GoToObjectiveHandler : MonoBehaviour
{
    private QuestToWorldHandler questToWorld;

    private ObjectiveGoTo objective;

    private Quest quest;

    private void Awake()
    {
        questToWorld = GameObject.Find("Global").GetComponent<QuestToWorldHandler>();
    }

    public void SetData(ObjectiveGoTo objective, Quest quest)
    {
        this.quest = quest;

        this.objective = objective;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            objective.Completed = true;

            questToWorld.SetQuestToWorld(quest);

            Destroy(gameObject);
        }
    }
}
