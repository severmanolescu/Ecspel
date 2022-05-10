using UnityEngine;

public class QuestKillEnemy : MonoBehaviour
{
    private int initalKillMobs;

    private KillEnemy killEnemy = null;

    private PlayerAchievements playerAchievements;

    private void Awake()
    {
        playerAchievements = GameObject.Find("Player").GetComponent<PlayerAchievements>();
    }

    public void SetQuest(KillEnemy killEnemy)
    {
        this.killEnemy = killEnemy;

        initalKillMobs = playerAchievements.GetEnemyKills();
    }

    private void Update()
    {
        if (killEnemy != null)
        {
            if (playerAchievements.CheckForEnemyKill(killEnemy, initalKillMobs))
            {
                GameObject.Find("Player/Canvas/QuestTab").GetComponent<QuestTabHandler>().DeleteQuest(killEnemy);
                GameObject.Find("Player/Canvas/QuestTab").GetComponent<QuestTabHandler>().AddQuest(killEnemy.NextQuest);

                GameObject.Find("Player/Canvas/PlayerItems").GetComponent<PlayerInventory>().AddItem(killEnemy.ItemsReceive);

                Destroy(this.gameObject);
            }

        }
    }
}
