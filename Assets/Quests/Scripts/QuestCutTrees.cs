using UnityEngine;

public class QuestCutTrees : MonoBehaviour
{
    private int initialCutTrees;

    private CutTrees cutTrees = null;

    private PlayerAchievements playerAchievements;

    private void Awake()
    {
        playerAchievements = GameObject.Find("Player").GetComponent<PlayerAchievements>();
    }

    public void SetQuest(CutTrees cutTrees)
    {
        this.cutTrees = cutTrees;

        initialCutTrees = playerAchievements.Trees;
    }

    private void Update()
    {
        if (cutTrees != null)
        {
            if (playerAchievements.Trees >= initialCutTrees + cutTrees.Number)
            {
                GameObject.Find("Player/Canvas/QuestTab").GetComponent<QuestTabHandler>().DeleteQuest(cutTrees);
                GameObject.Find("Player/Canvas/QuestTab").GetComponent<QuestTabHandler>().DeleteQuest(cutTrees.nextQuest);

                GameObject.Find("Player/Canvas/PlayerItems").GetComponent<PlayerInventory>().AddItem(cutTrees.itemsReceive);

                Destroy(this.gameObject);
            }
        }
    }
}
