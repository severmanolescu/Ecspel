using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyDropLoot : MonoBehaviour
{
    [SerializeField] private List<QuestItems> drops = new List<QuestItems>();

    private GameObject itemWorldPrefab;

    private void Awake()
    {
        itemWorldPrefab = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Items/ItemWorld.prefab", typeof(GameObject));
    }

    public void DropItem()
    {
        foreach(QuestItems questItems in drops)
        {
            ItemWorld itemWorld = Instantiate(itemWorldPrefab).GetComponent<ItemWorld>();

            Item item = Instantiate(questItems.Item);

            item.Amount = questItems.Drop;

            itemWorld.SetItem(item);

            itemWorld.transform.position = transform.position;

            itemWorld.MoveToPoint();

            return;
        }
    }
}
