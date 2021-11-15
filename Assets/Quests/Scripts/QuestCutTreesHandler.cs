using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestCutTreesHandler : MonoBehaviour
{
    public void SetCutTreesQuest(Quest quest)
    {
        GameObject prefabGameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameObject.prefab", typeof(GameObject)); 

        GameObject @object = Instantiate(prefabGameObject);

        @object.AddComponent<QuestCutTrees>().SetQuest((CutTrees)quest);
    }
}
