using UnityEngine;

public class QuestDestroyStone : MonoBehaviour
{
    private int initialDestroyStone;

    private DestroyStone destroyStone = null;

    private PlayerAchievements playerAchievements;

    private void Awake()
    {
        playerAchievements = GameObject.Find("Player").GetComponent<PlayerAchievements>();
    }

    public void SetQuest(DestroyStone cutTrees)
    {
        this.destroyStone = cutTrees;

        initialDestroyStone = playerAchievements.Stones;
    }

    private void Update()
    {
        if (destroyStone != null)
        {
            if (playerAchievements.Stones >= initialDestroyStone + destroyStone.Number)
            {
                GameObject.Find("Player/Canvas/QuestTab").GetComponent<QuestTabHandler>().DeleteQuest(destroyStone);
                GameObject.Find("Player/Canvas/QuestTab").GetComponent<QuestTabHandler>().DeleteQuest(destroyStone.NextQuest);

                GameObject.Find("Player/Canvas/PlayerItems").GetComponent<PlayerInventory>().AddItem(destroyStone.itemsReceive);

                Destroy(this.gameObject);
            }
        }
    }
}
