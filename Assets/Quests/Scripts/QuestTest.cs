using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTest : MonoBehaviour
{
    public Quest quest;

    private void Start()
    {
        gameObject.GetComponent<QuestTabHandler>().AddQuest(quest);
    }
}
