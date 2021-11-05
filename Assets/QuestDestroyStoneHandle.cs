using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class QuestDestroyStoneHandle : MonoBehaviour
{
    public void SetDestroyStonequest(Quest quest)
    {
        GameObject prefabGameObject = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/GameObject.prefab", typeof(GameObject));

        GameObject @object = Instantiate(prefabGameObject);

        @object.AddComponent<QuestDestroyStone>().SetQuest((DestroyStone)quest);
    }
}
