using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDestroyStoneHandle : MonoBehaviour
{
    public void SetDestroyStonequest(Quest quest)
    {
        GameObject @object = Instantiate(new GameObject());

        @object.AddComponent<QuestDestroyStone>().SetQuest((DestroyStone)quest);
    }
}
