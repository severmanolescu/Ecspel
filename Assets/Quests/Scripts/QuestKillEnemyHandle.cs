using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestKillEnemyHandle : MonoBehaviour
{
    public void SetKillEnemyQuest(Quest quest)
    {
        GameObject prefabGameObject = new GameObject();

        GameObject @object = Instantiate(prefabGameObject);

        @object.AddComponent<QuestKillEnemy>().SetQuest((KillEnemy)quest);
    }
}
