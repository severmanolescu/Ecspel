using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCutTreesHandler : MonoBehaviour
{
    public void SetCutTreesQuest(Quest quest)
    {
        GameObject @object = Instantiate(new GameObject());

        @object.AddComponent<QuestCutTrees>().SetQuest((CutTrees)quest);
    }
}
